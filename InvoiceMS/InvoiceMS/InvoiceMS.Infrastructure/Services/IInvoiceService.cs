using InvoiceMS.Contracts;
using InvoiceMS.Infrastructure.EventProcessors;

namespace InvoiceMS.Infrastructure.Services
{
    public interface IInvoiceService
    {
        Task<InvoiceDTO> AddInvoice(AddInvoiceDTO invoiceToAdd);
        Task<bool> DeleteInvoiceById(int id);
        Task<InvoiceDTO> GetInvoiceById(long id);
        Task<List<InvoiceDTO>> GetInvoicesByUser(long id);
    }
}
