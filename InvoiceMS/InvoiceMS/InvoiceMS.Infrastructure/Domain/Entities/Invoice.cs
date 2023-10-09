using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceMS.Infrastructure.Domain.Entities
{
    public class Invoice
    {
        [Key]
        public long InvoiceId { get; set; }
        [Required]
        public string InvoiceNumber { get; set; }
        [Required]
        public long UserId { get; set; }
        public ICollection<InvoiceEntry> InvoiceEntries { get; } = new List<InvoiceEntry>();
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime IssueDate { get; set; }
        //TODO: Хранить интервал годности инвойса в настройках приложения
        //TODO: Учитывать банковские дни
        [Required]
        public DateTime ExpirationDate { get; set; }
    }
}
