using InventoryMS.Contracts;
using InventoryMS.Host.Domain.Models;

//EditInventoryItemDTO dto = new EditInventoryItemDTO() { Id = 1, Stock = 10 };

//bool ContainsUpdates = dto.ContainsUpdates();
//var propertiesForUpdate = dto.GetPropertiesForUpdate();

//InventoryItem itemToUpdate = new InventoryItem();

//itemToUpdate.UpdateFromEditInventoryItemDto(dto);

Console.WriteLine("Start of main Thread");

Thread thread = new Thread(() => {

    Console.WriteLine("Start of second Thread");

    Thread.Sleep(3000);

    Console.WriteLine("End of second Thread");

});


thread.IsBackground = false; //this makes thread foreground

thread.Start();

Console.WriteLine("End of main Thread");

var a = 10;