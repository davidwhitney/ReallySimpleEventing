using ReallySimpleEventing.EventHandling;
using ReallySimpleEventing.ThreadingStrategies;

namespace ReallySimpleEventing.MessageBus
{
    public class MessageBusPublishingThreadingStrategy : TaskOfT
    {
        public new bool Supports<TEventType>(IHandle<TEventType> handler)
        {
            return handler is ISubscribeTo<TEventType>;
        }
    }
}