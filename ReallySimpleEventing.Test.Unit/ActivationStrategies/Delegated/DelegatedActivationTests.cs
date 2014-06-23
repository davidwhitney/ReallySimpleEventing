using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using ReallySimpleEventing.ActivationStrategies;
using ReallySimpleEventing.ActivationStrategies.Delegated;
using ReallySimpleEventing.EventHandling;

namespace ReallySimpleEventing.Test.Unit.ActivationStrategies.Delegated
{
    [TestFixture]
    public class DelegatedActivationTests
    {
        [Test]
        public void DelegatedActivationShouldCallTheDelegateWithTheCorrectType()
        {
            var resolver = new EventHandlerResolver(() => new[] { typeof(SomeMessageHandler) });

            Func<Type, IEnumerable<object>> handler = t =>
            {
                t.Should().Be<SomeMessage>();
                return Enumerable.Empty<object>();
            };

            var activator = new DelegatedActivationWithDiscovery(resolver, handler);
            activator.GetHandlers<SomeMessage>();
        }

        [Test]
        public void DelegatedActivationShouldReturnTheInstantiatedType()
        {
            var resolver = new EventHandlerResolver(() => new[] { typeof(SomeMessageHandler) }); 
            Func<Type, object> handler = t => new SomeMessageHandler();
            var activator = new DelegatedActivationWithDiscovery(resolver, handler);

            var handlers = activator.GetHandlers<SomeMessage>();

            handlers.Count().Should().Be(1);
            handlers.First().Should().BeOfType<SomeMessageHandler>();
        }

        private class SomeMessage{}
        private class SomeMessageHandler : IHandle<SomeMessage>
        {
            public void Handle(SomeMessage @event){}
            public void OnError(SomeMessage @event, Exception ex){}
        }

        [Test]
        public void WhenHandlerIsAsyncDelegatedActivationShouldCallTheAsyncDelegateWithTheCorrectType()
        {
            var resolver = new EventHandlerResolver(() => new[] { typeof(SomeAsyncMessageHandler) });
            var asyncCalled = false;
            Func<Type, IHandle<SomeAsyncMessage>> handler = t => new SomeAsyncMessageHandler();
            Func<Type, IHandle<SomeAsyncMessage>> handlerAsync = t =>
            {
                asyncCalled = true;
                return new SomeAsyncMessageHandler();
            };

            var activator = new DelegatedActivationWithDiscovery(resolver, handler, handlerAsync);
            activator.GetHandlers<SomeAsyncMessage>().ToList();

            Assert.That(asyncCalled, Is.True);
        }
        
        private class SomeAsyncMessage{}
        private class SomeAsyncMessageHandler : IHandleAsync<SomeAsyncMessage>
        {
            public void Handle(SomeAsyncMessage @event) { }
            public void OnError(SomeAsyncMessage @event, Exception ex) { }
        }

        [Test]
        public void WhenHandlerIsSubscription_ReturnsAsyncHandlerForPublishingMessages()
        {
            var resolver = new EventHandlerResolver(() => new[] { typeof(SomePubSubMessageHandler) });
            var asyncCalled = false;
            Func<Type, IHandle<SomePubSubMessage>> handler = t => new SomePubSubMessageHandler();
            Func<Type, IHandle<SomePubSubMessage>> handlerAsync = t =>
            {
                asyncCalled = true;
                return new SomePubSubMessageHandler();
            };

            var activator = new DelegatedActivationWithDiscovery(resolver, handler, handlerAsync);
            activator.GetHandlers<SomePubSubMessage>().ToList();

            Assert.That(asyncCalled, Is.True);
        }
        
        private class SomePubSubMessage{}
        private class SomePubSubMessageHandler : ISubscribeTo<SomePubSubMessage>
        {
            public void Handle(SomePubSubMessage @event) { }
            public void OnError(SomePubSubMessage @event, Exception ex) { }
        }
    }
}
