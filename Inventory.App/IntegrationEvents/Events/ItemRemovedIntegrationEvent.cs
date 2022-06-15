
using Oths.EventBus.Events;

namespace Inventory.App.IntegrationEvents.Events
{
    public record ItemRemovedIntegrationEvent : IntegrationEvent
    {
        public int ItemId { get; set; }

        public ItemRemovedIntegrationEvent(int itemId)
        {
            ItemId = itemId;
        }
    }
}
