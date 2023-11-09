using InventoryMS.Events;

namespace InvoiceMS.Infrastructure
{
    public interface IInvetoryServiceEventsProcessor
    {
        Task<bool> ProcessEvent(InventoryMsEvent eventReceived);
    }
}