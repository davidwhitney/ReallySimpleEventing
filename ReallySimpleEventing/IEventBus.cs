namespace ReallySimpleEventing
{
    public interface IEventBus
    {
        void Raise<TEventType>(TEventType @event);
    }
}