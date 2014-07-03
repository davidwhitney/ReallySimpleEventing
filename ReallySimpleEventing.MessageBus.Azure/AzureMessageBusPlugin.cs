using ReallySimpleEventing.ThreadingStrategies;

namespace ReallySimpleEventing.MessageBus.Azure
{
    public class AzureMessageBusPlugin : IReallySimpleEventingRegistrationModule
    {
        public void Bootstrap(ReallySimpleEventingConfiguration configuration)
        {
            configuration.ThreadingStrategies.RemoveAll(x => x is NullMessageBusPublishingStrategy);
            configuration.ThreadingStrategies.Insert(0, new AzureMessageBusPublishingStrategy());

            // Detect message bus messages

            // subscribe to topics
        }
    }
}
