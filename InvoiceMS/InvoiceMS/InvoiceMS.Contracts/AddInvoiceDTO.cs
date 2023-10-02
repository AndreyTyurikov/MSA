namespace InvoiceMS.Contracts
{
    public class AddInvoiceDTO
    {
        public long UserId { get; set; }
        public AddInvoiceEntry[] InvoiceEntries { get; set; }
    }
}