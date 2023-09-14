using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceMS.Contracts
{
    public class InvoiceEntry
    {
        //ID of goods
        public long InventoryId { get; set; }
        //Amount of goods to add to invoice
        public int Amount { get; set; }
    }
}
