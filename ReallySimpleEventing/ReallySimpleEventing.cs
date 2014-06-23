using System;
using System.Collections.Generic;
using ReallySimpleEventing.ActivationStrategies;
using ReallySimpleEventing.ThreadingStrategies;

namespace ReallySimpleEventing
{
    public class ReallySimpleEventing
    {
        private static readonly ReallySimpleEventingWorker Instance;
        
        static ReallySimpleEventing()
        {
            Instance = new ReallySimpleEventingWorker();
        }

        public static void RegisterModule(IReallySimpleEventingRegistrationModule registrationModule)
        {
            Instance.RegisterModule(registrationModule);
        }

        public static IEventStream CreateEventStream()
        {
            return Instance.CreateEventStream();
        }

        public static ReallySimpleEventingConfiguration Configuration
        {
            get { return Instance.Configuration; }
            set { Instance.Configuration = value; }
        }

        [Obsolete("Please move to using ReallySimpleEventing.Configuration.ActivationStrategy")]
        public static IHandlerActivationStrategy ActivationStrategy 
        {
            get { return Instance.Configuration.ActivationStrategy; }
            set { Instance.Configuration.ActivationStrategy = value; } 
        }

        [Obsolete("Please move to using ReallySimpleEventing.Configuration.ThreadingStrategies")]
        public static List<IHandlerThreadingStrategy> ThreadingStrategies
        {
            get { return Instance.Configuration.ThreadingStrategies; }
            set { Instance.Configuration.ThreadingStrategies = value; }
        }

        [Obsolete("Please move to using ReallySimpleEventing.Configuration.WhenErrorsAreNotHandled")]
        public static Action<object, Exception> WhenErrorsAreNotHandled
        {
            get { return Instance.Configuration.WhenErrorsAreNotHandled; }
            set { Instance.Configuration.WhenErrorsAreNotHandled = value; }
        }
    }
}