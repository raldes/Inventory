using Inventory.Domain.Dtos;
using Inventory.Domain.Entities;
using MediatR;

namespace Inventory.App.Commands
{
    public class RemoveItemCommand : IRequest<ItemDto>
    {
        public Item Item { get; set; }
 
        public RemoveItemCommand(Item item)
        {
            Item = item;
        }
    }
}
