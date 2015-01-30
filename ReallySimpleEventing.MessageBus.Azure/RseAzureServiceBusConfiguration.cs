using System;

namespace ReallySimpleEventing.MessageBus.Azure
{
    public class RseAzureServiceBusConfiguration : IMessageBusConfiguration
    {
        public bool AutoCreateTopics { get; set; }
        public TimeSpan DefaultMessageTimeToLive { get; set; }
        public string SubscriptionNamespace { get; set; }

        public RseAzureServiceBusConfiguration(string subscriptionNamespace)
        {
            SubscriptionNamespace = subscriptionNamespace;
            AutoCreateTopics = true;
            DefaultMessageTimeToLive = new TimeSpan(1, 0, 0);
        }
    }
}