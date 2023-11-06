using InventoryMS.Contracts;
using InventoryMS.Host.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InventoryMS.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        // GET: api/<InventoryController>
        [HttpGet]
        public async Task<List<InventoryItemDTO>> Get()
        {
            return await _inventoryService.GetAll();
        }

        // POST api/<InventoryController>
        [HttpPost]
        public async Task<InventoryItemDTO> Post([FromBody] AddInventoryItemDTO inventoryItemToAdd)
        {
            return await _inventoryService.AddInventoryItem(inventoryItemToAdd);
        }

        [HttpPut]
        public async Task<bool> Edit([FromBody] EditInventoryItemDTO editInventoryItemDTO)
        {
            return await _inventoryService.UpdateInventoryItem(editInventoryItemDTO);
        }

        [HttpPost("SearchByIds")]
        public async Task<List<InventoryItemDTO>> SearchByIdsAsync([FromBody] long[] ids)
        {
            return await _inventoryService.GetByIds(ids);
        }
    }
}
