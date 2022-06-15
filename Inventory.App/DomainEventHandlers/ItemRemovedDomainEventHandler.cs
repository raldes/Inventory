using Inventory.App.IntegrationEvents;
using Inventory.App.IntegrationEvents.Events;
using Inventory.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Inventory.App.DomainEventHandlers
{
    public class ItemRemovedDomainEventHandler : INotificationHandler<ItemRemovedDomainEvent>
    {
        private readonly ILoggerFactory _logger;
        private readonly IInventoryIntegrationEventService _integrationEventService;

        public ItemRemovedDomainEventHandler(
            ILoggerFactory logger,
            IInventoryIntegrationEventService integrationEventService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _integrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));
        }

        public async Task Handle(ItemRemovedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            _logger.CreateLogger<ItemRemovedDomainEventHandler>()
                .LogTrace("Item with Id: {Id} has been successfully removed",domainEvent.Item.ItemId);

            //here we could send an integration event to other services:

            var integrationEvent = new ItemRemovedIntegrationEvent(domainEvent.Item.ItemId);

            await _integrationEventService.PublishEventsThroughEventBusAsync(integrationEvent);
        }
    }

}
