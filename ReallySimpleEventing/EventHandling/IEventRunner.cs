namespace ReallySimpleEventing.EventHandling
{
    public interface IEventRunner
    {
        void ProcessEvent(object @event);
    }
}