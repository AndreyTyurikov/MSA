using InvoiceMS.Infrastructure.Domain.DbCtx;
using InvoiceMS.Infrastructure.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<Invoice> GetInvoiceById(long id)
        {
            using (InvoiceMsDbContext dbContext = new InvoiceMsDbContext()) {

                Invoice? invoiceById = await dbContext.Invoices.Where(i => i.InvoiceId == id).Include(i => i.InvoiceEntries).FirstOrDefaultAsync();

                //if (invoiceById != null)
                //{
                //    invoiceById.InvoiceEntries = await dbContext.InvoiceEntries.Where(i => i.InvoiceId == id).ToListAsync();    
                //}

                return invoiceById != null ? invoiceById : new Invoice();

            }
        }
    }
}
