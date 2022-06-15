using Inventory.App.IntegrationEvents;
using Inventory.App.IntegrationEvents.Events;
using Inventory.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Inventory.App.DomainEventHandlers
{
    public class ItemExpiredDomainEventHandler : INotificationHandler<ItemExpiredDomainEvent>
    {
        private readonly ILoggerFactory _logger;
        private readonly IInventoryIntegrationEventService _integrationEventService;

        public ItemExpiredDomainEventHandler(
            ILoggerFactory logger,
            IInventoryIntegrationEventService integrationEventService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _integrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(integrationEventService));
        }

        public async Task Handle(ItemExpiredDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            _logger.CreateLogger<ItemRemovedDomainEventHandler>()
                .LogTrace("Item with Id: {Id} is expired",domainEvent.Item.ItemId);

            domainEvent.Item.SetStatusCreated();

            //here we could send an integration event to other services:
        }
    }

}
