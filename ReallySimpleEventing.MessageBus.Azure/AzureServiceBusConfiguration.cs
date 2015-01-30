using System;

namespace ReallySimpleEventing.MessageBus.Azure
{
    public class AzureServiceBusConfiguration : IMessageBusConfiguration
    {
        public bool AutoCreateTopics { get; set; }
        public TimeSpan DefaultMessageTimeToLive { get; set; }
        public string SubscriptionNamespace { get; set; }

        public AzureServiceBusConfiguration(string subscriptionNamespace)
        {
            SubscriptionNamespace = subscriptionNamespace;
            AutoCreateTopics = true;
            DefaultMessageTimeToLive = new TimeSpan(1, 0, 0);
        }
    }
}