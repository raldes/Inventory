using Inventory.Domain.Entities;
using MediatR;

namespace Inventory.Domain.Events
{
    public class ItemUpdatedDomainEvent : INotification
    {
        public Item Item { get; set; }
        public ItemUpdatedDomainEvent(Item item)
        {
            Item = item;
        }
    }
}
