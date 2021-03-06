﻿using System.Collections.Generic;
using ReallySimpleEventing.EventHandling;

namespace ReallySimpleEventing.ActivationStrategies
{
    public interface IHandlerActivationStrategy
    {
        IEnumerable<IHandle<TEventType>> GetHandlers<TEventType>();
        void OnHandlerExecuted<TEventType>(IHandle<TEventType> handler);
    }
}