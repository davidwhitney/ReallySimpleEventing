using ReallySimpleEventing.ActivationStrategies;

namespace ReallySimpleEventing.MessageBus.Azure
{
    public class AzureMessageBusPlugin : MessageBusPlugin<RseAzureServiceBusConfiguration>
    {
        public AzureMessageBusPlugin(RseAzureServiceBusConfiguration busConfiguration, ITopicCreator topicCreator) 
            : base(busConfiguration, topicCreator, new AzurePublisher())
        {
        }

        public AzureMessageBusPlugin(RseAzureServiceBusConfiguration busConfiguration, IEventHandlerResolver resolver, ITopicCreator topicCreator)
            : base(busConfiguration, resolver, topicCreator, new AzurePublisher())
        {
        }
    }
}
