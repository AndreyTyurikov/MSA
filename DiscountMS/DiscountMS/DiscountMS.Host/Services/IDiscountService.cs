using DiscountMS.Contracts;

namespace DiscountMS.Host.Services
{
    public interface IDiscountService
    {
        Task<InventoryItemDiscountDTO> AddInventoryItemDiscount(AddInventoryItemDiscountDTO discountToAdd);
        Task<PersonalDiscountDTO> AddPersonalDiscount(AddPersonalDiscountDTO discountToAdd);
        Task<PersonalDiscountDTO[]> GetActivePersonalDiscounts();
        Task<InventoryItemDiscountDTO[]> GetAllActiveInventoryItemDiscounts();
    }
}
