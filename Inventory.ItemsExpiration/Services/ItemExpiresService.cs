using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Inventory.ItemsExpiration.Events;
using Oths.EventBus.Abstractions;

namespace Inventory.ItemsExpiration.Services
{
    public class ItemExpiresService : BackgroundService
    {
        private readonly ILogger<ItemExpiresService> _logger;
        private readonly ItemsExpirationSettings _settings;
        private readonly IEventBus _eventBus;

        public ItemExpiresService(IOptions<ItemsExpirationSettings> settings, IEventBus eventBus, ILogger<ItemExpiresService> logger)
        {
            _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("Item Expiration Service is starting.");

            stoppingToken.Register(() => _logger.LogDebug("#1 ItemSpiresService background task is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogDebug("ItemSpiresService background task is doing background work.");

                CheckItemExpireds();

                await Task.Delay(_settings.CheckUpdateTime, stoppingToken);
            }

            _logger.LogDebug("ItemSpiresService background task is stopping.");
        }

        private void CheckItemExpireds()
        {
            _logger.LogDebug("Checking item expireds");

            var itemsIds = GetExpiredItems();

            foreach (var itemId in itemsIds)
            {
                var itemExpiredEvent = new ItemExpiredIntegrationEvent(itemId);

                _logger.LogInformation("----- Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", itemExpiredEvent.Id, Program.AppName, itemExpiredEvent);

                _eventBus.Publish(itemExpiredEvent);
            }
        }

        private IEnumerable<int> GetExpiredItems()
        {
            var itemsIds = new List<int>();

            //mock expired items:
            itemsIds.Add(99);

            //in real application we can do :

            //using (var conn = new SqlConnection(_settings.ConnectionString))
            //{
            //    try
            //    {
            //       "sql query in items database"
            //    }
            //    catch (SqlException exception)
            //    {
            //        _logger.LogCritical(exception, "FATAL ERROR: Database connections could not be opened: {Message}", exception.Message);
            //    }

            //}

            return itemsIds;
        }
    }
}


