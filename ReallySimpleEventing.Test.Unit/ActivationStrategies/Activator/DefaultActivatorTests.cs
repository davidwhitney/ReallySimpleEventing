using System;
using System.Linq;
using NUnit.Framework;
using ReallySimpleEventing.ActivationStrategies;
using ReallySimpleEventing.EventHandling;

using FluentAssertions;

namespace ReallySimpleEventing.Test.Unit.ActivationStrategies.Activator
{
    [TestFixture]
    public class DefaultActivatorTests
    {
        [Test]
        public void ResolverShouldFindCorrectHandlerTypes_ExactMatch()
        {
            var resolver = new EventHandlerResolver(() => new[] { typeof(NonPolymorphicMessage1Handler) });

            var handlers = resolver.GetHandlerTypesForEvent(typeof(NonPolymorphicMessage1));
            handlers.Count().Should().Be(1);
            handlers.First().Should().Be<NonPolymorphicMessage1Handler>();
        }

        [Test]
        public void ResolverShouldNotFindNonMatchingHandlerTypes()
        {
            var resolver = new EventHandlerResolver(() => new[] { typeof(NonPolymorphicMessage1Handler) });

            var handlers = resolver.GetHandlerTypesForEvent(typeof(NonPolymorphicMessage2));
            handlers.Should().BeEmpty();
        }

        [Test]
        public void ResolverShouldNotFindMoreSpecificHandlerTypes()
        {
            var resolver = new EventHandlerResolver(() => new[] { typeof(ParentPolymorphicMessageHandler), typeof(ChildPolymorphicMessageHandler) });

            var handlers = resolver.GetHandlerTypesForEvent(typeof(ParentPolymorphicMessage));
            handlers.Count().Should().Be(1);
            handlers.Should().Contain(x => x == typeof(ParentPolymorphicMessageHandler));
            handlers.Should().NotContain(x => x == typeof(ChildPolymorphicMessageHandler));
        }

        [Test]
        public void ResolverShouldFindLessSpecificHandlerTypes()
        {
            var resolver = new EventHandlerResolver(() => new[] { typeof(ParentPolymorphicMessageHandler), typeof(ChildPolymorphicMessageHandler) });

            var handlers = resolver.GetHandlerTypesForEvent(typeof(ChildPolymorphicMessage));
            handlers.Count().Should().Be(2);
            handlers.Should().Contain(x => x == typeof(ParentPolymorphicMessageHandler));
            handlers.Should().Contain(x => x == typeof(ChildPolymorphicMessageHandler));
        }

        [Test]
        public void ResolverShouldFindHandlersThatHandleMultipleDisparateTypesOfMessage()
        {
            var resolver = new EventHandlerResolver(() => new[] { typeof(MultiMessageHandler)});

            var handlersA = resolver.GetHandlerTypesForEvent(typeof(MultiMessageA));
            handlersA.Count().Should().Be(1);
            handlersA.Should().Contain(x => x == typeof(MultiMessageHandler));

            var handlersB = resolver.GetHandlerTypesForEvent(typeof(MultiMessageB));
            handlersB.Count().Should().Be(1);
            handlersB.Should().Contain(x => x == typeof(MultiMessageHandler));
        }

        /********************************/
        public class NonPolymorphicMessage1{ }
        public class NonPolymorphicMessage2{ }

        public class NonPolymorphicMessage1Handler : IHandle<NonPolymorphicMessage1>
        {
            public void Handle(NonPolymorphicMessage1 @event){}
            public void OnError(NonPolymorphicMessage1 @event, Exception ex){}
        }

        public class MultiMessageA{}
        public class MultiMessageB{}

        public class MultiMessageHandler:IHandle<MultiMessageA>, IHandle<MultiMessageB>
        {
            void IHandle<MultiMessageA>.Handle(MultiMessageA @event){}
            void IHandle<MultiMessageA>.OnError(MultiMessageA @event, Exception ex){}
            void IHandle<MultiMessageB>.Handle(MultiMessageB @event){}
            void IHandle<MultiMessageB>.OnError(MultiMessageB @event, Exception ex){}
        }

        public class ParentPolymorphicMessage{}
        public class ChildPolymorphicMessage : ParentPolymorphicMessage { }

        public class ParentPolymorphicMessageHandler : IHandle<ParentPolymorphicMessage>
        {
            public void Handle(ParentPolymorphicMessage @event) { }
            public void OnError(ParentPolymorphicMessage @event, Exception ex) { }
        }

        public class ChildPolymorphicMessageHandler : IHandle<ChildPolymorphicMessage>
        {
            public void Handle(ChildPolymorphicMessage @event) { }
            public void OnError(ChildPolymorphicMessage @event, Exception ex) { }
        }
    }
}