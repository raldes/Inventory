using Inventory.App.IntegrationEvents;
using Inventory.App.IntegrationEvents.Events;
using Inventory.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Inventory.App.DomainEventHandlers
{
    public class ItemCreatedDomainEventHandler : INotificationHandler<ItemCreatedDomainEvent>
    {
        private readonly ILoggerFactory _logger;
        private readonly IInventoryIntegrationEventService _integrationEventService;

        public ItemCreatedDomainEventHandler(
            ILoggerFactory logger,
            IInventoryIntegrationEventService integrationEventService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _integrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));
        }

        public async Task Handle(ItemCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            _logger.CreateLogger<ItemCreatedDomainEventHandler>()
                .LogTrace("Item with Id: {Id} has been successfully created",domainEvent.ItemId);

            //here we could send an integration event to other services:

            var integrationEvent = new ItemCreatedIntegrationEvent(domainEvent.ItemId);

            await _integrationEventService.PublishEventsThroughEventBusAsync(integrationEvent);
        }
    }

}
