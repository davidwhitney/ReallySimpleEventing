using System;
using ReallySimpleEventing.EventHandling;

namespace ReallySimpleEventing.ActivationStrategies
{
    public interface IHandlerActivationStrategy
    {
        void ExecuteHandler<TEventType>(Type handlerType, Action<IHandle<TEventType>> unknown);
    }
}