using DiscountMS.Contracts;
using DiscountMS.Contracts.Enums;
using DiscountMS.Host.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DiscountMS.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountService _discountService;

        public DiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        // GET: api/<DiscountController>
        [HttpGet("personal")]
        public async Task<PersonalDiscountDTO[]> GetActivePersonalDiscounts()
        {
            PersonalDiscountDTO[] allActivePersonalDiscounts = await _discountService.GetActivePersonalDiscounts();

            return allActivePersonalDiscounts;
        }

        [HttpGet("inventory_item")]
        public async Task<InventoryItemDiscountDTO[]> GetActiveInventoryItemDiscounts()
        {
            InventoryItemDiscountDTO[] allActiveInventoryItemDiscounts = await _discountService.GetAllActiveInventoryItemDiscounts();

            return allActiveInventoryItemDiscounts;
        }

        [HttpPost("personal")]
        public async Task<PersonalDiscountDTO> AddPersonalDiscount([FromBody] AddPersonalDiscountDTO discountToAdd)
        {
            PersonalDiscountDTO addedPersonalDiscount = await _discountService.AddPersonalDiscount(discountToAdd);

            return addedPersonalDiscount;
        }

        [HttpPost("inventory_item")]
        public async Task<InventoryItemDiscountDTO> AddInventoryItemDiscount([FromBody] AddInventoryItemDiscountDTO discountToAdd)
        {
            InventoryItemDiscountDTO addedInventoryItemDiscount = await _discountService.AddInventoryItemDiscount(discountToAdd);

            return addedInventoryItemDiscount;
        }
    }
}
