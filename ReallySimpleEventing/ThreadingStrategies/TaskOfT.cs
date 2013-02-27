using System;
using System.Threading.Tasks;
using ReallySimpleEventing.EventHandling;

namespace ReallySimpleEventing.ThreadingStrategies
{
    public class TaskOfT : IHandlerThreadingStrategy
    {
        public bool Supports<TEventType>(IHandle<TEventType> handler)
        {
            return handler as IHandleAsync<TEventType> != null;
        }

        public void Run(Action operation)
        {
            Task.Factory.StartNew(operation);
        }
    }
}