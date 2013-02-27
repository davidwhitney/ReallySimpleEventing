using System;
using ReallySimpleEventing.EventHandling;

namespace ReallySimpleEventing.Test.Unit
{
    public abstract class TestHandler<TEventType> : IHandle<TEventType>
    {
        public static Action Callback { get; set; }
        public void Handle(TEventType @event)
        {
            Callback();
        }

        public void OnError(TEventType @event, Exception ex)
        {
        }
    }
}