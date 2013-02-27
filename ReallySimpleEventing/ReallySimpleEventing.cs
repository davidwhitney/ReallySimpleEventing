using System.Collections.Generic;
using ReallySimpleEventing.ActivationStrategies;
using ReallySimpleEventing.EventHandling;
using ReallySimpleEventing.ThreadingStrategies;

namespace ReallySimpleEventing
{
    public class ReallySimpleEventing
    {
        public static IEventHandlerResolver EventHandlerResolver { get; set; }
        public static IHandlerActivationStrategy ActivationStrategy { get; set; }
        public static List<IHandlerThreadingStrategy> ThreadingStrategies { get; set; }

        static ReallySimpleEventing()
        {
            EventHandlerResolver = new EventHandlerResolver();
            ActivationStrategy = new ActivatorActivation();
            ThreadingStrategies = new List<IHandlerThreadingStrategy> // Order is important for selection
                {
                    new TaskOfT(),
                    new CurrentThread() // Default
                };
        }

        public static IEventBus CreateEventBus()
        {
            return new EventBus(EventHandlerResolver, ActivationStrategy, ThreadingStrategies.ToArray()); 
        }
    }
}