using System;
using ReallySimpleEventing.ActivationStrategies;
using ReallySimpleEventing.EventHandling;
using ReallySimpleEventing.ThreadingStrategies;

namespace ReallySimpleEventing
{
    public class EventBus : IEventBus
    {
        private readonly IEventHandlerResolver _handlerResolver;
        private readonly IHandlerExecutionStrategy _handlerExecution;
        private readonly IHandlerThreadingStrategy _selectedThread;

        public EventBus(IEventHandlerResolver handlerResolver,
                        IHandlerExecutionStrategy handlerExecution,
                        bool runAsync = false)
        {
            _handlerResolver = handlerResolver;
            _handlerExecution = handlerExecution;
            _selectedThread = runAsync ? (IHandlerThreadingStrategy) new TaskOfT() : new CurrentThread();
        }

        public void Raise<TEventType>(TEventType @event)
        {
            var handlerTypes = _handlerResolver.GetHandlersForEvent(@event);

            foreach (var handlerType in handlerTypes)
            {
                _handlerExecution.ExecuteHandler<TEventType>(handlerType, handler =>
                    {
                        _selectedThread.Run(() => HandleWithHandler(@event, handler));
                    });
            }
        }

        private static void HandleWithHandler<TEventType>(TEventType @event, IHandle<TEventType> handler)
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
