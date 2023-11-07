using InventoryMS.Contracts;
using InventoryMS.Events;
using InventoryMS.Host.Domain.Models;
using InventoryMS.Host.MessageBroker;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Reflection;

namespace InventoryMS.Host.Services
{
    public class EventService : IEventService
    {
        private readonly IMessageBusProducer _messageBusProducer;

        public EventService(IMessageBusProducer messageBusProducer) {
            _messageBusProducer = messageBusProducer;
        }

        public InventoryMsEvent CreateInventoryItemNameUpdatedEvent(long Id, string OldName, string NewName)
        {
            return new InventoryMsEvent
            {
                eventType = EventType.InventoryItemNameUpdated,
                eventPayload = new InventoryItemNameUpdatedPayload
                {
                    ItemId = Id,
                    OldName = OldName,
                    NewName = NewName
                }
            };
        }

        public InventoryMsEvent CreateInventoryItemPriceUpdatedEvent(long Id, decimal OldPrice, decimal NewPrice)
        {
            return new InventoryMsEvent
            {
                eventType = EventType.InventoryItemPriceUpdated,
                eventPayload = new InventoryItemPriceUpdatedPayload
                {
                    ItemId = Id,
                    OldPrice = OldPrice,
                    NewPrice = NewPrice
                }
            };
        }

        public Task<bool> ProcessAndPublishInventoryItemUpdates(InventoryItem inventoryItemBeforeUpdate, EditInventoryItemDTO inventoryItemUpdates)
        {
            try
            {
                List<InventoryMsEvent> eventsToPublish = new List<InventoryMsEvent>();

                if (inventoryItemUpdates.Name != null)
                {
                    eventsToPublish.Add(
                        this.CreateInventoryItemNameUpdatedEvent(
                            Id: inventoryItemUpdates.Id,
                            OldName: inventoryItemBeforeUpdate.Name,
                            NewName: inventoryItemUpdates.Name
                            )
                        );
                }

                if (inventoryItemUpdates.Price.HasValue)
                {
                    eventsToPublish.Add(
                        this.CreateInventoryItemPriceUpdatedEvent(
                            Id: inventoryItemUpdates.Id,
                            OldPrice: inventoryItemBeforeUpdate.Price,
                            NewPrice: (decimal)inventoryItemUpdates.Price
                            )
                        );
                }

                if (eventsToPublish.Count > 0)
                {
                    Parallel.ForEach(eventsToPublish, (evnt) =>
                    {
                        _messageBusProducer.PublishEvent(evnt);
                    });
                }

                return Task.FromResult(true);
            }
            catch (Exception) {
                throw;
            }
        }
    }
}
