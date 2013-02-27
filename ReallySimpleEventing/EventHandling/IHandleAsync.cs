namespace ReallySimpleEventing.EventHandling
{
    public interface IHandleAsync<in TEventType> : IHandle<TEventType>
    {
    }
}