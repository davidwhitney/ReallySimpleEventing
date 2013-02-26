namespace ReallySimpleEventing.EventHandlerResolution
{
    public interface IEventExecutor
    {
        void ProcessEvent(object @event);
    }
}