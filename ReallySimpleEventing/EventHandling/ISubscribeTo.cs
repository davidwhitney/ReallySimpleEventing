namespace ReallySimpleEventing.EventHandling
{
    /// <summary>
    /// Publishes a message to your configured messaging bus. Not Implemented by default.
    /// </summary>
    public interface ISubscribeTo<in TEventType> : IHandleAsync<TEventType>
    {
    }
}