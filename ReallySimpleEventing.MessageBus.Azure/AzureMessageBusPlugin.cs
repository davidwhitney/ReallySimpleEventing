using System;
using ReallySimpleEventing.EventHandling;
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

    public class AzureMessageBusPublishingStrategy : IHandlerThreadingStrategy
    {
        public bool Supports<TEventType>(IHandle<TEventType> handler)
        {
            return handler as ISubscribeTo<TEventType> != null;
        }

        public void Run(Action operation)
        {
            throw new NotImplementedException("You published a message with an ISubscribeTo handler present - this type is designed to handle messages over message buses but you haven't configured one.");
        }
    }
}
