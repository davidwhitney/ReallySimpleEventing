using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using ReallySimpleEventing.ActivationStrategies.Delegated;
using ReallySimpleEventing.EventHandling;

namespace ReallySimpleEventing.Test.Unit.ActivationStrategies.Delegated
{
    [TestFixture]
    public class DelegatedActivatorTests
    {
        [Test]
        public void DelegatedActivationShouldCallTheDelegateWithTheCorrectType()
        {
            Func<Type, IEnumerable<object>> handler = t =>
            {
                t.Should().Be<SomeMessage>();
                return Enumerable.Empty<object>();
            };

            var activator = new DelegatedActivationWithoutDiscovery(handler);
            activator.GetHandlers<SomeMessage>();
        }

        [Test]
        public void DelegatedActivationShouldReturnTheInstantiatedType()
        {
            Func<Type, IEnumerable<object>> handler = t => new []{new SomeMessageHandler()};

            var activator = new DelegatedActivationWithoutDiscovery(handler);
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

    }
}
