using System;
using ReallySimpleEventing.EventHandling;
using ReallySimpleEventing.ThreadingStrategies;

namespace ReallySimpleEventing.MessageBus.Azure
{
    public class AzureMessageBusPublishingStrategy : IHandlerThreadingStrategy
    {
        public bool Supports<TEventType>(IHandle<TEventType> handler)
        {
            return handler as ISubscribeTo<TEventType> != null;
        }

        public void Run(Action operation)
        {
            throw new NotImplementedException();
        }
    }
}