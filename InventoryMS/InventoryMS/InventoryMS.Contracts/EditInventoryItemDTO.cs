using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

namespace InventoryMS.Contracts
{
    public class EditInventoryItemDTO
    {
        //Developer note: Please don't rename this field
        public long Id { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public int? Stock { get; set; }

        public bool ContainsUpdates()        
        {
            return GetPropertiesForUpdate().Count > 0;
        }

        public List<PropertyInfo> GetPropertiesForUpdate()
        {
            //All object properties without Key property
            List<PropertyInfo> valueProperties = this.GetType().GetProperties().Where(p => p.Name != "Id").ToList();

            List<PropertyInfo> propertiesForUpdate = new List<PropertyInfo>();       

            if (valueProperties.Count > 0)
            {
                foreach (PropertyInfo property in valueProperties)
                {
                    if (property.GetValue(this, null) != null)
                    {
                        propertiesForUpdate.Add(property);
                    }
                }
            }

            return propertiesForUpdate;
        }
    }
}