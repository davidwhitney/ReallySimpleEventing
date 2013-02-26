using System;

namespace ReallySimpleEventing.ActivationStrategies
{
    public class ActivatorExecution : DelegatedExecution
    {
        public ActivatorExecution() : base(Activator.CreateInstance)
        {
        }
    }
}
