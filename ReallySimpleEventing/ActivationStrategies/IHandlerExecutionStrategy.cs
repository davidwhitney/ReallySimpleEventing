using System;
using ReallySimpleEventing.EventHandling;

namespace ReallySimpleEventing.ActivationStrategies
{
    public interface IHandlerExecutionStrategy
    {
        void ExecuteHandler<TEventType>(Type handlerType, Action<IHandle<TEventType>> unknown);
    }
}