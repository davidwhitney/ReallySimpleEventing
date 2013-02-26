using System;

namespace ReallySimpleEventing.ThreadingStrategies
{
    public interface IHandlerThreadingStrategy
    {
        void Run(Action operation);
    }
}