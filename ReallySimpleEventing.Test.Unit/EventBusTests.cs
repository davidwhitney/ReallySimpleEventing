using System;
using System.Diagnostics;
using System.Threading;
using NUnit.Framework;
using ReallySimpleEventing.EventHandling;

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
            var executionCount = 0;
            EventHandledBySingleHandler.Callback = () => executionCount++;

            _eventBus.Raise(new EventHandledBySingle());

            Assert.That(executionCount, Is.EqualTo(1));
        }

        [Test]
        public void Raise_RaiseOneMillionTrivialEvents_HandlerExecutedInLessThanTenSeconds()
        {
            const int numberOfEventsToRaise = 1000000;
            const int maxTimeAllowableInSeconds = 10;
            var executionCount = 0;
            EventHandledBySingleHandler.Callback = () => executionCount++;

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            for (var i = 0; i < numberOfEventsToRaise; i++)
            {
                _eventBus.Raise(new EventHandledBySingle());
            }

            stopWatch.Stop();

            Assert.That(executionCount, Is.EqualTo(numberOfEventsToRaise));
            Assert.That(stopWatch.Elapsed.Seconds, Is.LessThan(maxTimeAllowableInSeconds));
        }

        public class EventHandledByMultiple { }
        public class EventHandledByMultipleHandler1 : TestHandler<EventHandledByMultiple> { }
        public class EventHandledByMultipleHandler2 : TestHandler<EventHandledByMultiple> { }

        [Test]
        public void Raise_MultipleHandlersExist_AllHandlersExecuted()
        {
            var executionCount = 0;
            EventHandledByMultipleHandler1.Callback = () => executionCount++;
            EventHandledByMultipleHandler2.Callback = () => executionCount++;

            _eventBus.Raise(new EventHandledByMultiple());

            Assert.That(executionCount, Is.EqualTo(2));
            Assert.That(executionCount, Is.EqualTo(2));
        }

        public class EventWhereHandlerIsAsync { }
        public class AsycHandlerForEventWhereHandlerIsAsync : IHandleAsync<EventWhereHandlerIsAsync>
        {
            public static Action Callback { get; set; }
            public void Handle(EventWhereHandlerIsAsync @event) { Callback(); }
            public void OnError(EventWhereHandlerIsAsync @event, Exception ex) { }
        }

        [Test]
        public void Raise_AsyncHandlerExists_HandlerExecuted()
        {
            var called = false;
            AsycHandlerForEventWhereHandlerIsAsync.Callback = () => called = true;

            _eventBus.Raise(new EventWhereHandlerIsAsync());

            Thread.Sleep(100); // Hack to let async callback happen
            Assert.That(called, Is.True);
        }

        public class EventWhereHandlerThrowsOnError { }
        public class HandlerThatThrowsOnError : IHandle<EventWhereHandlerThrowsOnError>
        {
            public void Handle(EventWhereHandlerThrowsOnError @event) { throw new Exception("initial exception"); }
            public void OnError(EventWhereHandlerThrowsOnError @event, Exception ex) { throw ex; }
        }

        [Test]
        public void Raise_HandledThrowsOnError_HandlerCascadesOriginalException()
        {
            var ex = Assert.Throws<Exception>(() => _eventBus.Raise(new EventWhereHandlerThrowsOnError()));

            Assert.That(ex.Message, Is.EqualTo("initial exception"));
        }
    }
}
