using System;
using System.Collections.Generic;
using System.Linq;
using ReallySimpleEventing.EventHandling;

namespace ReallySimpleEventing.ActivationStrategies.Delegated
{
    public class DelegatedActivation : IHandlerActivationStrategy
    {
        protected Func<Type, IEnumerable<object>> _createHandler;

        public DelegatedActivation(Func<Type, IEnumerable<object>> createHandler)
        {
            _createHandler = createHandler;
        }

        public IEnumerable<IHandle<TEventType>> GetHandlers<TEventType>()
        {
            var handlers = _createHandler(typeof (TEventType));
            return handlers.Cast<IHandle<TEventType>>();
        }
    }
}
