using System;
using System.Collections.Generic;
using System.Linq;
using ReallySimpleEventing.EventHandling;

namespace ReallySimpleEventing.ActivationStrategies.Composed
{
    public class UnionOfAllActivatorsActivation : IHandlerActivationStrategy
    {
        private readonly IEnumerable<IHandlerActivationStrategy> _activators;

        public UnionOfAllActivatorsActivation(IEnumerable<IHandlerActivationStrategy> activators)
        {
            _activators = activators ?? Enumerable.Empty<IHandlerActivationStrategy>();
        }

        public IEnumerable<IHandle<TEventType>> GetHandlers<TEventType>()
        {
            var result = new Dictionary<Type, IHandle<TEventType>>();

            foreach (var activator in _activators)
            {
                var currentActivator = activator.GetHandlers<TEventType>();

                if (currentActivator != null)
                {
                    foreach (var handler in currentActivator)
                    {
                        result[handler.GetType()] = handler;//cant use a set as different instances of the same type may have different hash codes
                    }
                }
            }

            return result.Values;
        }

        public virtual void OnHandlerExecuted<TEventType>(IHandle<TEventType> handler)
        {
        }
    }
}