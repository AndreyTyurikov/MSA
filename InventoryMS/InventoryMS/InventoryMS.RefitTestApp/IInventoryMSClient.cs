using InventoryMS.Contracts;
using Refit;

namespace InventoryMS.RefitTestApp
{
    public interface IInventoryMSClient
    {
        [Get("/api/inventory")]
        public Task<List<InventoryItemDTO>> GetAll();

        [Post("/api/inventory/SearchByIds")]
        public Task<List<InventoryItemDTO>> SearchByIdsAsync([Body] long[] ids);
    }
}
