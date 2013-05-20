using ReallySimpleEventing.ActivationStrategies.Delegated;

namespace ReallySimpleEventing.ActivationStrategies.Activator
{
    public class ActivatorActivation : DelegatedActivationWithDiscovery
    {
        public ActivatorActivation() : base(System.Activator.CreateInstance)
        {
        }
    }
}
