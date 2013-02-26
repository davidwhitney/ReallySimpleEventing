using System;
using ReallySimpleEventing.ActivationStrategies;
using ReallySimpleEventing.EventHandling;
using ReallySimpleEventing.ThreadingStrategies;

namespace ReallySimpleEventing
{
    public class EventBus : IEventBus
    {
        private readonly IEventHandlerResolver _resolver;
        private readonly IHandlerActivationStrategy _activator;
        private readonly IHandlerThreadingStrategy _thread;

        public EventBus(IEventHandlerResolver resolver,
                        IHandlerActivationStrategy handlerActivation,
                        bool runAsync = false)
        {
            _resolver = resolver;
            _activator = handlerActivation;
            _thread = runAsync ? (IHandlerThreadingStrategy) new TaskOfT() : new CurrentThread();
        }

        public void Raise<TEventType>(TEventType @event)
        {
            var handlerTypes = _resolver.GetHandlersForEvent(@event);

            foreach (var t in handlerTypes)
            {
                _activator.ExecuteHandler<TEventType>(t, h => _thread.Run(() => Handle(@event, h)));
            }
        }

        private static void Handle<TEventType>(TEventType @event, IHandle<TEventType> handler)
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
