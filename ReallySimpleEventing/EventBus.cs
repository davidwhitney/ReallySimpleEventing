using System;
using System.Collections.Generic;
using System.Linq;
using ReallySimpleEventing.ActivationStrategies;
using ReallySimpleEventing.EventHandling;
using ReallySimpleEventing.ThreadingStrategies;

namespace ReallySimpleEventing
{
    public class EventBus : IEventBus
    {
        private readonly IEventHandlerResolver _resolver;
        private readonly IHandlerActivationStrategy _activator;
        private readonly IEnumerable<IHandlerThreadingStrategy> _threadingStrategies;

        public EventBus(IEventHandlerResolver resolver,
                        IHandlerActivationStrategy handlerActivation,
                        IEnumerable<IHandlerThreadingStrategy> threadingStrategies)
        {
            _resolver = resolver;
            _activator = handlerActivation;
            _threadingStrategies = threadingStrategies;
        }

        public void Raise<TEventType>(TEventType @event)
        {
            var handlerTypes = _resolver.GetHandlersForEvent(@event);

            foreach (var t in handlerTypes)
            {
                _activator.ExecuteHandler<TEventType>(t, handler =>
                    {
                        var thread = _threadingStrategies.First(x => x.Supports(handler));
                        thread.Run(() => Handle(handler, @event));
                    });
            }
        }

        private static void Handle<TEventType>(IHandle<TEventType> handler, TEventType @event)
        {
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
