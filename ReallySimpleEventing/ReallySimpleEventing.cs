using ReallySimpleEventing.EventHandling;

namespace ReallySimpleEventing
{
    public class ReallySimpleEventing
    {
        public static EventHandlerResolver EventHandlerResolver { get; set; }
        public static EventRunner EventRunner { get; set; }

        static ReallySimpleEventing()
        {
            EventHandlerResolver = new EventHandlerResolver();
            EventRunner = new EventRunner(EventHandlerResolver);
        }

        public static IEventBus CreateRealTimeEventBus()
        {
            return new RealTimeEventBus(EventRunner); 
        }
    }
}