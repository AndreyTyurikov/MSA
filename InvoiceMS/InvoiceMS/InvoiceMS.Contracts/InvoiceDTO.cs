using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceMS.Contracts
{
    public class InvoiceDTO
    {
        public long UserId { get; set; }
        public InvoiceEntryDTO[] InvoiceEntries { get; set; } = Array.Empty<InvoiceEntryDTO>();
        public DateTime IssueDate { get; set; }
    }
}
