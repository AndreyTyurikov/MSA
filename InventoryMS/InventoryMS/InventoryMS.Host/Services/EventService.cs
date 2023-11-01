using InventoryMS.Events;

namespace InventoryMS.Host.Services
{
    public class EventService : IEventService
    {
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
    }
}
