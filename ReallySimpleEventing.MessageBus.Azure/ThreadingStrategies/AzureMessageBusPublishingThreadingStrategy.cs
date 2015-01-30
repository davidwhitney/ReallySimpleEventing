using ReallySimpleEventing.EventHandling;
using ReallySimpleEventing.ThreadingStrategies;

namespace ReallySimpleEventing.MessageBus.Azure.ThreadingStrategies
{
    public class AzureMessageBusPublishingThreadingStrategy : TaskOfT
    {
        public new bool Supports<TEventType>(IHandle<TEventType> handler)
        {
            return handler is ISubscribeTo<TEventType>;
        }
    }
}