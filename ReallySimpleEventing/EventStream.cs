using System;
using System.Collections.Generic;
using System.Linq;
using ReallySimpleEventing.ActivationStrategies;
using ReallySimpleEventing.EventHandling;
using ReallySimpleEventing.ThreadingStrategies;

namespace ReallySimpleEventing
{
    public class EventStream : IEventStream
    {
        private readonly IHandlerActivationStrategy _activator;
        private readonly IEnumerable<IHandlerThreadingStrategy> _threadingStrategies;

        public EventStream(IHandlerActivationStrategy handlerActivation,
                           IEnumerable<IHandlerThreadingStrategy> threadingStrategies)
        {
            _activator = handlerActivation;
            _threadingStrategies = threadingStrategies;
        }

        public void Raise<TEventType>(TEventType @event)
        {
            var handlers = _activator.GetHandlers<TEventType>();

            foreach (var handler in handlers)
            {
                var threadingStrategy = SelectExecutionContext(handler);
                threadingStrategy.Run(() => SafelyHandle(handler, @event));
            }
        }

        public IHandlerThreadingStrategy SelectExecutionContext<TEventType>(IHandle<TEventType> handler)
        {
            return _threadingStrategies.First(x => x.Supports(handler));
        }

        private static void SafelyHandle<TEventType>(IHandle<TEventType> handler, TEventType @event)
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