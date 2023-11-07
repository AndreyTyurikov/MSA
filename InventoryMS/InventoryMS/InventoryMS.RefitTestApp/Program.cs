using InventoryMS.Contracts;
using InventoryMS.Host.Domain.Models;

EditInventoryItemDTO dto = new EditInventoryItemDTO() { Id = 1, Stock = 10 };

bool ContainsUpdates = dto.ContainsUpdates();
var propertiesForUpdate = dto.GetPropertiesForUpdate();

//InventoryItem itemToUpdate = new InventoryItem();

//itemToUpdate.UpdateFromEditInventoryItemDto(dto);

var a = 10;