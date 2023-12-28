using InventoryMS.Contracts;

namespace InventoryMS.CacheClient
{
    public interface IInventoryMsCacheClient
    {
        InventoryItemDTO? GetInventoryItem(long id);
    }
}