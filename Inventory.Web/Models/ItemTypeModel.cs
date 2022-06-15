using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Inventory.Web.Models
{
    public class ItemTypeModel 
    {
        public Guid ItemTypeId { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }
 
    }
}
