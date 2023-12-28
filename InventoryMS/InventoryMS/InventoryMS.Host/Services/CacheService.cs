using InventoryMS.Contracts;
using InventoryMS.Host.Domain.DataLayer;
using InventoryMS.Host.Domain.Models;
using Mapster;
using StackExchange.Redis;
using System.Text.Json;

namespace InventoryMS.Host.Services
{
    public class CacheService
    {
        private readonly IInventoryDataLayer _inventoryDataLayer;
        private readonly ConnectionMultiplexer _redisConnection;
        private readonly IDatabase _redisDb;

        public CacheService()
        {
            _inventoryDataLayer = new InventoryDataLayer();

            _redisConnection = ConnectionMultiplexer.Connect("localhost");
            _redisDb = _redisConnection.GetDatabase(1);
        }

        public async Task<bool> PutInventoryToCache()
        {
            List<InventoryItem> inventoryItems = await _inventoryDataLayer.All();

            string itemJson = string.Empty;
            InventoryItemDTO itemDTO;

            foreach (InventoryItem inventoryItem in inventoryItems)
            {
                itemDTO = inventoryItem.Adapt<InventoryItemDTO>();
                itemJson = JsonSerializer.Serialize(itemDTO);

                _redisDb.StringSet(BuildInventoryKey(inventoryItem.Id), new RedisValue(itemJson));
            }

            Thread.Sleep(3000);

            return true;
        }

        private RedisKey BuildInventoryKey(long id)
        {
            return new RedisKey($"Inventory: {id}");
        }
    }
}
