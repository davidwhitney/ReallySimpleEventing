using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using ReallySimpleEventing.ActivationStrategies;
using ReallySimpleEventing.EventHandling;
using ReallySimpleEventing.MessageBus.Azure.AzureBootstrapping;
using ReallySimpleEventing.MessageBus.Azure.ThreadingStrategies;
using ReallySimpleEventing.ThreadingStrategies;

namespace ReallySimpleEventing.MessageBus.Azure.Test.Unit
{
    [TestFixture]
    public class AzureMessageBusPluginTests
    {
        private ReallySimpleEventingConfiguration _cfg;
        private AzureMessageBusPlugin _plugin;
        private RseAzureServiceBusConfiguration _busConfig;
        private Mock<IAzureTopicCreator> _mockTopicCreator;

        [SetUp]
        public void SetUp()
        {
            _busConfig = new RseAzureServiceBusConfiguration();
            _cfg = new ReallySimpleEventingConfiguration();
            _mockTopicCreator = new Mock<IAzureTopicCreator>();
            _plugin = new AzureMessageBusPlugin(_busConfig, new EventHandlerResolver(), _mockTopicCreator.Object);
        }

        [Test]
        public void Bootstrap_GivenConfiguration_AddsMessageBusHandlingStrategy()
        {
            _plugin.Bootstrap(_cfg);

            Assert.That(_cfg.ThreadingStrategies.FirstOrDefault(x => x.GetType() == typeof (AzureMessageBusPublishingStrategy)), Is.Not.Null);
        }

        [Test]
        public void Bootstrap_GivenConfiguration_RemovedDefaultNullHandler()
        {
            _cfg.ThreadingStrategies = new List<IHandlerThreadingStrategy> {new NullMessageBusPublishingStrategy()};

            _plugin.Bootstrap(_cfg);

            Assert.That(_cfg.ThreadingStrategies.FirstOrDefault(x => x.GetType() == typeof(NullMessageBusPublishingStrategy)), Is.Null);
        }

        [Test]
        public void Bootstrap_AutoRegistrationEnabled_TopicIsRegisteredForMessage()
        {
            _busConfig.AutoCreateTopics = true;

            _plugin.Bootstrap(_cfg);

            _mockTopicCreator.Verify(x=>x.CreateTopic("ReallySimpleEventing.MulticastMessage"));
        }

        public class MulticastMessage { }

        public class TestSubscription : ISubscribeTo<MulticastMessage>
        {
            public void Handle(MulticastMessage @event) { }
            public void OnError(MulticastMessage @event, Exception ex) { }
        }
    }
}
