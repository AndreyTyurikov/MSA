using InvoiceMS.Infrastructure.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceMS.Infrastructure.DataLayer
{
    public interface IInvoiceDataLayer
    {
        Task<Invoice> AddInvoice(Invoice newInvoice, List<InvoiceEntry> newInvoiceEntries);
        Task<Invoice> GetInvoiceById(long id);
    }
}
