using System;
using System.Collections.Generic;
using System.Linq;
using ReallySimpleEventing.ActivationStrategies;
using ReallySimpleEventing.MessageBus.Azure.AzureBootstrapping;
using ReallySimpleEventing.MessageBus.Azure.ThreadingStrategies;
using ReallySimpleEventing.ThreadingStrategies;

namespace ReallySimpleEventing.MessageBus.Azure
{
    public class AzureMessageBusPlugin : IReallySimpleEventingRegistrationModule
    {
        private readonly RseAzureServiceBusConfiguration _busConfiguration;
        private readonly IEventHandlerResolver _resolver;
        private readonly IAzureTopicCreator _topicCreator;

        public AzureMessageBusPlugin(RseAzureServiceBusConfiguration busConfiguration) 
            : this (busConfiguration, new EventHandlerResolver(), new AzureTopicCreator())
        {
        }

        public AzureMessageBusPlugin(RseAzureServiceBusConfiguration busConfiguration, IEventHandlerResolver resolver, IAzureTopicCreator topicCreator)
        {
            _busConfiguration = busConfiguration;
            _resolver = resolver;
            _topicCreator = topicCreator;
        }

        public void Bootstrap(ReallySimpleEventingConfiguration configuration)
        {
            configuration.ThreadingStrategies.RemoveAll(x => x is NullMessageBusPublishingStrategy);
            configuration.ThreadingStrategies.Insert(0, new AzureMessageBusPublishingStrategy());

            var messageToHandlerMap = CreateMapOfMessagesToSubscriptionHandlers();

            CreateAnyMissingTopics(messageToHandlerMap);
            SubscribeToAllAvailableTopics(messageToHandlerMap);
        }

        private void CreateAnyMissingTopics(IEnumerable<KeyValuePair<Type, Type>> messageToHandlerMap)
        {
            if (_busConfiguration.AutoCreateTopics)
            {
                foreach (var pair in messageToHandlerMap)
                {
                    _topicCreator.CreateTopic("ReallySimpleEventing." + pair.Key.Name);
                }
            }
        }

        private void SubscribeToAllAvailableTopics(IEnumerable<KeyValuePair<Type, Type>> distributedMessageTypes)
        {
        }

        private IDictionary<Type, Type> CreateMapOfMessagesToSubscriptionHandlers()
        {
            var subscriptionHandlers = _resolver.GetAllSubscriptionHandlers().ToList();

            var messageToHandlers = new Dictionary<Type, Type>();
            foreach (var subscriptionHandler in subscriptionHandlers)
            {
                var subscriptionInterface = subscriptionHandler.GetInterfaces().Single(x => x.Name.StartsWith("ISubscribeTo"));
                var messageType = subscriptionInterface.GetGenericArguments()[0];
                messageToHandlers.Add(messageType, subscriptionHandler);
            }

            return messageToHandlers;
        }
    }
}
