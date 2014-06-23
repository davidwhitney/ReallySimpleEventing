using System;
using System.Collections.Generic;
using ReallySimpleEventing.ActivationStrategies;
using ReallySimpleEventing.ActivationStrategies.Activator;
using ReallySimpleEventing.ThreadingStrategies;

namespace ReallySimpleEventing
{
    public class ReallySimpleEventing
    {
        public static IHandlerActivationStrategy ActivationStrategy { get; set; }
        public static List<IHandlerThreadingStrategy> ThreadingStrategies { get; set; }
        public static Action<object, Exception> WhenErrorsAreNotHandled { get; set; }

        static ReallySimpleEventing()
        {
            ActivationStrategy = new ActivatorActivation();
            ThreadingStrategies = new List<IHandlerThreadingStrategy> // Order is important for selection
                {
                    new NullMessageBusPublishingStrategy(),
                    new TaskOfT(),
                    new CurrentThread() // Default
                };

            WhenErrorsAreNotHandled = ((obj, ex) => { throw ex; });
        }

        public static IEventStream CreateEventStream()
        {
            return new EventStream(ActivationStrategy, ThreadingStrategies.ToArray(), WhenErrorsAreNotHandled); 
        }
    }
}