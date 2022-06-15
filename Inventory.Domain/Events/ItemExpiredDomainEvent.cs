using Inventory.Domain.Entities;
using MediatR;

namespace Inventory.Domain.Events
{
    public class ItemExpiredDomainEvent : INotification
    {
        public Item Item { get; set; }
        public ItemExpiredDomainEvent(Item item)
        {
            Item = item;
        }
    }
}
