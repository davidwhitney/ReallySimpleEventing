using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ReallySimpleEventing.Client;
using ReallySimpleEventing.EventHandling;

namespace ReallySimpleEventing.EventHandlerResolution
{
    public class EventExecutor : IEventExecutor
    {
        private readonly EventHandlerResolver _handlerResolver;

        public EventExecutor() : this(new EventHandlerResolver())
        {
        }

        public EventExecutor(EventHandlerResolver handlerResolver)
        {
            _handlerResolver = handlerResolver;
        }

        public void ProcessEvent(object @event)
        {
            var handlerTypes = _handlerResolver.GetHandlersForEvent(@event);
        }

        
    }
}
