using InvoiceMS.Contracts;
using UserMS.DTO;
using UserMS.Client;
using InventoryMS.Contracts;
using InventoryMS.Client;
using System.Linq;
using InvoiceMS.Infrastructure.Domain.Entities;
using InvoiceMS.Infrastructure.DataLayer;
using Mapster;
using System.Collections.Generic;
using InvoiceMS.Infrastructure.EventProcessors;

namespace InvoiceMS.Infrastructure.Services
{
    public class InvoiceService : IInvoiceService, IInventoryUpdatesNotificationsProcessor
    {
        private readonly IUserMsClient _userMsClient;
        private readonly IInventoryMSClient _inventoryMSClient;
        private readonly IInvoiceDataLayer _invoiceDataLayer;

        public InvoiceService(IUserMsClient userMsClient, IInvoiceDataLayer invoiceDataLayer) { 
            _userMsClient = userMsClient;
            _inventoryMSClient = InventoryMsClient.Client;
            _invoiceDataLayer = invoiceDataLayer;
        }

        public async Task<InvoiceDTO> AddInvoice(AddInvoiceDTO addInvoiceDto)
        {
            InvoiceDTO addedInvoiceDTO = new InvoiceDTO();
            
            UserDTO userByID = await _userMsClient.GetUserByID(addInvoiceDto.UserId);

            //Check whether user exists and invoice has items
            if (userByID.Id > 0 && addInvoiceDto.InvoiceEntries.Length > 0) 
            {
                List<InventoryItemDTO> inventoryItems = 
                    await _inventoryMSClient.SearchByIdsAsync(addInvoiceDto.InvoiceEntries.Select(e => e.InventoryId).ToArray());

                if (inventoryItems.Count > 0)
                {
                    //Lookup for fast search items by Inventory ID
                    ILookup<long, InventoryItemDTO> inventoryItemsLookup = inventoryItems.ToLookup(i => i.Id);

                    //Storage for future invoice items
                    List<InvoiceEntry> newInvoiceEntries = new List<InvoiceEntry>();

                    foreach (AddInvoiceEntry invoiceEntry in addInvoiceDto.InvoiceEntries)
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
                        Invoice newInvoice = new Invoice {
                            InvoiceNumber = $"{addInvoiceDto.UserId}-{DateTime.Now.ToString("yyyy-MM-dd-HH-mm")}",
                            UserId = addInvoiceDto.UserId,
                            IssueDate = DateTime.Now,
                            //TODO: Take invoice exp. period from app settings
                            ExpirationDate = DateTime.Now.AddDays(3)
                        };

                        Invoice addedInvoice = await _invoiceDataLayer.AddInvoice(newInvoice, newInvoiceEntries);

                        addedInvoiceDTO = addedInvoice.Adapt<InvoiceDTO>();
                    }
                }
            }

            return addedInvoiceDTO;
        }

        public async Task<bool> DeleteInvoiceById(int id)
        {
            bool isInvoiceDeleted = await _invoiceDataLayer.DeleteInvoiceById(id);

            return isInvoiceDeleted;
        }

        public async Task<InvoiceDTO> GetInvoiceById(long id)
        {
            Invoice invoiceById = await _invoiceDataLayer.GetInvoiceById(id);

            return invoiceById.Adapt<InvoiceDTO>();
        }

        public async Task<List<InvoiceDTO>> GetInvoicesByUser(long id)
        {
            List<Invoice> invoicesByUserId = await _invoiceDataLayer.GetInvoicesByUserId(id);

            return invoicesByUserId.Adapt<List<InvoiceDTO>>();
        }

        public Task ProcessInventoryItemNameUpdatedNotification(InventoryItemNameUpdatedNotification updateNotification)
        {
            throw new NotImplementedException();
        }

        public Task ProcessInventoryItemPriceUpdatedNotification(InventoryItemPriceUpdatedNotification updateNotification)
        {
            throw new NotImplementedException();
        }
    }
}
