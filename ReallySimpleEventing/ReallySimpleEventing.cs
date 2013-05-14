﻿using System.Collections.Generic;
using ReallySimpleEventing.ActivationStrategies;
using ReallySimpleEventing.ActivationStrategies.Activator;
using ReallySimpleEventing.ThreadingStrategies;

namespace ReallySimpleEventing
{
    public class ReallySimpleEventing
    {
        public static IHandlerActivationStrategy ActivationStrategy { get; set; }
        public static List<IHandlerThreadingStrategy> ThreadingStrategies { get; set; }

        static ReallySimpleEventing()
        {
            ActivationStrategy = new ActivatorActivation();
            ThreadingStrategies = new List<IHandlerThreadingStrategy> // Order is important for selection
                {
                    new TaskOfT(),
                    new CurrentThread() // Default
                };
        }

        public static IEventStream CreateEventStream()
        {
            return new EventStream(ActivationStrategy, ThreadingStrategies.ToArray()); 
        }
    }
}