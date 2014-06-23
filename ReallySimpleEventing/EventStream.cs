using System;
using System.Linq;
using ReallySimpleEventing.ActivationStrategies;
using ReallySimpleEventing.EventHandling;
using ReallySimpleEventing.ThreadingStrategies;

namespace ReallySimpleEventing
{
    public class EventStream : IEventStream
    {
        private readonly ReallySimpleEventingConfiguration _configuration;
        
        public EventStream(ReallySimpleEventingConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Raise<TEventType>(TEventType @event)
        {
            var handlers = _configuration.ActivationStrategy.GetHandlers<TEventType>();

            foreach (var handler in handlers)
            {
                var threadingStrategy = SelectExecutionContext(handler);
                threadingStrategy.Run(() => SafelyHandle(handler, @event, _configuration.ActivationStrategy));
            }
        }

        public IHandlerThreadingStrategy SelectExecutionContext<TEventType>(IHandle<TEventType> handler)
        {
            return _configuration.ThreadingStrategies.First(x => x.Supports(handler));
        }

        private void SafelyHandle<TEventType>(IHandle<TEventType> handler, TEventType @event, IHandlerActivationStrategy handlerActivation)
        {
            try
            {
                handler.Handle(@event);
            }
            catch (Exception ex)
            {
                try { handler.OnError(@event, ex); }
                catch (Exception unhandled) { _configuration.WhenErrorsAreNotHandled(@event, unhandled); }
            }
            finally
            {
                handlerActivation.OnHandlerExecuted(handler);
            }
        }
    }
}