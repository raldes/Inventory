using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Dtos
{
    public class ItemCreateRequest
    {
        [Required]
        [StringLength(50, ErrorMessage = "Name length error, allowed [3,50] ", MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        public int ItemTypeId { get; set; }

        public string Description { get; set; }

    }
}
