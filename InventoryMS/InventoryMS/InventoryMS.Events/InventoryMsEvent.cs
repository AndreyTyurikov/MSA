namespace InventoryMS.Events
{
    public class InventoryMsEvent
    {
        public DateTime eventDateTime { get; set; } = DateTime.Now;

        public EventType eventType { get; set; }

        public object eventPayload { get; set; }
    }
}