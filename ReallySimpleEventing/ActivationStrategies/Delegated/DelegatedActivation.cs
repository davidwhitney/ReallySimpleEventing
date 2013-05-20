using System;
using System.Collections.Generic;
using ReallySimpleEventing.EventHandling;

namespace ReallySimpleEventing.ActivationStrategies.Delegated
{
    public class DelegatedActivation : IHandlerActivationStrategy
    {
        private readonly IEventHandlerResolver _eventHandlerResolver;
        private readonly Func<Type, object> _createHandler;

        public DelegatedActivation(Func<Type, object> createHandler)
            :this(new EventHandlerResolver(), createHandler)
        {
        }

        public DelegatedActivation(IEventHandlerResolver eventHandlerResolver, Func<Type, object> createHandler)
        {
            _eventHandlerResolver = eventHandlerResolver;
            _createHandler = createHandler;
        }

        public IEnumerable<IHandle<TEventType>> GetHandlers<TEventType>()
        {
            var types = _eventHandlerResolver.GetHandlerTypesForEvent(typeof(TEventType));

            foreach (var type in types)
            {
                yield return (IHandle<TEventType>)_createHandler(type);
            }
        }
    }
}