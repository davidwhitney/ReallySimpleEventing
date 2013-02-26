using System.Diagnostics;
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

        [Test]
        public void Raise_RaiseOneMillionTrivialEvents_HandlerExecutedInLessThanTenSeconds()
        {
            const int numberOfEventsToRaise = 1000000;
            const int maxTimeAllowableInSeconds = 10;
            EventHandledBySingleHandler.ExecutionCount = 0;

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            for (var i = 0; i < numberOfEventsToRaise; i++)
            {
                _eventBus.Raise(new EventHandledBySingle());
            }

            stopWatch.Stop();

            Assert.That(EventHandledBySingleHandler.ExecutionCount, Is.EqualTo(numberOfEventsToRaise));
            Assert.That(stopWatch.Elapsed.Seconds, Is.LessThan(maxTimeAllowableInSeconds));
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
