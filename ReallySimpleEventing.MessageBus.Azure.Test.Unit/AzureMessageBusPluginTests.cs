using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

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
        public void Bootstrap_AddsMessageBusHandlingStrategy()
        {
            _plugin.Bootstrap(_cfg);

            Assert.That(_cfg.ThreadingStrategies.Count, Is.EqualTo(1));
            Assert.That(_cfg.ThreadingStrategies[0], Is.TypeOf<AzureMessageBusPublishingStrategy>());
        }
    }
}
