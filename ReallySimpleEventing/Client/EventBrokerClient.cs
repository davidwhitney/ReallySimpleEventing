namespace ReallySimpleEventing.Client
{
    public class EventBrokerClient
    {
        public void Event<TEventType>(TEventType @event)
        {
            // raise event here
        }
    }
}
