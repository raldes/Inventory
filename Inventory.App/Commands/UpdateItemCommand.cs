using Inventory.Domain.Dtos;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Inventory.App.Commands
{
    public class UpdateItemCommand : IRequest<ItemDto>
    {
        [Required]
        public int ItemId { get; set; }

        [Required]
        public int ItemTypeId { get; set; }

        public DateTime ExpirationDate { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Code length error, allowed [3,50] ", MinimumLength = 3)]
        public string Name { get; set; }

        [StringLength(50, ErrorMessage = "Code length error, allowed [3,50] ", MinimumLength = 3)]
        public string Description { get; set; }

    }
}
