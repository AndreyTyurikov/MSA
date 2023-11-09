namespace InventoryMS.Events
{
    public class InventoryMsEvent
    {
        public DateTime eventDateTime { get; set; } = DateTime.Now;

        public EventType eventType { get; set; }

        public string eventPayload { get; set; }
    }
}