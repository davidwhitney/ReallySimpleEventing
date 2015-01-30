using System;
using ReallySimpleEventing.EventHandling;

namespace ReallySimpleEventing.MessageBus.Azure.EventHandling
{
    public class AzureMessageBusPublishingHandler<TEventType> : ISubscribeTo<TEventType>
    {
        public void Handle(TEventType @event)
        {
            throw new NotImplementedException();
        }

        public void OnError(TEventType @event, Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}