using InvoiceMS.Infrastructure.Domain.DbCtx;
using InvoiceMS.Infrastructure.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;

namespace InvoiceMS.Infrastructure.DataLayer
{
    public class InvoiceDataLayer : IInvoiceDataLayer
    {
        public async Task<Invoice> AddInvoice(Invoice newInvoice, List<InvoiceEntry> newInvoiceEntries)
        {
            using (InvoiceMsDbContext dbContext = new InvoiceMsDbContext())
            {
                IDbContextTransaction transaction = dbContext.Database.BeginTransaction();

                try
                {
                    dbContext.Invoices.Add(newInvoice);
                    await dbContext.SaveChangesAsync();

                    //If invoice added to DB
                    if (newInvoice.InvoiceId > 0)
                    {
                        foreach (InvoiceEntry invoiceEntry in newInvoiceEntries)
                        {
                            //Привязываем записи инвойса к Id добавленного в базу инвойса
                            invoiceEntry.InvoiceId = newInvoice.InvoiceId;
                        }

                        dbContext.InvoiceEntries.AddRange(newInvoiceEntries);
                        await dbContext.SaveChangesAsync();

                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();

                    throw;
                }
                
                return newInvoice;
            }
        }

        public async Task<bool> DeleteInvoiceById(int id)
        {
            bool isInvoiceDeleted = false;  

            using (InvoiceMsDbContext dbContext = new InvoiceMsDbContext())
            {
                Invoice? invoiceById = dbContext.Invoices.Where(i => i.InvoiceId == id).Include(i => i.InvoiceEntries).FirstOrDefault();

                if (invoiceById != null)
                {
                    IDbContextTransaction transaction = dbContext.Database.BeginTransaction();

                    try
                    {
                        if(invoiceById.InvoiceEntries.Count > 0) {
                            dbContext.InvoiceEntries.RemoveRange(invoiceById.InvoiceEntries);                            
                        }
                       
                        dbContext.Invoices.Remove(invoiceById);
                        await dbContext.SaveChangesAsync();

                        transaction.Commit();

                        isInvoiceDeleted = true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();

                        throw;
                    }
                }

                return isInvoiceDeleted;
            }
        }

        public async Task<Invoice> GetInvoiceById(long id)
        {
            using (InvoiceMsDbContext dbContext = new InvoiceMsDbContext()) {

                Invoice? invoiceById = await dbContext.Invoices.Where(i => i.InvoiceId == id).Include(i => i.InvoiceEntries).FirstOrDefaultAsync();

                return invoiceById != null ? invoiceById : new Invoice();

            }
        }

        public async Task<List<InvoiceEntry>> GetInvoiceEntriesByInventoryItemID(long itemId)
        {
            using (InvoiceMsDbContext dbContext = new InvoiceMsDbContext())
            {
                List<InvoiceEntry> invoiceEntriesByInventoryItemID
                    = await dbContext.InvoiceEntries.Where(ie => ie.InventoryId == itemId).ToListAsync();

                return invoiceEntriesByInventoryItemID;
            }
        }

        public async Task<List<Invoice>> GetInvoicesByUserId(long id)
        {
            using (InvoiceMsDbContext dbContext = new InvoiceMsDbContext())
            {
                List<Invoice> invoicesByUserID = new List<Invoice>();

                if (dbContext.Invoices.Any(i => i.UserId == id))
                {
                    invoicesByUserID = await dbContext.Invoices.Where(i => i.UserId == id).Include(i => i.InvoiceEntries).ToListAsync();
                }

                return invoicesByUserID;
            }
        }

        public Task SaveUpdatedInvoiceEntries(List<InvoiceEntry> invoiceEntriesByInventoryItemID)
        {
            using (InvoiceMsDbContext dbContext = new InvoiceMsDbContext())
            {
                dbContext.InvoiceEntries.UpdateRange(invoiceEntriesByInventoryItemID);

                dbContext.SaveChanges();

                return Task.CompletedTask;
            }
        }
    }
}
