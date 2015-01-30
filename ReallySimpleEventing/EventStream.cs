using System;
using System.Linq;
using ReallySimpleEventing.ActivationStrategies;
using ReallySimpleEventing.EventHandling;

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
                var threadingStrategy = _configuration.ThreadingStrategies.First(x => x.Supports(handler));

                var thisHandler = handler;
                threadingStrategy.Run(() => SafelyHandle(thisHandler, @event, _configuration.ActivationStrategy));
            }
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