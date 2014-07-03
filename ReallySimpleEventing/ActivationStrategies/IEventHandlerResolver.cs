using System;
using System.Collections.Generic;

namespace ReallySimpleEventing.ActivationStrategies
{
    public interface IEventHandlerResolver
    {
        IEnumerable<Type> GetHandlerTypesForEvent(Type eventType);
        IEnumerable<Type> GetAllSubscriptionHandlers();
    }
}