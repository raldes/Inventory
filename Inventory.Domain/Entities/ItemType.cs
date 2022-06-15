using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Inventory.Domain.Entities
{
    public class ItemType : EFEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemTypeId { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        [JsonIgnore]
        public virtual ICollection<Item> Items { get; set; }
    }
}
