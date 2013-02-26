using System;
using ReallySimpleEventing.EventHandling;

namespace ReallySimpleEventing.ActivationStrategies
{
    public class DelegatedActivation : IHandlerActivationStrategy
    {
        private readonly Func<Type, object> _createHandler;

        public DelegatedActivation(Func<Type, object> createHandler)
        {
            _createHandler = createHandler;
        }

        public void ExecuteHandler<TEventType>(Type handlerType, Action<IHandle<TEventType>> handle)
        {
            var handler = (IHandle<TEventType>)_createHandler(handlerType);
            handle(handler);
        }
    }
}
