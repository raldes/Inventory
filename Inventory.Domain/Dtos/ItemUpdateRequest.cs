using System.ComponentModel.DataAnnotations;

namespace Inventory.Domain.Dtos
{
    public class ItemUpdateRequest
    {
        [Required]
        public int ItemId { get; set; }

        [Required]
        public int ItemTypeId { get; set; }

        public DateTime ExpirationDate { get; set; }

        public int ItemStatusId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Code length error, allowed [3,50] ", MinimumLength = 3)]
        public string Name { get; set; }

        [StringLength(50, ErrorMessage = "Code length error, allowed [3,50] ", MinimumLength = 3)]
        public string Description { get; set; }
    }
}
