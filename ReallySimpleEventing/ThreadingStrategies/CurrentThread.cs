using System;
using ReallySimpleEventing.EventHandling;

namespace ReallySimpleEventing.ThreadingStrategies
{
    public class CurrentThread : IHandlerThreadingStrategy
    {
        public bool Supports<TEventType>(IHandle<TEventType> handler)
        {
            return handler != null;
        }

        public void Run(Action operation)
        {
            operation();
        }
    }
}