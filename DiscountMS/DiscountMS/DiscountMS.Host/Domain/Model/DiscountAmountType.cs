using System.ComponentModel.DataAnnotations;

namespace DiscountMS.Host.Domain.Model
{
    //Fixed / Percents
    public class DiscountAmountType
    {
        [Key]
        public int DiscountAmountTypeId { get; set; }

        [Required]
        public string DiscountAmountTypeName { get; set; }
    }
}
