using Inventory.App.IntegrationEvents;
using Inventory.App.IntegrationEvents.Events;
using Inventory.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Inventory.App.DomainEventHandlers
{
    public class ItemUpdatedDomainEventHandler : INotificationHandler<ItemUpdatedDomainEvent>
    {
        private readonly ILoggerFactory _logger;
        private readonly IInventoryIntegrationEventService _integrationEventService;

        public ItemUpdatedDomainEventHandler(
            ILoggerFactory logger,
            IInventoryIntegrationEventService integrationEventService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _integrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));
        }

        public async Task Handle(ItemUpdatedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            _logger.CreateLogger<ItemUpdatedDomainEventHandler>()
                .LogTrace("Item with Id: {Id} has been successfully updated",domainEvent.Item.ItemId);

            //here we could send an integration event to other services:
            //_integrationEventService.PublishEventsThroughEventBusAsync(...);
        }
    }

}
