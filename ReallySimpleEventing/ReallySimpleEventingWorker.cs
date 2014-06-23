using System.Collections.Generic;
using ReallySimpleEventing.ThreadingStrategies;

namespace ReallySimpleEventing
{
    public class ReallySimpleEventingWorker
    {
        public ReallySimpleEventingConfiguration Configuration { get; set; }

        public ReallySimpleEventingWorker()
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

        public void RegisterModule(IReallySimpleEventingRegistrationModule registrationModule)
        {
            registrationModule.Bootstrap(Configuration);
        }

        public IEventStream CreateEventStream()
        {
            return new EventStream(Configuration); 
        }
    }
}