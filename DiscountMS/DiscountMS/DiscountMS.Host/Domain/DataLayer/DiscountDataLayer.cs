using DiscountMS.Host.Domain.Model;
using DiscountMS.Host.Domain.DbCtx;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace DiscountMS.Host.Domain.DataLayer
{
    public class DiscountDataLayer : IDiscountDataLayer
    {
        public Task<Tuple<Discount, InventoryItemDiscount>> AddInventoryItemDiscount(Discount baseDiscountPart, InventoryItemDiscount specificDiscountPart)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<DiscountType?, DiscountAmountType?, DiscountTerminationType?>> 
            GetDiscountCategories(int discountTypeId, int discountAmountTypeId, int discountTerminationTypeId)
        {
            using (DiscountServiceDbContext dbContext = new DiscountServiceDbContext())
            {
                DiscountType? discountType = dbContext.DiscountTypes.Where(dt => dt.DiscountTypeId == discountTypeId).FirstOrDefault();
                
                DiscountAmountType? discountAmountType
                    = dbContext.DiscountAmountTypes.Where(dt => dt.DiscountAmountTypeId == discountAmountTypeId).FirstOrDefault();
                
                DiscountTerminationType? discountTermination
                    = dbContext.DiscountTerminationTypes.Where(dt => dt.DiscountTerminationTypeId == discountTerminationTypeId).FirstOrDefault();

                return Task.FromResult(new Tuple<DiscountType?, DiscountAmountType?, DiscountTerminationType?>
                    (discountType, discountAmountType, discountTermination));
            }              
        }

        public async Task<Tuple<Discount, PersonalDiscount>> AddPersonalDiscount(Discount baseDiscountPart, PersonalDiscount specificDiscountPart)
        {
            using (DiscountServiceDbContext dbContext = new DiscountServiceDbContext())
            {
                using (IDbContextTransaction addDiscountTransaction = dbContext.Database.BeginTransaction())
                {
                    try {
                        
                        dbContext.PersonalDiscounts.Add(specificDiscountPart);
                        await dbContext.SaveChangesAsync();

                        //Personal discount was added 
                        if (specificDiscountPart.PersonalDiscountId > 0)
                        {
                            DiscountType? discountType = dbContext.DiscountTypes
                                .Where(dt => dt.DiscountTypeId == baseDiscountPart.DiscountType.DiscountTypeId).FirstOrDefault();

                            DiscountAmountType? discountAmountType = dbContext.DiscountAmountTypes
                                .Where(dt => dt.DiscountAmountTypeId == baseDiscountPart.DiscountAmountType.DiscountAmountTypeId).FirstOrDefault();

                            DiscountTerminationType? discountTerminationType = dbContext.DiscountTerminationTypes
                                .Where(dt => dt.DiscountTerminationTypeId == baseDiscountPart.DiscountTerminationType.DiscountTerminationTypeId).FirstOrDefault();

                            baseDiscountPart.DiscountType = discountType;
                            baseDiscountPart.DiscountAmountType = discountAmountType;
                            baseDiscountPart.DiscountTerminationType = discountTerminationType;

                            baseDiscountPart.SpecificDiscountTableKey = specificDiscountPart.PersonalDiscountId;

                            dbContext.Discounts.Add(baseDiscountPart);
                            await dbContext.SaveChangesAsync();
                        }
                                              
                        addDiscountTransaction.Commit();

                        return new Tuple<Discount, PersonalDiscount>(baseDiscountPart, specificDiscountPart);

                    }
                    catch {

                        addDiscountTransaction.Rollback();

                        throw;
                    }
                }
            }
        }

        public Task<List<Tuple<Discount, InventoryItemDiscount?>>> GetAllActiveInventoryItemDiscounts()
        {
            List<Tuple<Discount, InventoryItemDiscount?>> activeDiscounts = new List<Tuple<Discount, InventoryItemDiscount?>>();

            using (DiscountServiceDbContext dbContext = new DiscountServiceDbContext())
            {
                List<Discount> inventoryDiscountsBaseParts 
                    = dbContext.Discounts
                                        .Include(d => d.DiscountAmountType)
                                        .Include(d => d.DiscountType)
                                        .Include(d => d.DiscountTerminationType)
                                        .Where(d => d.DiscountType.DiscountTypeId == 2).ToList();

                if (inventoryDiscountsBaseParts.Count > 0)
                {
                    long[] inventoryDiscountKeys = inventoryDiscountsBaseParts.Select(d => d.SpecificDiscountTableKey).ToArray();

                    ILookup<long, InventoryItemDiscount> inventoryDiscountsSpecificParts
                        = dbContext.InventoryItemDiscounts
                                        .Where(d => inventoryDiscountKeys.Contains(d.InventoryItemDiscountId))
                                        .ToLookup(d => d.InventoryItemDiscountId);

                    foreach (Discount inventoryDiscountBasePart in inventoryDiscountsBaseParts)
                    {
                        InventoryItemDiscount? inventoryDiscountSpecificPart
                            = inventoryDiscountsSpecificParts[inventoryDiscountBasePart.SpecificDiscountTableKey].FirstOrDefault();

                        activeDiscounts.Add(new Tuple<Discount, InventoryItemDiscount?>(inventoryDiscountBasePart, inventoryDiscountSpecificPart));
                    }
                }
            }

            return Task.FromResult(activeDiscounts);
        }

        public Task<List<Tuple<Discount, PersonalDiscount?>>> GetAllActivePersonalDiscounts()
        {
            List<Tuple<Discount, PersonalDiscount?>> activeDiscounts = new List<Tuple<Discount, PersonalDiscount?>>();

            using (DiscountServiceDbContext dbContext = new DiscountServiceDbContext())
            {
                List<Discount> personalDiscountsBaseParts
                    = dbContext.Discounts
                                        .Include(d => d.DiscountAmountType)
                                        .Include(d => d.DiscountType)
                                        .Include(d => d.DiscountTerminationType)
                                        .Where(d => d.DiscountType.DiscountTypeId == 1).ToList();

                if (personalDiscountsBaseParts.Count > 0)
                {
                    long[] inventoryDiscountKeys = personalDiscountsBaseParts.Select(d => d.SpecificDiscountTableKey).ToArray();

                    ILookup<long, PersonalDiscount> personalDiscountsSpecificParts
                        = dbContext.PersonalDiscounts
                                        .Where(d => inventoryDiscountKeys.Contains(d.PersonalDiscountId))
                                        .ToLookup(d => d.PersonalDiscountId);

                    foreach (Discount personalDiscountBasePart in personalDiscountsBaseParts)
                    {
                        PersonalDiscount? personalDiscountSpecificPart
                            = personalDiscountsSpecificParts[personalDiscountBasePart.SpecificDiscountTableKey].FirstOrDefault();

                        activeDiscounts.Add(new Tuple<Discount, PersonalDiscount?>(personalDiscountBasePart, personalDiscountSpecificPart));
                    }
                }
            }

            return Task.FromResult(activeDiscounts);
        }
    }
}
