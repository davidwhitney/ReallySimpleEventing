using System;
using System.Collections.Generic;
using ReallySimpleEventing.ActivationStrategies;
using ReallySimpleEventing.ActivationStrategies.Activator;
using ReallySimpleEventing.ThreadingStrategies;

namespace ReallySimpleEventing
{
    public class ReallySimpleEventingConfiguration
    {
        public IHandlerActivationStrategy ActivationStrategy { get; set; }
        public List<IHandlerThreadingStrategy> ThreadingStrategies { get; set; }
        public Action<object, Exception> WhenErrorsAreNotHandled { get; set; }

        public ReallySimpleEventingConfiguration()
        {
            ActivationStrategy = new ActivatorActivation();
            ThreadingStrategies = new List<IHandlerThreadingStrategy>();
            WhenErrorsAreNotHandled = ((obj, ex) => { throw ex; });
        }
    }
}