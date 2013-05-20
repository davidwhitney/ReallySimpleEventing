using System;

namespace ReallySimpleEventing.ActivationStrategies.Delegated
{
    [Obsolete("Please use DelegatedActivationWithDiscovery - will be removed in later versions")]
    public class DelegatedActivation : DelegatedActivationWithDiscovery
    {
        public DelegatedActivation(Func<Type, object> createHandler)
            : base(createHandler)
        {
        }

        public DelegatedActivation(IEventHandlerResolver eventHandlerResolver,
                                                Func<Type, object> createHandler)
            : base(eventHandlerResolver, createHandler)
        {
        }
    }
}