using System;
using System.Linq;
using NUnit.Framework;
using ReallySimpleEventing.ActivationStrategies.Activator;
using ReallySimpleEventing.EventHandling;

namespace ReallySimpleEventing.MessageBus.Test.Unit
{
    [TestFixture]
    public class InterceptedHandlerActivationStrategyTests
    {
        [Test]
        public void GetHandlers_WhenASubscriptionIsPresent_TwoHandlersReturned()
        {
            var interceptedStrat = new InterceptedHandlerActivationStrategy(new ActivatorActivation(), new FakePublisher());

            var handlers = interceptedStrat.GetHandlers<MulticastMessage>();

            Assert.That(handlers.Count(), Is.EqualTo(2));
        }

        [Test]
        public void GetHandlers_WhenASubscriptionIsPresent_MutlicastingHandlerReturned()
        {
            var interceptedStrat = new InterceptedHandlerActivationStrategy(new ActivatorActivation(), new FakePublisher());

            var handlers = interceptedStrat.GetHandlers<MulticastMessage>();

            Assert.That(handlers.Count(x => x.GetType() == typeof(PublishingHandler<MulticastMessage>)), Is.EqualTo(1));
        }

        public class MulticastMessage { }
        public class TestSubscription : ISubscribeTo<MulticastMessage>
        {
            public void Handle(MulticastMessage @event) { }
            public void OnError(MulticastMessage @event, Exception ex) { }
        }

        [Test]
        public void GetHandlers_MultipleSubscriptionsPresent_OnlyOneAzureBusPublisherAdded()
        {
            var interceptedStrat = new InterceptedHandlerActivationStrategy(new ActivatorActivation(), new FakePublisher());

            var handlers = interceptedStrat.GetHandlers<MulticastMessageWithManySubscriptions>();

            Assert.That(handlers.Count(x => x.GetType() == typeof(PublishingHandler<MulticastMessageWithManySubscriptions>)), Is.EqualTo(1));
        }

        public class MulticastMessageWithManySubscriptions { }
        public class TestMultiSubscription1 : ISubscribeTo<MulticastMessageWithManySubscriptions>
        {
            public void Handle(MulticastMessageWithManySubscriptions @event) { }
            public void OnError(MulticastMessageWithManySubscriptions @event, Exception ex) { }
        }
        public class TestMultiSubscription2 : ISubscribeTo<MulticastMessageWithManySubscriptions>
        {
            public void Handle(MulticastMessageWithManySubscriptions @event) { }
            public void OnError(MulticastMessageWithManySubscriptions @event, Exception ex) { }
        }

    }
    
    public class FakePublisher : IPublishToMessageBuses
    {
        public void Publish(object @event)
        {
            throw new NotImplementedException();
        }
    }
}
