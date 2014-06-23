using System;
using NUnit.Framework;

namespace ReallySimpleEventing.Test.Unit
{
    [TestFixture]
    public class ReallySimpleEventingConfigurationTests
    {
        [Test]
        public void Ctor_CreatesValidEmptyConfiguration()
        {
            var sut = new ReallySimpleEventingConfiguration();

            Assert.That(sut.ActivationStrategy, Is.Not.Null);
            Assert.That(sut.ThreadingStrategies, Is.Empty);
            Assert.That(sut.WhenErrorsAreNotHandled, Is.Not.Null);
        }

        [Test]
        public void Ctor_CreatesDefaultErrorHandlerThatThrowsHandledException()
        {
            var sut = new ReallySimpleEventingConfiguration();
            var handledException = new Exception("test");

            var ex = Assert.Throws<Exception>(() => sut.WhenErrorsAreNotHandled(new object(), handledException));

            Assert.That(ex, Is.EqualTo(handledException));
        }
    }
}
