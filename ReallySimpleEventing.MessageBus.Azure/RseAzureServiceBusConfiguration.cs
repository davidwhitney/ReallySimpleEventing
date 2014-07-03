namespace ReallySimpleEventing.MessageBus.Azure
{
    public class RseAzureServiceBusConfiguration
    {
        public bool AutoCreateTopics { get; set; }

        public RseAzureServiceBusConfiguration()
        {
            AutoCreateTopics = true;
        }
    }
}