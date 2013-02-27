using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using ReallySimpleEventing.ThreadingStrategies;

namespace ReallySimpleEventing.Test.Unit.ThreadingStrategies
{
    [TestFixture]
    public class TaskOfTTests
    {
        [Test]
        public void RunTask_CreatesATaskAndStartsIt()
        {
            var tot = new TaskOfT();
            var task = tot.RunTask(() => Thread.Sleep(1000));

            Assert.That(task, Is.Not.Null);
            Assert.That(task.Status, Is.EqualTo(TaskStatus.Running));
        }
    }
}
