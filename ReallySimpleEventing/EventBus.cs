using System;
using ReallySimpleEventing.ActivationStrategies;
using ReallySimpleEventing.EventHandling;
using ReallySimpleEventing.ThreadingStrategies;

namespace ReallySimpleEventing
{
    public class EventBus : IEventBus
    {
        private readonly IEventHandlerResolver _handlerResolver;
        private readonly IHandlerExecutionStrategy _handlerFactory;
        private readonly IHandlerThreadingStrategy _thread;

        public EventBus(IEventHandlerResolver handlerResolver,
                        IHandlerExecutionStrategy handlerExecution,
                        bool runAsync = false)
        {
            _handlerResolver = handlerResolver;
            _handlerFactory = handlerExecution;
            _thread = runAsync ? (IHandlerThreadingStrategy) new TaskOfT() : new CurrentThread();
        }

        public void Raise<TEventType>(TEventType @event)
        {
            var handlerTypes = _handlerResolver.GetHandlersForEvent(@event);

            foreach (var t in handlerTypes)
            {
                _handlerFactory.ExecuteHandler<TEventType>(t, h => _thread.Run(() => Handle(@event, h)));
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
