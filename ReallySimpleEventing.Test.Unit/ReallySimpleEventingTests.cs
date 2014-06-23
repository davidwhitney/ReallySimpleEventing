using NUnit.Framework;

namespace ReallySimpleEventing.Test.Unit
{
    [TestFixture]
    public class ReallySimpleEventingTests
    {
        [Test]
        public void RegisterModule_CanModifyStaticConfiguration()
        {
            Assert.That(ReallySimpleEventing.Configuration.ThreadingStrategies, Is.Not.Empty);

            ReallySimpleEventing.RegisterModule(new ModuleThatClearsThreadingStrategies());

            Assert.That(ReallySimpleEventing.Configuration.ThreadingStrategies, Is.Empty);
        }

        private class ModuleThatClearsThreadingStrategies : IReallySimpleEventingRegistrationModule
        {
            public void Bootstrap(ReallySimpleEventingConfiguration configuration)
            {
                configuration.ThreadingStrategies.Clear();
            }
        }
    }
}
