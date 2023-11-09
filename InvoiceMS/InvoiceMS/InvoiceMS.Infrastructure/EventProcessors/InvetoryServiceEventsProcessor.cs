using InventoryMS.Events;
using InvoiceMS.Infrastructure.Services;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace InvoiceMS.Infrastructure.EventProcessors
{
    public class InvetoryServiceEventsProcessor : IInvetoryServiceEventsProcessor
    {
        private readonly IInventoryUpdatesNotificationsProcessor _updatesNotificationProcess;

        public InvetoryServiceEventsProcessor(IInventoryUpdatesNotificationsProcessor notificationsProcessor)
        {
            _updatesNotificationProcess = notificationsProcessor;
        }

        public async Task<bool> ProcessEvent(InventoryMsEvent eventReceived)
        {
            switch (eventReceived.eventType)
            {
                case EventType.InventoryItemNameUpdated:
                    {
                        InventoryItemNameUpdatedPayload? eventPayload = 
                            JsonSerializer.Deserialize<InventoryItemNameUpdatedPayload>(eventReceived.eventPayload);

                        if (eventPayload != null)
                        {
                            InventoryItemNameUpdatedNotification updateNotification = new InventoryItemNameUpdatedNotification
                            {
                                ItemId = eventPayload.ItemId,
                                OldName = eventPayload.OldName,
                                NewName = eventPayload.NewName,
                            };

                            await _updatesNotificationProcess.ProcessInventoryItemNameUpdatedNotification(updateNotification);
                        }

                        break;
                    }
                case EventType.InventoryItemPriceUpdated:
                    {
                        InventoryItemPriceUpdatedPayload? eventPayload = 
                            JsonSerializer.Deserialize<InventoryItemPriceUpdatedPayload>(eventReceived.eventPayload);

                        if (eventPayload != null)
                        {
                            InventoryItemPriceUpdatedNotification updateNotification = new InventoryItemPriceUpdatedNotification
                            {
                                ItemId = eventPayload.ItemId,
                                OldPrice = eventPayload.OldPrice,
                                NewPrice = eventPayload.NewPrice,
                            };

                            await _updatesNotificationProcess.ProcessInventoryItemPriceUpdatedNotification(updateNotification);

                            Debug.WriteLine($"Inventory Item {eventPayload.ItemId} price updated to {eventPayload.NewPrice}");
                        }

                        break;
                    }
            }

            return true;
        }
    }
}
