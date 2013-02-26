using ReallySimpleEventing.ActivationStrategies;
using ReallySimpleEventing.EventHandling;

namespace ReallySimpleEventing
{
    public class ReallySimpleEventing
    {
        public static IEventHandlerResolver EventHandlerResolver { get; set; }
        public static IHandlerExecutionStrategy ExecutionStrategy { get; set; }

        static ReallySimpleEventing()
        {
            EventHandlerResolver = new EventHandlerResolver();
            ExecutionStrategy = new ActivatorExecution();
        }

        public static IEventBus CreateEventBus(bool async = false)
        {
            return new EventBus(EventHandlerResolver, ExecutionStrategy, async); 
        }
    }
}