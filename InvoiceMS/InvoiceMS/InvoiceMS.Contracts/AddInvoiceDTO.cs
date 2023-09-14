namespace InvoiceMS.Contracts
{
    public class AddInvoiceDTO
    {
        public long UserId { get; set; }
        public InvoiceEntry[] InvoiceEntries { get; set; }
        public DateTime IssueDate { get; set; }
    }
}