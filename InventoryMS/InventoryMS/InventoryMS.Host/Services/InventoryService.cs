﻿using InventoryMS.Contracts;
using InventoryMS.Host.Domain.DataLayer;
using InventoryMS.Host.Domain.Models;
using Mapster;
using System.Collections.Generic;

namespace InventoryMS.Host.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryDataLayer _inventoryDataLayer;
        private readonly IEventService _eventService;

        public InventoryService(IInventoryDataLayer inventoryDataLayer, IEventService eventService)
        {
            _inventoryDataLayer = inventoryDataLayer;
            _eventService = eventService;
        }

        public async Task<InventoryItemDTO> AddInventoryItem(AddInventoryItemDTO inventoryItemToAdd)
        {
            InventoryItem newInventoryItem = inventoryItemToAdd.Adapt<InventoryItem>();

            await _inventoryDataLayer.Add(newInventoryItem);

            return newInventoryItem.Adapt<InventoryItemDTO>();
        }

        public async Task<List<InventoryItemDTO>> GetAll()
        {
            List<InventoryItem> inventoryItems = await _inventoryDataLayer.All();

            return inventoryItems.Adapt<List<InventoryItemDTO>>();
        }

        public async Task<List<InventoryItemDTO>> GetByIds(long[] ids)
        {
            List<InventoryItem> inventoryItemsByIds = await _inventoryDataLayer.ByIds(ids);

            return inventoryItemsByIds.Adapt<List<InventoryItemDTO>>();
        }

        public async Task<bool> UpdateInventoryItem(EditInventoryItemDTO editInventoryItemDTO)
        {
            if (editInventoryItemDTO.ContainsUpdates())
            {
                if (editInventoryItemDTO.Id > 0)
                {
                    InventoryItem inventoryItemByID = await _inventoryDataLayer.ById(editInventoryItemDTO.Id);

                    if (inventoryItemByID.Id > 0)
                    {
                        //Note: we don't await this Task. "Fire and forget" mode
                        _eventService.ProcessAndPublishInventoryItemUpdates(inventoryItemByID, editInventoryItemDTO);

                        inventoryItemByID.UpdateFromEditInventoryItemDto(editInventoryItemDTO);

                        return await _inventoryDataLayer.Update(inventoryItemByID);
                    }
                }
            }

            return false;
        }
    }
}
