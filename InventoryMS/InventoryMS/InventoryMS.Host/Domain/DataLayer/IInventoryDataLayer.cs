using InventoryMS.Host.Domain.Models;

namespace InventoryMS.Host.Domain.DataLayer
{
    public interface IInventoryDataLayer
    {
        Task<InventoryItem> Add(InventoryItem newIntoryItem);
        Task<List<InventoryItem>> All();
        Task<List<InventoryItem>> ByIds(long[] ids);
    }
}
