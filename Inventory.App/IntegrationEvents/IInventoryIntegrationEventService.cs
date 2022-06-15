using Inventory.App.IntegrationEvents.Events;
using Oths.EventBus.Events;

namespace Inventory.App.IntegrationEvents;

public interface IInventoryIntegrationEventService
{
    Task PublishEventsThroughEventBusAsync(Guid transactionId);
    Task PublishEventsThroughEventBusAsync(ItemCreatedIntegrationEvent integrationEvent);
    Task PublishEventsThroughEventBusAsync(ItemRemovedIntegrationEvent integrationEvent);
    Task AddAndSaveEventAsync(IntegrationEvent evt);
}
