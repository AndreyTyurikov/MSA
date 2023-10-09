using InvoiceMS.Contracts;
using UserMS.DTO;
using UserMS.Client;
using InventoryMS.Contracts;
using InventoryMS.Client;
using System.Linq;
using InvoiceMS.Infrastructure.Domain.Entities;

namespace InvoiceMS.Infrastructure.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IUserMsClient _userMsClient;
        private readonly IInventoryMSClient _inventoryMSClient;

        public InvoiceService(IUserMsClient userMsClient) { 
            _userMsClient = userMsClient;
            _inventoryMSClient = InventoryMsClient.Client;
        }

        public async Task<InvoiceDTO> AddInvoice(AddInvoiceDTO invoiceToAdd)
        {/*
          * #4 Make invoice
          * #5 Save invoice
          * #6 Return invoice DTO
          */
            UserDTO userByID = await _userMsClient.GetUserByID(invoiceToAdd.UserId);

            //Check whether user exists and invoice has items
            if (userByID.Id > 0 && invoiceToAdd.InvoiceEntries.Length > 0) 
            {
                List<InventoryItemDTO> inventoryItems = 
                    await _inventoryMSClient.SearchByIdsAsync(invoiceToAdd.InvoiceEntries.Select(e => e.InventoryId).ToArray());

                if (inventoryItems.Count > 0)
                {
                    //Lookup for fast search items by Inventory ID
                    ILookup<long, InventoryItemDTO> inventoryItemsLookup = inventoryItems.ToLookup(i => i.Id);

                    //Storage for future invoice items
                    List<InvoiceEntry> newInvoiceEntries = new List<InvoiceEntry>();

                    foreach (AddInvoiceEntry invoiceEntry in invoiceToAdd.InvoiceEntries)
                    {
                        var inventoryItemToAdd = inventoryItemsLookup[invoiceEntry.InventoryId].FirstOrDefault();

                        if (inventoryItemToAdd != null)
                        {
                            newInvoiceEntries.Add(new InvoiceEntry { 
                                InventoryId = inventoryItemToAdd.Id,
                                Name = inventoryItemToAdd.Name,
                                Price = inventoryItemToAdd.Price,
                                Amount = invoiceEntry.Amount,
                            });
                        }
                    }

                    //We do have items for new invoice
                    if (newInvoiceEntries.Count > 0)
                    { 
                        //TODO: Save invoice to DB
                    }
                }
            }

            return new InvoiceDTO();
        }

        public Task<bool> DeleteInvoiceById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<InvoiceDTO> GetInvoiceById(long id)
        {
            throw new NotImplementedException();
        }

        public Task<List<InvoiceDTO>> GetInvoicesByUser(long id)
        {
            throw new NotImplementedException();
        }
    }
}
