using System;
using System.Collections.Generic;
using ReallySimpleEventing.ActivationStrategies;
using ReallySimpleEventing.ThreadingStrategies;

namespace ReallySimpleEventing
{
    public class ReallySimpleEventing
    {
        public static ReallySimpleEventingConfiguration Configuration { get; set; }
        
        static ReallySimpleEventing()
        {
            Configuration = new ReallySimpleEventingConfiguration
            {
                ThreadingStrategies = new List<IHandlerThreadingStrategy> // Order is important for selection
                {
                    new NullMessageBusPublishingStrategy(),
                    new TaskOfT(),
                    new CurrentThread() // Default
                }
            };
        }

        public static void WithExtension(IReallySimpleEventingRegistrationModule registrationModule)
        {
            registrationModule.Bootstrap(Configuration);
        }

        public static IEventStream CreateEventStream()
        {
            return new EventStream(Configuration); 
        }

        [Obsolete("Please move to using ReallySimpleEventing.Configuration.ActivationStrategy")]
        public static IHandlerActivationStrategy ActivationStrategy 
        {
            get { return Configuration.ActivationStrategy; }
            set { Configuration.ActivationStrategy = value; } 
        }

        [Obsolete("Please move to using ReallySimpleEventing.Configuration.ThreadingStrategies")]
        public static List<IHandlerThreadingStrategy> ThreadingStrategies
        {
            get { return Configuration.ThreadingStrategies; }
            set { Configuration.ThreadingStrategies = value; }
        }

        [Obsolete("Please move to using ReallySimpleEventing.Configuration.WhenErrorsAreNotHandled")]
        public static Action<object, Exception> WhenErrorsAreNotHandled
        {
            get { return Configuration.WhenErrorsAreNotHandled; }
            set { Configuration.WhenErrorsAreNotHandled = value; }
        }
    }
}