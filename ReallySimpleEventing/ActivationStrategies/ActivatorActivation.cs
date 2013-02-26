using System;

namespace ReallySimpleEventing.ActivationStrategies
{
    public class ActivatorActivation : DelegatedActivation
    {
        public ActivatorActivation() : base(Activator.CreateInstance)
        {
        }
    }
}
