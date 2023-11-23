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
        public PersonalDiscountDTO[] GetActivePersonalDiscounts()
        {
            PersonalDiscountDTO[] tmpResults = { 
                new PersonalDiscountDTO {
                    DiscountID = 1,
                    DiscountAmountType = DiscountAmountType.Percentage,
                    DiscountAmount = 25.00M,
                    DateFrom = DateTime.Today.AddHours(9),
                    DateTo = null,
                    TerminationType = DiscountTerminationType.Never,
                    PersonalDiscountId = 1,
                    UserID = 1,
                }
            };

            return tmpResults;
        }

        [HttpGet("inventory_item")]
        public InventoryItemDiscountDTO[] GetActiveInventoryItemDiscounts()
        {
            InventoryItemDiscountDTO[] tmpResults = {

                new InventoryItemDiscountDTO {
                    DiscountID = 2,
                    DiscountAmountType = DiscountAmountType.FixedAmount,
                    DiscountAmount = 6000,
                    DateFrom = DateTime.Today.AddHours(9),
                    DateTo = DateTime.Today.AddHours(18),
                    TerminationType = DiscountTerminationType.OutOfStock,
                    InventoryItemDiscountId = 1,
                    InventoryID = 1
                }
            };

            return tmpResults;
        }


        // GET api/<DiscountController>/5
        //[HttpGet("{id}")]
        //public DiscountDTO Get(int id)
        //{
        //    return null;
        //}

        [HttpPost("personal")]
        public DiscountDTO AddPersonalDiscount([FromBody] AddPersonalDiscountDTO discountToAdd)
        {
            return null;
        }

        [HttpPost("inventory_item")]
        public DiscountDTO AddInventoryItemDiscount([FromBody] AddInventoryItemDiscountDTO discountToAdd)
        {
            return null;
        }
    }
}
