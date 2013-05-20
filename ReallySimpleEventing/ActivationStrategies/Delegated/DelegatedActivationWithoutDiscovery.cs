using System;
using System.Collections.Generic;
using System.Linq;
using ReallySimpleEventing.EventHandling;

namespace ReallySimpleEventing.ActivationStrategies.Delegated
{
    public class DelegatedActivationWithoutDiscovery : IHandlerActivationStrategy
    {
        protected Func<Type, IEnumerable<object>> CreateHandler;

        public DelegatedActivationWithoutDiscovery(Func<Type, IEnumerable<object>> createHandler)
        {
            CreateHandler = createHandler;
        }

        public IEnumerable<IHandle<TEventType>> GetHandlers<TEventType>()
        {
            var handlers = CreateHandler(typeof (TEventType));
            return handlers.Cast<IHandle<TEventType>>();
        }
    }
}
