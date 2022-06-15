
using Inventory.App.IntegrationEvents.Events;
using Microsoft.Extensions.Logging;
using Oths.EventBus.Abstractions;
using Serilog.Context;

namespace Inventory.App.IntegrationEvents.EventHandling
{
    public class ItemExpiredIntegrationEventHandler : IIntegrationEventHandler<ItemExpiredIntegrationEvent>
    {
        private readonly ILogger<ItemExpiredIntegrationEventHandler> _logger;

        public ItemExpiredIntegrationEventHandler(
            ILogger<ItemExpiredIntegrationEventHandler> logger
            )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(ItemExpiredIntegrationEvent integrationEvent)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{integrationEvent.Id}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", integrationEvent.Id, "Program.AppName", integrationEvent);

            }
        }
    }
}
