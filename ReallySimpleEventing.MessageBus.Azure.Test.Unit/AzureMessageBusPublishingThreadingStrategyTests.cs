using System;
using NUnit.Framework;
using ReallySimpleEventing.EventHandling;

namespace ReallySimpleEventing.MessageBus.Azure.Test.Unit
{
    [TestFixture]
    public class AzureMessageBusPublishingThreadingStrategyTests
    {
        [Test]
        public void Supports_SynchronousHandler_ReturnsFalse()
        {
            var strategy = new MessageBusPublishingThreadingStrategy();

            var supported = strategy.Supports(new SyncHandler());

            Assert.That(supported, Is.Not.True);
        }

        [Test]
        public void Supports_AsynchronousHandler_ReturnsFalse()
        {
            var strategy = new MessageBusPublishingThreadingStrategy();

            var supported = strategy.Supports(new AsyncHandler());

            Assert.That(supported, Is.Not.True);
        }

        [Test]
        public void Supports_SubscriptionHandler_ReturnsTrue()
        {
            var strategy = new MessageBusPublishingThreadingStrategy();

            var supported = strategy.Supports(new SubscriptionHandler());

            Assert.That(supported, Is.True);
        }

        public class SyncHandler : IHandle<SomeObject>
        {
            public void Handle(SomeObject @event) { throw new NotImplementedException(); }
            public void OnError(SomeObject @event, Exception ex) { throw new NotImplementedException(); }
        }

        public class AsyncHandler : IHandleAsync<SomeObject>
        {
            public void Handle(SomeObject @event) { throw new NotImplementedException(); }
            public void OnError(SomeObject @event, Exception ex) { throw new NotImplementedException(); }
        }

        public class SubscriptionHandler : ISubscribeTo<SomeObject>
        {
            public void Handle(SomeObject @event) { throw new NotImplementedException(); }
            public void OnError(SomeObject @event, Exception ex) { throw new NotImplementedException(); }
        }

        public class SomeObject { }
    }
}
