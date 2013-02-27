using System;
using ReallySimpleEventing.EventHandling;

namespace ReallySimpleEventing.ThreadingStrategies
{
    public interface IHandlerThreadingStrategy
    {
        bool Supports<TEventType>(IHandle<TEventType> handler);
        void Run(Action operation);
    }
}