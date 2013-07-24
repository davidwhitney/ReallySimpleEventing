using System;
using System.Collections.Generic;
using System.Linq;
using ReallySimpleEventing.EventHandling;

namespace ReallySimpleEventing.ActivationStrategies.Delegated
{
    public class DelegatedActivationWithoutDiscovery : IHandlerActivationStrategy
    {
        protected Func<Type, IEnumerable<object>> CreateHandlers;
        protected Func<Type, IEnumerable<object>> CreateHandlersAsync;

        public DelegatedActivationWithoutDiscovery(Func<Type, IEnumerable<object>> createHandlers, 
            Func<Type, IEnumerable<object>> createHandlersAsync = null)
        {
            CreateHandlers = createHandlers;
            CreateHandlersAsync = createHandlersAsync ?? CreateHandlers;
        }

        public IEnumerable<IHandle<TEventType>> GetHandlers<TEventType>()
        {
            var handlers = CreateHandlers(typeof (TEventType));
            return handlers.Cast<IHandle<TEventType>>();
        }

        public virtual void OnHandlerExecuted<TEventType>(IHandle<TEventType> handler)
        {
        }
    }
}