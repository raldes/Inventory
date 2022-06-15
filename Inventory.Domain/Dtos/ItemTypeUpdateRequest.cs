using System;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Domain.Dtos
{
    public class ItemTypeUpdateRequest
    {
        [Required]
        public int ItemTypeId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Code length error, allowed [3,50] ", MinimumLength = 3)]
        public string Code { get; set; }

        public string Description { get; set; }
    }
}
