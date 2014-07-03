using System;
using NUnit.Framework;

namespace ReallySimpleEventing.MessageBus.Azure.Test.Unit
{
    [TestFixture]
    public class RseAzureServiceBusConfigurationTests
    {
        [Test]
        public void Ctor_ProvidedNamespace_GetsAssignedToCorrectProperty()
        {
            var cfg = new RseAzureServiceBusConfiguration("ns");

            Assert.That(cfg.SubscriptionNamespace, Is.EqualTo("ns"));
        }

        [Test]
        public void Ctor_SaneDefaultTtlSettingProvided()
        {
            var cfg = new RseAzureServiceBusConfiguration("ns");

            Assert.That(cfg.DefaultMessageTimeToLive, Is.EqualTo(new TimeSpan(1,0,0)));
        }
    }
}
