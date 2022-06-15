using Inventory.App.IntegrationEvents;
using Inventory.App.IntegrationEvents.Events;
using Microsoft.Extensions.Logging;
using Oths.EventBus.Abstractions;
using Oths.EventBus.Events;

namespace Inventory.AppIntegrationEvents;

public class InventoryIntegrationEventService : IInventoryIntegrationEventService
{
    private readonly IEventBus _eventBus;
    private readonly ILogger<InventoryIntegrationEventService> _logger;

    public InventoryIntegrationEventService(IEventBus eventBus,

        ILogger<InventoryIntegrationEventService> logger)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task AddAndSaveEventAsync(IntegrationEvent evt)
    {
        throw new NotImplementedException();
    }
      
    public async Task PublishEventsThroughEventBusAsync(ItemCreatedIntegrationEvent integrationEvent)
    {
        try
        {
            _eventBus.Publish(integrationEvent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ERROR publishing integration event: {IntegrationEventId}", integrationEvent.ItemId);

        }
    }    
    
    public async Task PublishEventsThroughEventBusAsync(ItemRemovedIntegrationEvent integrationEvent)
    {
        try
        {
            _eventBus.Publish(integrationEvent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ERROR publishing integration event: {IntegrationEventId}", integrationEvent.ItemId);

        }
    }

    public Task PublishEventsThroughEventBusAsync(Guid transactionId)
    {
        throw new NotImplementedException();
    }


}

 

