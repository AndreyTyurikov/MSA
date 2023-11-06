using InventoryMS.Contracts;
using InventoryMS.Host.Domain.Models;

EditInventoryItemDTO dto = new EditInventoryItemDTO() { Id = 1, Name = "Updated Name", Price = 200 };

InventoryItem itemToUpdate = new InventoryItem();

itemToUpdate.UpdateFromEditInventoryItemDto(dto);

var a = 10;