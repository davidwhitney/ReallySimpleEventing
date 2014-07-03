using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ReallySimpleEventing.ThreadingStrategies;

namespace ReallySimpleEventing.MessageBus.Azure.Test.Unit
{
    [TestFixture]
    public class AzureMessageBusPluginTests
    {
        private ReallySimpleEventingConfiguration _cfg;
        private AzureMessageBusPlugin _plugin;

        [SetUp]
        public void SetUp()
        {
            _cfg = new ReallySimpleEventingConfiguration();
            _plugin = new AzureMessageBusPlugin();
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
    }
}
