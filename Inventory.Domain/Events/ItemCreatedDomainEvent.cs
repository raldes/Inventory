
using MediatR;

namespace Inventory.Domain.Events
{
    public class ItemCreatedDomainEvent : INotification
    {
        public int ItemId { get; }

        public string Name { get; }

        public string Description { get; }

        public DateTime ExpirationDate { get; }

        public int ItemTypeId { get; }

        public ItemCreatedDomainEvent(
                string name,
                string description,
                DateTime expirationDate, 
                int itemTypeId
            )
        {
            Name = name;
            Description = description;
            ExpirationDate = expirationDate;
            ItemTypeId = itemTypeId;
        }
    }
}
