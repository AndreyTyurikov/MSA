using InvoiceMS.Contracts;
using Microsoft.AspNetCore.Mvc;
using InvoiceMS.Infrastructure.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InvoiceMS.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        //private readonly IInvoiceService _invoiceService;

        //public InvoiceController(IInvoiceService invoiceService)
        //{
        //    _invoiceService = invoiceService;
        //}

        // GET: api/<InvoiceController>
        [HttpGet("byUser/{id}")]
        public IEnumerable<string> GetByUserId(long id)
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<InvoiceController>/5
        [HttpGet("{id}")]
        public string Get(long id)
        {
            return "value";
        }

        // POST api/<InvoiceController>
        [HttpPost]
        public async void Post([FromBody] AddInvoiceDTO invoiceToAdd)
        {
        }

        // PUT api/<InvoiceController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<InvoiceController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
