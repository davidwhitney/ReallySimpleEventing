using System;

namespace ReallySimpleEventing.EventHandling
{
    public interface IHandle<in TEventType>
    {
        void Handle(TEventType @event);
        void OnError(TEventType @event, Exception ex);
    }
}