
using Oths.EventBus.Events;

namespace Inventory.App.IntegrationEvents.Events
{
    public record ItemCreatedIntegrationEvent : IntegrationEvent
    {
        public int ItemId { get; set; }

        public ItemCreatedIntegrationEvent(int itemId)
        {
            ItemId = itemId;
        }

    }
}
