using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Web.Models
{
    public class ItemTypeCreateRequest
    {
        [Required]
        [StringLength(50, ErrorMessage = "Code length error, allowed [3,50] ", MinimumLength = 3)]
        public string Code { get; set; }

        public string Description { get; set; }
    }
}
