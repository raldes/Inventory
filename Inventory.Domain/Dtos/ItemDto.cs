using Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Dtos
{
    public class ItemDto 
    {
        public int ItemId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime ExpirationDate { get; set; }

        public ItemStatus ItemStatus { get; set; }

        public int ItemTypeId { get; set; }

        public string Code { get; set; }


        public virtual ItemTypeDto ItemType { get; set; }
    }
}
