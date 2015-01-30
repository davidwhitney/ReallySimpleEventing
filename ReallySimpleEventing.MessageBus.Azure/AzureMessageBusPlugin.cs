using System;
using System.Collections.Generic;
using System.Linq;
using ReallySimpleEventing.ActivationStrategies;
using ReallySimpleEventing.EventHandling;
using ReallySimpleEventing.MessageBus.Azure.ActivationStrategies;
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
            : this (busConfiguration, new EventHandlerResolver(), new AzureTopicCreator(busConfiguration))
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
            configuration.ThreadingStrategies.Insert(0, new AzureMessageBusPublishingThreadingStrategy());
            configuration.ActivationStrategy = new InterceptedHandlerActivationStrategy(configuration.ActivationStrategy);

            var messageToHandlerMap = CreateMapOfMessagesToSubscriptionHandlers();

            CreateAnyMissingTopics(messageToHandlerMap);
            SubscribeToAllAvailableTopics(messageToHandlerMap);
        }

        private void CreateAnyMissingTopics(IEnumerable<Type> messagesWithSubscriptions)
        {
            if (!_busConfiguration.AutoCreateTopics)
            {
                return;
            }

            foreach (var messageType in messagesWithSubscriptions)
            {
                _topicCreator.CreateTopic(messageType.Name);
            }
        }

        private void SubscribeToAllAvailableTopics(IList<Type> distributedMessageTypes)
        {
        }

        private IList<Type> CreateMapOfMessagesToSubscriptionHandlers()
        {
            var subscriptionHandlers = _resolver.GetAllSubscriptionHandlers().ToList();

            var messageToHandlers = new List<Type>();
            foreach (var subscriptionHandler in subscriptionHandlers)
            {
                var subscriptionInterface =
                    subscriptionHandler.GetInterfaces()
                        .Single(x => x.GetGenericTypeDefinition() == typeof (ISubscribeTo<>));

                var messageType = subscriptionInterface.GetGenericArguments()[0];
                messageToHandlers.Add(messageType);
            }

            return messageToHandlers;
        }
    }
}
