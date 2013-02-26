namespace ReallySimpleEventing.EventHandling
{
    public interface IHandle<in TEventType>
    {
        void Handle(TEventType @event);
    }
}