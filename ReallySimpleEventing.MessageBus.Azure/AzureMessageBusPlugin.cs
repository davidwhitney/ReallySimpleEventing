using ReallySimpleEventing.ActivationStrategies;

namespace ReallySimpleEventing.MessageBus.Azure
{
    public class AzureMessageBusPlugin : MessageBusPlugin<AzureServiceBusConfiguration>
    {
        public AzureMessageBusPlugin(AzureServiceBusConfiguration busConfiguration, ITopicCreator topicCreator) 
            : base(busConfiguration, topicCreator, new AzurePublisher())
        {
        }

        public AzureMessageBusPlugin(AzureServiceBusConfiguration busConfiguration, IEventHandlerResolver resolver, ITopicCreator topicCreator)
            : base(busConfiguration, resolver, topicCreator, new AzurePublisher())
        {
        }
    }
}
