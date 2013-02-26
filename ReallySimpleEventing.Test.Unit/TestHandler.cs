using System;
using ReallySimpleEventing.EventHandling;

namespace ReallySimpleEventing.Test.Unit
{
    public abstract class TestHandler<TEventType> : IHandle<TEventType>
    {
        public static int ExecutionCount { get; set; }

        public void Handle(TEventType @event)
        {
            ExecutionCount++;
        }

        public void OnError(TEventType @event, Exception ex)
        {
        }
    }
}