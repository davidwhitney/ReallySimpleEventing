using System;
using System.Collections.Generic;

namespace ReallySimpleEventing.EventHandling
{
    public interface IEventHandlerResolver
    {
        IEnumerable<Type> GetHandlersForEvent(object @event);
    }
}