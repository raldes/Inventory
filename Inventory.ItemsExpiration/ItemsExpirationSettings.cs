namespace Inventory.ItemsExpiration
{
    public class ItemsExpirationSettings
    {
        public string ConnectionString { get; set; }

        public string EventBusConnection { get; set; }

        public int ExpirationPeriodTime { get; set; }

        public int CheckUpdateTime { get; set; }

        public string SubscriptionClientName { get; set; }
    }
}

