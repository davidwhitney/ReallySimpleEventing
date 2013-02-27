namespace ReallySimpleEventing
{
    public interface IEventStream
    {
        void Raise<TEventType>(TEventType @event);
    }
}