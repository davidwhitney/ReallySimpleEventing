using System;
using System.Collections.Generic;
using System.Linq;
using ReallySimpleEventing.EventHandling;

namespace ReallySimpleEventing.ActivationStrategies.Delegated
{
    public class DelegatedActivationWithoutDiscovery : IHandlerActivationStrategy
    {
        protected Func<Type, IEnumerable<object>> CreateHandlers;

        public DelegatedActivationWithoutDiscovery(Func<Type, IEnumerable<object>> createHandlers)
        {
            CreateHandlers = createHandlers;
        }

        public IEnumerable<IHandle<TEventType>> GetHandlers<TEventType>()
        {
            var handlers = CreateHandlers(typeof (TEventType));
            return handlers.Cast<IHandle<TEventType>>();
        }
    }
}
