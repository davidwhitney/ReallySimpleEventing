using System.Linq;
using NUnit.Framework;
using ReallySimpleEventing.EventHandling;

namespace ReallySimpleEventing.Test.Unit
{
    [TestFixture]
    public class ReallySimpleEventingTests
    {
        [Test]
        public void AA()
        {
            var eventBus = ReallySimpleEventing.CreateRealTimeEventBus();
            
            eventBus.Raise(new MyEvent());
        }
    }

    public class MyEvent
    {
        public string EventData { get { return "hi"; } }
    }

    public class DoSomethingWhenMyEventHappens : IHandle<MyEvent>
    {
        public void Handle(MyEvent @event)
        {
            
        }
    }
}
