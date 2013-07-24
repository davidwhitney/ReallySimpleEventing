using System.Collections.Generic;
using System.Linq;
using ReallySimpleEventing.EventHandling;

namespace ReallySimpleEventing.ActivationStrategies.Composed
{
    public class FirstViableActivatorActivation : IHandlerActivationStrategy
    {
        private readonly IEnumerable<IHandlerActivationStrategy> _activators;

        public FirstViableActivatorActivation(IEnumerable<IHandlerActivationStrategy> activators)
        {
            _activators = activators ?? Enumerable.Empty<IHandlerActivationStrategy>();
        }

        public IEnumerable<IHandle<TEventType>> GetHandlers<TEventType>()
        {
            var fallback = Enumerable.Empty<IHandle<TEventType>>();

            foreach (var activator in _activators)
            {
                var current = (activator.GetHandlers<TEventType>() ?? fallback).ToList();

                if (current.Any())
                {
                    return current;
                }
            }

            return fallback;
        }

        public virtual void OnHandlerExecuted<TEventType>(IHandle<TEventType> handler)
        {
        }
    }
}