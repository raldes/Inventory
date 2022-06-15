using Oths.EventBus.Events;

namespace Inventory.App.IntegrationEvents.Events
{
    public record ItemExpiredIntegrationEvent : IntegrationEvent
    {
        public int ItemId { get; }

        public ItemExpiredIntegrationEvent(int itemId) =>
            ItemId = itemId;
    }
}

