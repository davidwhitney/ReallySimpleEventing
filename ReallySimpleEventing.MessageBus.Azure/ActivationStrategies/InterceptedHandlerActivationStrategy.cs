using System.Collections.Generic;
using System.Linq;
using ReallySimpleEventing.ActivationStrategies;
using ReallySimpleEventing.EventHandling;
using ReallySimpleEventing.MessageBus.Azure.EventHandling;

namespace ReallySimpleEventing.MessageBus.Azure.ActivationStrategies
{
    public class InterceptedHandlerActivationStrategy : IHandlerActivationStrategy
    {
        public IHandlerActivationStrategy OriginalStrategy { get; set; }

        public InterceptedHandlerActivationStrategy(IHandlerActivationStrategy originalStrategy)
        {
            OriginalStrategy = originalStrategy;
        }

        public IEnumerable<IHandle<TEventType>> GetHandlers<TEventType>()
        {
            var handlersToInvoke = new List<IHandle<TEventType>>();
            handlersToInvoke.AddRange(OriginalStrategy.GetHandlers<TEventType>());

            if (handlersToInvoke.Any(x => x is ISubscribeTo<TEventType>))
            {
                handlersToInvoke.Add(new AzureMessageBusPublishingHandler<TEventType>());
            }

            return handlersToInvoke;
        }

        public void OnHandlerExecuted<TEventType>(IHandle<TEventType> handler)
        {
        }
    }
}