using ReallySimpleEventing.EventHandling;

namespace ReallySimpleEventing
{
    public class RealTimeEventBus : IEventBus
    {
        private readonly IEventRunner _embeddedEventRunner;

        public RealTimeEventBus(IEventRunner embeddedEventRunner)
        {
            _embeddedEventRunner = embeddedEventRunner;
        }

        public void Raise<TEventType>(TEventType @event)
        {
            _embeddedEventRunner.ProcessEvent(@event);
        }
    }
}
