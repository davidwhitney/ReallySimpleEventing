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
        private readonly IHandlerActivationStrategy _handlerActivation;
        private readonly IEnumerable<IHandlerThreadingStrategy> _threadingStrategies;
        private readonly Action<object, Exception> _globalErrorHandler;

        public EventStream(IHandlerActivationStrategy handlerActivation,
                           IEnumerable<IHandlerThreadingStrategy> threadingStrategies,
                           Action<object, Exception> globalErrorHandler = null)
        {
            _handlerActivation = handlerActivation;
            _threadingStrategies = threadingStrategies;
            _globalErrorHandler = globalErrorHandler ?? ((obj, ex) => { throw ex; });
        }

        public void Raise<TEventType>(TEventType @event)
        {
            var handlers = _handlerActivation.GetHandlers<TEventType>();

            foreach (var handler in handlers)
            {
                var threadingStrategy = SelectExecutionContext(handler);
                threadingStrategy.Run(() => SafelyHandle(handler, @event, _handlerActivation));
            }
        }

        public IHandlerThreadingStrategy SelectExecutionContext<TEventType>(IHandle<TEventType> handler)
        {
            return _threadingStrategies.First(x => x.Supports(handler));
        }

        private void SafelyHandle<TEventType>(IHandle<TEventType> handler, TEventType @event,
            IHandlerActivationStrategy handlerActivation)
        {
            try
            {
                handler.Handle(@event);
            }
            catch (Exception ex)
            {
                try { handler.OnError(@event, ex); }
                catch (Exception unhandled) { _globalErrorHandler(@event, unhandled); }
            }
            finally
            {
                handlerActivation.OnHandlerExecuted(handler);
            }
        }
    }
}