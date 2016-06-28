using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ReallySimpleEventing.EventHandling;

namespace ReallySimpleEventing.ActivationStrategies.Delegated
{
    public class DelegatedActivationWithDiscovery : IHandlerActivationStrategy
    {
        private readonly IEventHandlerResolver _eventHandlerResolver;
        private readonly Func<Type, object> _createHandler;
        private readonly Func<Type, object> _createHandlerAsync;

        public DelegatedActivationWithDiscovery(Func<Type, object> createHandler,
            Func<Type, object> createHandlerAsync = null)
            : this(new EventHandlerResolver(), createHandler, createHandlerAsync)
        {
        }

        public DelegatedActivationWithDiscovery(IEventHandlerResolver eventHandlerResolver,
            Func<Type, object> createHandler, Func<Type, object> createHandlerAsync = null)
        {
            _eventHandlerResolver = eventHandlerResolver;
            _createHandler = createHandler;
            _createHandlerAsync = createHandlerAsync ?? createHandler;
        }

        public IEnumerable<IHandle<TEventType>> GetHandlers<TEventType>()
        {
            var types = _eventHandlerResolver.GetHandlerTypesForEvent(typeof(TEventType));

            foreach (var type in types)
            {
                if (type.GetInterfaces().Contains(typeof(IHandleAsync<TEventType>)))
                {
                    yield return (IHandle<TEventType>)_createHandlerAsync(type);
                }
                else
                {
                    yield return (IHandle<TEventType>) _createHandler(type);
                }
            }
        }

        public virtual void OnHandlerExecuted<TEventType>(IHandle<TEventType> handler)
        {
        }
    }
}