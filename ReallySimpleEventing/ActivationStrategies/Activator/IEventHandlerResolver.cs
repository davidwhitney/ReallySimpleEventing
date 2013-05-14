using System;
using System.Collections.Generic;

namespace ReallySimpleEventing.ActivationStrategies.Activator
{
    public interface IEventHandlerResolver
    {
        IEnumerable<Type> GetHandlerTypesForEvent(Type eventType);
    }
}