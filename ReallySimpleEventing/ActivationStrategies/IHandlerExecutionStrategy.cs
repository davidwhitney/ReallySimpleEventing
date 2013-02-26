using System;
using ReallySimpleEventing.EventHandling;

namespace ReallySimpleEventing.ActivationStrategies
{
    public interface IHandlerExecutionStrategy
    {
        void ExecuteHandler<TEventType>(Type type, Action<IHandle<TEventType>> unknown);
    }
}