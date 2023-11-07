using InventoryMS.Host.Domain.DbCtx;
using InventoryMS.Host.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace InventoryMS.Host.Domain.DataLayer
{
    public class InventoryDataLayer : IInventoryDataLayer
    {
        public async Task<InventoryItem> Add(InventoryItem newIntoryItem)
        {
            using (InventoryMsDbContext dbContext = new InventoryMsDbContext())
            {
                dbContext.InventoryItems.Add(newIntoryItem);

                await dbContext.SaveChangesAsync();

                return newIntoryItem;
            }
        }

        public async Task<List<InventoryItem>> All()
        {
            using (InventoryMsDbContext dbContext = new InventoryMsDbContext())
            {
                return await dbContext.InventoryItems.ToListAsync(); 
            }
        }

        public async Task<InventoryItem> ById(long id)
        {
            using (InventoryMsDbContext dbContext = new InventoryMsDbContext())
            {
                InventoryItem? inventoryItemById = await dbContext.InventoryItems.FirstOrDefaultAsync(i => i.Id == id);

                return inventoryItemById != null ? inventoryItemById : new InventoryItem();
            }
        }

        public async Task<List<InventoryItem>> ByIds(long[] ids)
        {
            using (InventoryMsDbContext dbContext = new InventoryMsDbContext())
            {
                List<InventoryItem> itemsByIds = new List<InventoryItem>();

                var searchResults = dbContext.InventoryItems.Where(i => ids.Contains(i.Id));

                if (searchResults.Any())
                    itemsByIds = await searchResults.ToListAsync();

                return itemsByIds;
            }
        }

        public async Task<bool> Update(InventoryItem updatedInventoryItem)
        {
            using (InventoryMsDbContext dbContext = new InventoryMsDbContext())
            {
                int rowsUpdates = 0;

                dbContext.InventoryItems.Update(updatedInventoryItem);

                rowsUpdates = await dbContext.SaveChangesAsync();

                return rowsUpdates > 0;
            }
        }
    }
}
