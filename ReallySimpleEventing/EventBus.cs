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

        public EventBus(IEventHandlerResolver resolver,
                        IHandlerActivationStrategy handlerActivation)
        {
            _resolver = resolver;
            _activator = handlerActivation;
        }

        public void Raise<TEventType>(TEventType @event)
        {
            var handlerTypes = _resolver.GetHandlersForEvent(@event);

            foreach (var t in handlerTypes)
            {
                _activator.ExecuteHandler<TEventType>(t, handler =>
                    {
                        var thread = SelectThread(handler);
                        thread.Run(() => Handle(handler, @event));
                    });
            }
        }

        private static IHandlerThreadingStrategy SelectThread<TEventType>(IHandle<TEventType> handler)
        {
            return handler as IHandleAsync<TEventType> != null
                       ? (IHandlerThreadingStrategy) new TaskOfT()
                       : new CurrentThread();
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
