using InventoryMS.Contracts;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace InventoryMS.Host.Domain.Models
{
    public class InventoryItem
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }

        public InventoryItem UpdateFromEditInventoryItemDto(EditInventoryItemDTO editInventoryItemDTO)
        {
            List<PropertyInfo> propertiesToUpdate = editInventoryItemDTO.GetPropertiesForUpdate();

            if (propertiesToUpdate.Count > 0) {

                Type typeOfIventoryItem = this.GetType();
                Type typeOfEditInvItemDTO = editInventoryItemDTO.GetType();

                foreach (var property in propertiesToUpdate)
                {
                    object? valueToCopy = typeOfEditInvItemDTO.GetProperty(property.Name)?.GetValue(editInventoryItemDTO, null);

                    PropertyInfo? propertyToUpdate = typeOfIventoryItem.GetProperty(property.Name);

                    if (propertyToUpdate != null)
                    {
                        propertyToUpdate.SetValue(this, valueToCopy, null);               
                    }
                }
            }

            return this;
        }
    }
}
