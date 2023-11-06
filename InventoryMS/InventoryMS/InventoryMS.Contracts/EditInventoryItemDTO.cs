using System.Reflection;

namespace InventoryMS.Contracts
{
    public class EditInventoryItemDTO
    {
        //Developer note: Please don't rename this field
        public long Id { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }

        public bool IsUpdated()        
        {           
            foreach (PropertyInfo pi in GetPropertiesForUpdate())
            {
                if (pi.GetValue(this, null) != null)
                {
                    return true;
                }
            }

            return false;
        }

        public List<PropertyInfo> GetPropertiesForUpdate()
        {
            return this.GetType().GetProperties().Where(p => p.Name != "Id").ToList();
        }
    }
}