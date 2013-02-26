using System;
using ReallySimpleEventing.EventHandling;

namespace ReallySimpleEventing.ActivationStrategies
{
    public interface IHandlerActivationStrategy
    {
        IHandle<THandlerType> CreateHandlerFor<THandlerType>(Type type);
    }
}