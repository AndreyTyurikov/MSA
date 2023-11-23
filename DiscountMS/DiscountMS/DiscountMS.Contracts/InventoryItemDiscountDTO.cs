using DiscountMS.Contracts.Enums;

namespace DiscountMS.Contracts
{
    public class InventoryItemDiscountDTO : DiscountDTO
    {
        public long InventoryItemDiscountId { get; set; }
        public long InventoryID { get; set; }

        public InventoryItemDiscountDTO()
        {
            DiscountType = DiscountTypeEnum.InventoryItem;
        }
    }
}
