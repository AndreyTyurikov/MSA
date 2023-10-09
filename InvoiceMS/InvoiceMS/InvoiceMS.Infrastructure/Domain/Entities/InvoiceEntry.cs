using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceMS.Infrastructure.Domain.Entities
{
    public class InvoiceEntry
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public long InvoiceId { get; set; }
        [Required]
        public long InventoryId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Amount { get; set; }
    }
}
