using DiscountMS.Host.Domain.Model;

namespace DiscountMS.Host.Domain.DataLayer
{
    public interface IDiscountDataLayer
    {
        Task<Tuple<Discount, InventoryItemDiscount>> AddInventoryItemDiscount(Discount baseDiscountPart, InventoryItemDiscount specificDiscountPart);
        Task<Tuple<Discount, PersonalDiscount>> AddPersonalDiscount(Discount baseDiscountPart, PersonalDiscount specificDiscountPart);
        Task<List<Tuple<Discount, InventoryItemDiscount?>>> GetAllActiveInventoryItemDiscounts();
        Task<List<Tuple<Discount, PersonalDiscount?>>> GetAllActivePersonalDiscounts();
    }
}
