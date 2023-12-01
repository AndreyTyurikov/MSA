using DiscountMS.Contracts;
using DiscountMS.Host.Domain.DataLayer;
using DiscountMS.Host.Domain.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            Discount baseDiscountPart = BuildDiscountObjectFromAddDiscountDTO(discountToAdd);

            InventoryItemDiscount specificDiscountPart = new InventoryItemDiscount {
            
                InventoryID = discountToAdd.InventoryID

            };

            Tuple<Discount, InventoryItemDiscount> addedDiscount = 
                await _discountDataLayer.AddInventoryItemDiscount(baseDiscountPart, specificDiscountPart);

            InventoryItemDiscountDTO inventoryItemDiscountDTO 
                = BuildInventoryItemDiscountDtoFromDbParts(addedDiscount.Item1, addedDiscount.Item2);

            return inventoryItemDiscountDTO;
        }

        private Discount BuildDiscountObjectFromAddDiscountDTO(AddDiscountDTO addDiscountDTO)
        {
            return new Discount()
            {
                DiscountType = new DiscountType { DiscountTypeId = (int)addDiscountDTO.DiscountType },
                DiscountAmountType = new DiscountAmountType { DiscountAmountTypeId = (int)addDiscountDTO.DiscountAmountType },
                DiscountAmount = addDiscountDTO.DiscountAmount,
                DateFrom = addDiscountDTO.DateFrom,
                DateTo = addDiscountDTO.DateTo,
                DiscountTerminationType =
                    new DiscountTerminationType { DiscountTerminationTypeId = (int)addDiscountDTO.DiscountTerminationType }
            };
        }

        public async Task<PersonalDiscountDTO> AddPersonalDiscount(AddPersonalDiscountDTO discountToAdd)
        {
            Discount baseDiscountPart = BuildDiscountObjectFromAddDiscountDTO(discountToAdd);

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

        private InventoryItemDiscountDTO BuildInventoryItemDiscountDtoFromDbParts(Discount baseDiscountPart, InventoryItemDiscount specificDiscountPart)
        {
            InventoryItemDiscountDTO inventoryItemDiscountDTO = BuildDiscountDtoBase<InventoryItemDiscountDTO>(baseDiscountPart);

            inventoryItemDiscountDTO.InventoryItemDiscountId = specificDiscountPart.InventoryItemDiscountId;
            inventoryItemDiscountDTO.InventoryID = specificDiscountPart.InventoryID;

            return inventoryItemDiscountDTO;
        }

        private T BuildDiscountDtoBase<T>(Discount baseDiscountPart) where T : DiscountDTO
        {
            T result = Activator.CreateInstance<T>();

            result.DiscountID = baseDiscountPart.DiscountId;
            result.DiscountAmountType = (Contracts.Enums.DiscountAmountTypeEnum)baseDiscountPart.DiscountAmountType.DiscountAmountTypeId;
            result.DiscountAmount = baseDiscountPart.DiscountAmount;
            result.DateFrom = baseDiscountPart.DateFrom;
            result.DateTo = baseDiscountPart.DateTo;
            result.TerminationType = (Contracts.Enums.DiscountTerminationTypeEnum)baseDiscountPart.DiscountTerminationType.DiscountTerminationTypeId;

            return result;
        }

        public async Task<PersonalDiscountDTO[]> GetActivePersonalDiscounts()
        {
            List<Tuple<Discount, PersonalDiscount?>> personalDiscountsParts = await _discountDataLayer.GetAllActivePersonalDiscounts();

            List<PersonalDiscountDTO> activeDiscountsDTOs = new List<PersonalDiscountDTO>();

            foreach (Tuple<Discount, PersonalDiscount?> discountParts in personalDiscountsParts)
            {
                PersonalDiscountDTO discountDTO = BuildPersonalDiscountDtoFromDbParts(discountParts.Item1, discountParts.Item2);

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
