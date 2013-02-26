using System;

namespace ReallySimpleEventing.ThreadingStrategies
{
    public class CurrentThread : IHandlerThreadingStrategy
    {
        public void Run(Action operation)
        {
            operation();
        }
    }
}