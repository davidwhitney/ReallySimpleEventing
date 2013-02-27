using System;
using ReallySimpleEventing.EventHandling;
using ReallySimpleEventing.ThreadingStrategies;

namespace ReallySimpleEventing.Test.Unit
{
    public class TestThreadStrategy : IHandlerThreadingStrategy
    {
        private readonly bool _supports;
        public bool WasExecuted { get; private set; }

        public TestThreadStrategy(bool supports)
        {
            _supports = supports;
        }

        public bool Supports<TEventType>(IHandle<TEventType> handler)
        {
            return _supports;
        }

        public void Run(Action operation)
        {
            WasExecuted = true;
        }
    }
}