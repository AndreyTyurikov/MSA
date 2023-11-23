using DiscountMS.Contracts.Enums;

namespace DiscountMS.Contracts
{
    public class PersonalDiscountDTO : DiscountDTO
    {
        public long PersonalDiscountId { get; set; }

        public long UserID { get; set; }

        public PersonalDiscountDTO() {
            DiscountType = DiscountTypeEnum.Personal;
        }
    }
}
