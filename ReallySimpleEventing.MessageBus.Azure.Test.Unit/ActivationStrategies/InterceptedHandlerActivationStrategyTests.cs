using System;
using System.Linq;
using NUnit.Framework;
using ReallySimpleEventing.ActivationStrategies.Activator;
using ReallySimpleEventing.EventHandling;
using ReallySimpleEventing.MessageBus.Azure.ActivationStrategies;
using ReallySimpleEventing.MessageBus.Azure.EventHandling;

namespace ReallySimpleEventing.MessageBus.Azure.Test.Unit.ActivationStrategies
{
    [TestFixture]
    public class InterceptedHandlerActivationStrategyTests
    {
        [Test]
        public void GetHandlers_WhenASubscriptionIsPresent_TwoHandlersReturned()
        {
            var interceptedStrat = new InterceptedHandlerActivationStrategy(new ActivatorActivation());

            var handlers = interceptedStrat.GetHandlers<MulticastMessage>();

            Assert.That(handlers.Count(), Is.EqualTo(2));
        }

        [Test]
        public void GetHandlers_WhenASubscriptionIsPresent_MutlicastingHandlerReturned()
        {
            var interceptedStrat = new InterceptedHandlerActivationStrategy(new ActivatorActivation());

            var handlers = interceptedStrat.GetHandlers<MulticastMessage>();

            Assert.That(handlers.Count(x=>x.GetType() == typeof(AzureMessageBusPublishingHandler<MulticastMessage>)), Is.EqualTo(1));
        }

        public class MulticastMessage { }

        public class TestSubscription : ISubscribeTo<MulticastMessage>
        {
            public void Handle(MulticastMessage @event) { }
            public void OnError(MulticastMessage @event, Exception ex) { }
        }
    }

}
