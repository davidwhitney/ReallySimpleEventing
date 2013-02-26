using System;
using System.Threading.Tasks;

namespace ReallySimpleEventing.ThreadingStrategies
{
    public class TaskOfT : IHandlerThreadingStrategy
    {
        public void Run(Action operation)
        {
            Task.Factory.StartNew(operation);
        }
    }
}