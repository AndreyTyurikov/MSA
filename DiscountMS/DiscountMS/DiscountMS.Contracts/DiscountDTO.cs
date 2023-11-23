using DiscountMS.Contracts.Enums;

namespace DiscountMS.Contracts
{
    public abstract class DiscountDTO
    {
        public long DiscountID { get; set; }
        public DiscountTypeEnum DiscountType { get; protected set; }
        public DiscountAmountType DiscountAmountType { get; set; }
        public decimal DiscountAmount { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public DiscountTerminationType TerminationType { get; set; }
    }
}
