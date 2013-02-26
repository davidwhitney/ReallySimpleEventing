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
        
        public IHandle<THandlerType> CreateHandlerFor<THandlerType>(Type type)
        {
            return (IHandle<THandlerType>)_createHandler(type);
        }
    }
}
