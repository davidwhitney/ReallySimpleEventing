using System;
using System.Collections.Generic;
using System.Linq;
using ReallySimpleEventing.ActivationStrategies;
using ReallySimpleEventing.EventHandling;

namespace ReallySimpleEventing.MessageBus
{
    public class InterceptedHandlerActivationStrategy : IHandlerActivationStrategy
    {
        private readonly IPublishToMessageBuses _publisher;
        public IHandlerActivationStrategy OriginalStrategy { get; set; }

        public InterceptedHandlerActivationStrategy(IHandlerActivationStrategy originalStrategy, IPublishToMessageBuses publisher)
        {
            _publisher = publisher;
            OriginalStrategy = originalStrategy;
        }
        
        public IEnumerable<IHandle<TEventType>> GetHandlers<TEventType>()
        {
            var handlersToInvoke = new List<IHandle<TEventType>>();
            handlersToInvoke.AddRange(OriginalStrategy.GetHandlers<TEventType>());

            if (handlersToInvoke.Any(x => x is ISubscribeTo<TEventType>))
            {
                handlersToInvoke.Add(new PublishingHandler<TEventType>(_publisher));
            }

            return handlersToInvoke;
        }

        public void OnHandlerExecuted<TEventType>(IHandle<TEventType> handler)
        {
        }
    }
}