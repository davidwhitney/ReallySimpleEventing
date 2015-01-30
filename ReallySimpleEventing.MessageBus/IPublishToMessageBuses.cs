namespace ReallySimpleEventing.MessageBus
{
    public interface IPublishToMessageBuses
    {
        void Publish(object @event);
    }
}
