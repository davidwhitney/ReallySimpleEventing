namespace ReallySimpleEventing.EventHandling
{
    public class EventRunner : IEventRunner
    {
        private readonly EventHandlerResolver _handlerResolver;

        public EventRunner(EventHandlerResolver handlerResolver)
        {
            _handlerResolver = handlerResolver;
        }

        public void ProcessEvent(object @event)
        {
            var handlerTypes = _handlerResolver.GetHandlersForEvent(@event);

            foreach (var type in handlerTypes)
            {
                
            }
        }
    }
}
