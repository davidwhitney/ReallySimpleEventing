using System;
using ReallySimpleEventing.EventHandling;

namespace ReallySimpleEventing.Test.Unit
{
    public abstract class TestHandler<TEventType> : IHandle<TEventType>
    {
        public static Action OnHandleAction { get; set; }
        public static Action<Exception> OnErrorAction { get; set; }
        
        public void Handle(TEventType @event)
        {
            if (OnHandleAction != null) OnHandleAction();
        }
        
        public void OnError(TEventType @event, Exception ex)
        {
            if (OnErrorAction != null) OnErrorAction(ex);
        }
    }

    public abstract class TestHandlerAsync<TEventType> : TestHandler<TEventType>, IHandleAsync<TEventType>
    {
    }
}