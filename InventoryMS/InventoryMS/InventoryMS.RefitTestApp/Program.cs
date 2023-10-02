using InventoryMS.Client;

var inventoryServiceClient = InventoryMsClient.Client;

var inventoryItems = await inventoryServiceClient.SearchByIdsAsync(new[] { 1L, 2L, 3L });

var a = 1;