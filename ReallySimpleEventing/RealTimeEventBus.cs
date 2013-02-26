using System;
using ReallySimpleEventing.ActivationStrategies;
using ReallySimpleEventing.EventHandling;

namespace ReallySimpleEventing
{
    public class RealTimeEventBus : IEventBus
    {
        private readonly IEventHandlerResolver _handlerResolver;
        private readonly IHandlerActivationStrategy _handlerActivation;

        public RealTimeEventBus(IEventHandlerResolver handlerResolver, IHandlerActivationStrategy handlerActivation)
        {
            _handlerResolver = handlerResolver;
            _handlerActivation = handlerActivation;
        }

        public void Raise<TEventType>(TEventType @event)
        {
            var handlerTypes = _handlerResolver.GetHandlersForEvent(@event);

            foreach (var type in handlerTypes)
            {
                var handler = _handlerActivation.CreateHandlerFor<TEventType>(type);

                try
                {
                    handler.Handle(@event);
                }
                catch (Exception ex)
                {
                    handler.OnError(@event, ex);
                }
            }
        }
    }
}
