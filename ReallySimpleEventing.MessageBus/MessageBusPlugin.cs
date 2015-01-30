using System;
using System.Collections.Generic;
using System.Linq;
using ReallySimpleEventing.ActivationStrategies;
using ReallySimpleEventing.EventHandling;
using ReallySimpleEventing.ThreadingStrategies;

namespace ReallySimpleEventing.MessageBus
{
    public abstract class MessageBusPlugin<TConfigurationType> : IReallySimpleEventingRegistrationModule where TConfigurationType : IMessageBusConfiguration
    {
        private readonly TConfigurationType _busConfiguration;
        private readonly IEventHandlerResolver _resolver;
        private readonly ITopicCreator _topicCreator;
        private readonly IPublishToMessageBuses _busPublisher;

        protected MessageBusPlugin(TConfigurationType busConfiguration, ITopicCreator topicCreator, IPublishToMessageBuses busPublisher)
            : this(busConfiguration, new EventHandlerResolver(), topicCreator, busPublisher)
        {
        }

        protected MessageBusPlugin(TConfigurationType busConfiguration, IEventHandlerResolver resolver, ITopicCreator topicCreator, IPublishToMessageBuses busPublisher)
        {
            _busConfiguration = busConfiguration;
            _resolver = resolver;
            _topicCreator = topicCreator;
            _busPublisher = busPublisher;
        }
        
        public void Bootstrap(ReallySimpleEventingConfiguration configuration)
        {
            configuration.ThreadingStrategies.RemoveAll(x => x is NullMessageBusPublishingStrategy);
            configuration.ThreadingStrategies.Insert(0, new MessageBusPublishingThreadingStrategy());
            configuration.ActivationStrategy = new InterceptedHandlerActivationStrategy(configuration.ActivationStrategy, _busPublisher);

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