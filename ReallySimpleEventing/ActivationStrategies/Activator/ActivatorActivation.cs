using System;
using System.Collections.Generic;
using System.Linq;
using ReallySimpleEventing.EventHandling;

namespace ReallySimpleEventing.ActivationStrategies.Activator
{
    public class ActivatorActivation : IHandlerActivationStrategy
    {
        private IEventHandlerResolver _eventHandlerResolver;

        public ActivatorActivation() : this(new EventHandlerResolver())
        {}

        public ActivatorActivation(IEventHandlerResolver eventHandlerResolver)
        {
            _eventHandlerResolver = eventHandlerResolver;
        }
        
        public IEnumerable<IHandle<TEventType>> GetHandlers<TEventType>()
        {
            var handlerTypes = _eventHandlerResolver.GetHandlerTypesForEvent(typeof (TEventType));

            foreach (var type in handlerTypes)
            {
                yield return (IHandle<TEventType>)System.Activator.CreateInstance(type);
            }
        }
    }
}
