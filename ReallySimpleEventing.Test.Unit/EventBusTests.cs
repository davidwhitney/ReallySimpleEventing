using NUnit.Framework;

namespace ReallySimpleEventing.Test.Unit
{
    [TestFixture]
    public class EventBusTests
    {
        private IEventBus _eventBus;

        [SetUp]
        public void SetUp()
        {
            _eventBus = ReallySimpleEventing.CreateEventBus();
        }

        public class EventHandledBySingle {}
        public class EventHandledBySingleHandler : TestHandler<EventHandledBySingle> { }

        [Test]
        public void Raise_HandlerExist_HandlerExecuted()
        {
            _eventBus.Raise(new EventHandledBySingle());

            Assert.That(EventHandledBySingleHandler.ExecutionCount, Is.EqualTo(1));
        }

        public class EventHandledByMultiple { }
        public class EventHandledByMultipleHandler1 : TestHandler<EventHandledByMultiple> { }
        public class EventHandledByMultipleHandler2 : TestHandler<EventHandledByMultiple> { }

        [Test]
        public void Raise_MultipleHandlersExist_AllHandlersExecuted()
        {
            _eventBus.Raise(new EventHandledByMultiple());

            Assert.That(EventHandledByMultipleHandler1.ExecutionCount, Is.EqualTo(2));
            Assert.That(EventHandledByMultipleHandler2.ExecutionCount, Is.EqualTo(2));
        }
    }
}
