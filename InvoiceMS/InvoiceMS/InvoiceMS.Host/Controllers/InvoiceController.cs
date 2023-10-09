using InvoiceMS.Contracts;
using Microsoft.AspNetCore.Mvc;
using InvoiceMS.Infrastructure.Services;

namespace InvoiceMS.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        // GET: api/<InvoiceController>
        [HttpGet("byUser/{id}")]
        public async Task<IEnumerable<InvoiceDTO>> GetByUserId(long id)
        {
            List<InvoiceDTO> invoicesByUser = await _invoiceService.GetInvoicesByUser(id);

            return invoicesByUser;
        }

        // GET api/<InvoiceController>/5
        [HttpGet("{id}")]
        public async Task<InvoiceDTO> Get(long id)
        {
            InvoiceDTO invoiceById = await _invoiceService.GetInvoiceById(id);

            return invoiceById;
        }

        // POST api/<InvoiceController>
        [HttpPost]
        public async Task<InvoiceDTO> Post([FromBody] AddInvoiceDTO invoiceToAdd)
        {
            InvoiceDTO addedInvoice = await _invoiceService.AddInvoice(invoiceToAdd);

            return addedInvoice;
        }

        // DELETE api/<InvoiceController>/5
        //TODO: Удалять только неоплаченные инвойсы
        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            bool isInvoiceDeleted = await _invoiceService.DeleteInvoiceById(id);

            return isInvoiceDeleted;
        }
    }
}
