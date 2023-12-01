using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountMS.Contracts
{
    public class AddInventoryItemDiscountDTO : AddDiscountDTO
    {
        public AddInventoryItemDiscountDTO()
        {
            DiscountType = Enums.DiscountTypeEnum.InventoryItem;
        }

        public long InventoryID { get; set; }
    }
}
