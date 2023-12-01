using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountMS.Contracts
{
    public class AddPersonalDiscountDTO : AddDiscountDTO
    {
        public AddPersonalDiscountDTO()
        {
            DiscountType = Enums.DiscountTypeEnum.Personal;
        }

        public long UserID { get; set; }
    }
}
