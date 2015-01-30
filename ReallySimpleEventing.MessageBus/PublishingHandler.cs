using System;
using ReallySimpleEventing.EventHandling;

namespace ReallySimpleEventing.MessageBus
{
    public class PublishingHandler<TEventType> : ISubscribeTo<TEventType>
    {
        private readonly IPublishToMessageBuses _publishing;

        public PublishingHandler(IPublishToMessageBuses publishing)
        {
            _publishing = publishing;
        }

        public void Handle(TEventType @event)
        {
            _publishing.Publish(@event);
        }

        public void OnError(TEventType @event, Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}