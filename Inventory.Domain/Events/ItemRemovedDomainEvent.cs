
using Inventory.Domain.Entities;
using MediatR;

namespace Inventory.Domain.Events
{
    public class ItemRemovedDomainEvent : INotification
    {
        public Item Item { get; }

        public ItemRemovedDomainEvent(Item item)
        {
            Item = item;
        }
    }
}
