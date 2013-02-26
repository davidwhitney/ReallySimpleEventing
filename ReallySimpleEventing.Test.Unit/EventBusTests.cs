using System;
using NUnit.Framework;
using ReallySimpleEventing.EventHandling;

namespace ReallySimpleEventing.Test.Unit
{
    [TestFixture]
    public class EventBusTests
    {
        [Test]
        public void Raise_HandlerExist_HandlerExecuted()
        {
            var eventBus = ReallySimpleEventing.CreateEventBus();
            
            eventBus.Raise(new MyEvent());

            Assert.That(MyEventHandler.ExecutionCount, Is.EqualTo(1));
        }
    }

    public class MyEvent
    {
        public string EventData { get { return "hi"; } }
    }

    public class MyEventHandler : IHandle<MyEvent>
    {
        public static int ExecutionCount { get; private set; }

        public void Handle(MyEvent @event)
        {
            ExecutionCount++;
        }

        public void OnError(MyEvent @event, Exception ex)
        {
        }
    }
}
