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
using UserMS.CacheClient;

namespace InvoiceMS.Infrastructure.Services
{
    public class InvoiceService : IInvoiceService, IInventoryUpdateNotificationsProcessor
    {
        private readonly IUserMsClient _userMsClient;
        private readonly IUserMsCacheClient _userMsCacheClient;
        private readonly IInventoryMSClient _inventoryMSClient;
        private readonly IInvoiceDataLayer _invoiceDataLayer;

        public InvoiceService(
            IUserMsClient userMsClient, 
            IInvoiceDataLayer invoiceDataLayer,
            IUserMsCacheClient userMsCacheClient
            ) { 
            _userMsClient = userMsClient;
            _userMsCacheClient = userMsCacheClient;
            _inventoryMSClient = InventoryMsClient.Client;
            _invoiceDataLayer = invoiceDataLayer;
        }

        public async Task<InvoiceDTO> AddInvoice(AddInvoiceDTO addInvoiceDto)
        {
            InvoiceDTO addedInvoiceDTO = new InvoiceDTO();

            UserDTO userByID = await GetUserById(addInvoiceDto.UserId);

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

        private async Task<UserDTO> GetUserById(long userId)
        {
            //Cache first
            UserDTO? userFromCache = _userMsCacheClient.GetUser(userId);

            if (userFromCache != null) return userFromCache;

            UserDTO userById = await _userMsClient.GetUserByID(userId);

            return userById != null ? userById : new UserDTO();
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

        public async Task ProcessInventoryItemNameUpdatedNotification(InventoryItemNameUpdatedNotification updateNotification)
        {
            //All invoices holding specific InventoryItem udpated.
            List<InvoiceEntry> invoiceEntriesByInventoryItemID = 
                await _invoiceDataLayer.GetInvoiceEntriesByInventoryItemID(updateNotification.ItemId);

            foreach (InvoiceEntry invoiceEntry in invoiceEntriesByInventoryItemID)
            {
                invoiceEntry.Name = updateNotification.NewName;
            }

            await _invoiceDataLayer.SaveUpdatedInvoiceEntries(invoiceEntriesByInventoryItemID);

            return;
        }

        public async Task ProcessInventoryItemPriceUpdatedNotification(InventoryItemPriceUpdatedNotification updateNotification)
        {
            List<InvoiceEntry> invoiceEntriesByInventoryItemID =
                await _invoiceDataLayer.GetInvoiceEntriesByInventoryItemID(updateNotification.ItemId);

            foreach (InvoiceEntry invoiceEntry in invoiceEntriesByInventoryItemID)
            {
                invoiceEntry.Price = updateNotification.NewPrice;
            }

            await _invoiceDataLayer.SaveUpdatedInvoiceEntries(invoiceEntriesByInventoryItemID);

            return;
        }
    }
}
