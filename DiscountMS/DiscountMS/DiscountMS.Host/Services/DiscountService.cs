using DiscountMS.Contracts;
using DiscountMS.Host.Domain.DataLayer;
using DiscountMS.Host.Domain.Model;

namespace DiscountMS.Host.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IDiscountDataLayer _discountDataLayer;

        public DiscountService(IDiscountDataLayer discountDataLayer) {
            _discountDataLayer = discountDataLayer;
        }

        public async Task<InventoryItemDiscountDTO> AddInventoryItemDiscount(AddInventoryItemDiscountDTO discountToAdd)
        {
            //TODO: Create both object from DTO
            Discount baseDiscountPart = new Discount();
            InventoryItemDiscount specificDiscountPart = new InventoryItemDiscount();

            Tuple<Discount, InventoryItemDiscount> addedDiscount = 
                await _discountDataLayer.AddInventoryItemDiscount(baseDiscountPart, specificDiscountPart);

            return null;
        }

        public async Task<PersonalDiscountDTO> AddPersonalDiscount(AddPersonalDiscountDTO discountToAdd)
        {
            Discount baseDiscountPart = new Discount {

                DiscountType = new DiscountType { DiscountTypeId = (int)discountToAdd.DiscountType },
                DiscountAmountType = new DiscountAmountType { DiscountAmountTypeId = (int)discountToAdd.DiscountAmountType },
                DiscountAmount = discountToAdd.DiscountAmount,
                DateFrom = discountToAdd.DateFrom,
                DateTo = discountToAdd.DateTo,
                DiscountTerminationType = 
                    new DiscountTerminationType { DiscountTerminationTypeId = (int)discountToAdd.DiscountTerminationType  },

            };

            PersonalDiscount specificDiscountPart = new PersonalDiscount { 

                UserID = discountToAdd.UserID

            };

            Tuple<Discount, PersonalDiscount> addedDiscount =
                await _discountDataLayer.AddPersonalDiscount(baseDiscountPart, specificDiscountPart);

            PersonalDiscountDTO addedDiscountDTO = BuildPersonalDiscountDtoFromDbParts(baseDiscountPart, specificDiscountPart);

            return addedDiscountDTO;
        }

        private PersonalDiscountDTO BuildPersonalDiscountDtoFromDbParts(Discount baseDiscountPart, PersonalDiscount specificDiscountPart)
        {
            return new PersonalDiscountDTO()
            {
                DiscountID = baseDiscountPart.DiscountId,
                DiscountAmountType = (Contracts.Enums.DiscountAmountTypeEnum)baseDiscountPart.DiscountAmountType.DiscountAmountTypeId,
                DiscountAmount = baseDiscountPart.DiscountAmount,
                DateFrom = baseDiscountPart.DateFrom,
                DateTo = baseDiscountPart.DateTo,
                TerminationType = (Contracts.Enums.DiscountTerminationTypeEnum)baseDiscountPart.DiscountTerminationType.DiscountTerminationTypeId,
                PersonalDiscountId = specificDiscountPart.PersonalDiscountId,
                UserID = specificDiscountPart.UserID
            };
        }

        public async Task<PersonalDiscountDTO[]> GetActivePersonalDiscounts()
        {
            List<Tuple<Discount, PersonalDiscount?>> personalDiscountsParts = await _discountDataLayer.GetAllActivePersonalDiscounts();

            List<PersonalDiscountDTO> activeDiscountsDTOs = new List<PersonalDiscountDTO>();

            foreach (Tuple<Discount, PersonalDiscount?> discountParts in personalDiscountsParts)
            {
                PersonalDiscountDTO discountDTO = new PersonalDiscountDTO()
                {
                    DiscountID = discountParts.Item1.DiscountId,
                    DiscountAmountType = (Contracts.Enums.DiscountAmountTypeEnum)discountParts.Item1.DiscountAmountType.DiscountAmountTypeId,
                    DiscountAmount = discountParts.Item1.DiscountAmount,
                    DateFrom = discountParts.Item1.DateFrom,
                    DateTo = discountParts.Item1.DateTo,
                    TerminationType = (Contracts.Enums.DiscountTerminationTypeEnum)discountParts.Item1.DiscountTerminationType.DiscountTerminationTypeId,
                    PersonalDiscountId = discountParts.Item2?.PersonalDiscountId,
                    UserID = discountParts.Item2?.UserID
                };

                activeDiscountsDTOs.Add(discountDTO);
            }

            return activeDiscountsDTOs.ToArray();
        }

        public async Task<InventoryItemDiscountDTO[]> GetAllActiveInventoryItemDiscounts()
        {
            List<Tuple<Discount, InventoryItemDiscount?>> inventoryItemDiscountsParts = await _discountDataLayer.GetAllActiveInventoryItemDiscounts();

            List<InventoryItemDiscountDTO> activeDiscountsDTOs = new List<InventoryItemDiscountDTO>();

            foreach (Tuple<Discount, InventoryItemDiscount?> discountParts in inventoryItemDiscountsParts)
            {
                InventoryItemDiscountDTO discountDTO = new InventoryItemDiscountDTO() {
                    DiscountID = discountParts.Item1.DiscountId,
                    DiscountAmountType = (Contracts.Enums.DiscountAmountTypeEnum)discountParts.Item1.DiscountAmountType.DiscountAmountTypeId,
                    DiscountAmount = discountParts.Item1.DiscountAmount,
                    DateFrom = discountParts.Item1.DateFrom,
                    DateTo = discountParts.Item1.DateTo,
                    TerminationType = (Contracts.Enums.DiscountTerminationTypeEnum)discountParts.Item1.DiscountTerminationType.DiscountTerminationTypeId,
                    InventoryItemDiscountId = discountParts.Item2?.InventoryItemDiscountId,
                    InventoryID = discountParts.Item2?.InventoryID
                };

                activeDiscountsDTOs.Add(discountDTO);
            }

            return activeDiscountsDTOs.ToArray();
        }
    }
}
