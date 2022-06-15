using Inventory.Domain.Dtos;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Inventory.App.Commands
{
    public class CreateItemCommand : IRequest<ItemDto>
    {
        [Required]
        [StringLength(50, ErrorMessage = "Name length error, allowed [3,50] ", MinimumLength = 3)]
        public string Name { get; set; }

        public DateTime ExpirationDate { get; set; }

        [Required]
        public int ItemTypeId { get; set; }

        public string Description { get; set; }

    }
}
