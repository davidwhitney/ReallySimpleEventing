using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using ReallySimpleEventing.ActivationStrategies;
using ReallySimpleEventing.ActivationStrategies.Composed;
using ReallySimpleEventing.EventHandling;

namespace ReallySimpleEventing.Test.Unit.ActivationStrategies.Composed
{
    [TestFixture]
    public class FirstViableActivatorActivatorTests
    {
        [Test]
        public void WhenNoStrategIsProvided_ResolverShouldResolveEmptyResult()
        {
            var resolver = new FirstViableActivatorActivation(null);
            var result = resolver.GetHandlers<SomeMessage>();

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Test]
        public void WhenEmptyStrategyCollectionIsProvided_ResolverShouldResolveEmptyResult()
        {
            var resolver = new FirstViableActivatorActivation(Enumerable.Empty<IHandlerActivationStrategy>());
            var result = resolver.GetHandlers<SomeMessage>();

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Test]
        public void WhenNoStrategyCanResolveHandlers_ResolverShouldResolveEmptyResult()
        {
            var resolver = new FirstViableActivatorActivation(new[] { new HandlerActivationStrategyA() });
            var result = resolver.GetHandlers<SomeMessage>();

            result.Should().BeEmpty();
        }

        [Test]
        public void WhenTheOnlyStrategyCanResolveHandlers_ResolverShouldReturnThoseHandlers()
        {
            var resolver = new FirstViableActivatorActivation(new[] { new HandlerActivationStrategyA { ShouldFindItems = true } });
            var result = resolver.GetHandlers<SomeMessage>();

            result.Count().Should().Be(1);
            result.First().GetType().Should().Be<SomeMessageHandlerA<SomeMessage>>();
        }

        [Test]
        public void WhenTheFirstStrategyHasAMatchingHandlerButSecondStrategyCannotMatchHandlersHandlers_ResolverShouldReturnFirstStrategiesHandlers()
        {
            var resolver = new FirstViableActivatorActivation(new IHandlerActivationStrategy[] { new HandlerActivationStrategyA { ShouldFindItems = true }, new HandlerActivationStrategyB { ShouldFindItems = false } });
            var result = resolver.GetHandlers<SomeMessage>();

            result.Count().Should().Be(1);
            result.First().GetType().Should().Be<SomeMessageHandlerA<SomeMessage>>();
        }

        [Test]
        public void WhenTheFirstStrategyCannotMatchHandlersHandlersButSecondStrategyHasAMatchingHandler_ResolverShouldReturnSecondStrategiesHandlers()
        {
            var resolver = new FirstViableActivatorActivation(new IHandlerActivationStrategy[] { new HandlerActivationStrategyA { ShouldFindItems = false }, new HandlerActivationStrategyB { ShouldFindItems = true } });
            var result = resolver.GetHandlers<SomeMessage>();

            result.Count().Should().Be(1);
            result.First().GetType().Should().Be<SomeMessageHandlerB<SomeMessage>>();
        }

        [Test]
        public void WhenTheFirstStrategyHasAMatchingHandlerAndTheSecondStrategyHasAMatchingHandler_ResolverShouldReturnFirstStrategiesHandlers()
        {
            var resolver = new FirstViableActivatorActivation(new IHandlerActivationStrategy[] { new HandlerActivationStrategyA { ShouldFindItems = true }, new HandlerActivationStrategyB { ShouldFindItems = true } });
            var result = resolver.GetHandlers<SomeMessage>();

            result.Count().Should().Be(1);
            result.First().GetType().Should().Be<SomeMessageHandlerA<SomeMessage>>();
        }


        private class SomeMessage { }
        private class SomeMessageHandlerA<T> : IHandle<T>
        {
            public void Handle(T @event) { }
            public void OnError(T @event, Exception ex) { }
        }
        private class HandlerActivationStrategyA : IHandlerActivationStrategy
        {
            public bool ShouldFindItems { get; set; }

            public IEnumerable<IHandle<TEventType>> GetHandlers<TEventType>()
            {
                return ShouldFindItems ? new[] { new SomeMessageHandlerA<TEventType>() } : Enumerable.Empty<IHandle<TEventType>>();
            }
        }

        private class HandlerActivationStrategyB : IHandlerActivationStrategy
        {
            public bool ShouldFindItems { get; set; }

            public IEnumerable<IHandle<TEventType>> GetHandlers<TEventType>()
            {
                return ShouldFindItems ? new[] { new SomeMessageHandlerB<TEventType>() } : Enumerable.Empty<IHandle<TEventType>>();
            }
        }
        private class SomeMessageHandlerB<T> : IHandle<T>
        {
            public void Handle(T @event) { }
            public void OnError(T @event, Exception ex) { }
        }
    }
}