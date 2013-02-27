using NUnit.Framework;
using ReallySimpleEventing.ThreadingStrategies;

namespace ReallySimpleEventing.Test.Unit.ThreadingStrategies
{
    [TestFixture]
    public class CurrentThreadTests
    {
        [Test]
        public void RunTask_CreatesATaskAndStartsIt()
        {
            var executed = false;
            var current = new CurrentThread();
            current.Run(() => executed = true);

            Assert.That(executed, Is.True);
        }
    }
}