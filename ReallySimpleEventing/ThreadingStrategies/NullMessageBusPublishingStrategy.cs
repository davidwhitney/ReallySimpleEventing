using System;
using ReallySimpleEventing.EventHandling;

namespace ReallySimpleEventing.ThreadingStrategies
{
    /// <summary>
    /// Not implemented hook for messaging buses - requires companion package for Azure or Amazon to work
    /// </summary>
    public class NullMessageBusPublishingStrategy : IHandlerThreadingStrategy
    {
        public bool Supports<TEventType>(IHandle<TEventType> handler)
        {
            return handler as ISubscribeTo<TEventType> != null;
        }

        public void Run(Action operation)
        {
            throw new NotImplementedException("You published a message with an ISubscribeTo handler present - this type is designed to handle messages over message buses but you haven't configured one.");
        }
    }
}