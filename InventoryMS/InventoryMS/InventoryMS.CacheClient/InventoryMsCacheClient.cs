using InventoryMS.Contracts;
using StackExchange.Redis;
using System.Text.Json;

namespace InventoryMS.CacheClient
{
    public class InventoryMsCacheClient : IInventoryMsCacheClient
    {
        private readonly ConnectionMultiplexer _redisConnection;
        private readonly IDatabase _redisDb;

        public InventoryMsCacheClient()
        {
            _redisConnection = ConnectionMultiplexer.Connect("localhost");
            _redisDb = _redisConnection.GetDatabase(1);
        }

        ~InventoryMsCacheClient()
        {
            _redisConnection.Close();
        }

        public InventoryItemDTO? GetInventoryItem(long id)
        {
            InventoryItemDTO? itemById = null;

            try
            {
                RedisValue objectById = _redisDb.StringGet(BuildInventoryKey(id));

                if (objectById.HasValue) itemById = JsonSerializer.Deserialize<InventoryItemDTO>(objectById);
            }
            catch (Exception)
            {
                throw;
            }

            return itemById;
        }

        private RedisKey BuildInventoryKey(long id)
        {
            return new RedisKey($"Inventory: {id}");
        }

    }
}