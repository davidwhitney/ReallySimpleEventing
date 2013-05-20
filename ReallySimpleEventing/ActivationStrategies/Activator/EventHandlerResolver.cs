using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ReallySimpleEventing.ActivationStrategies.Activator
{
    public class EventHandlerResolver : IEventHandlerResolver
    {
        private readonly Lazy<IEnumerable<Type>> _allHandlers;
        private readonly ConcurrentDictionary<Type, List<Type>> _cache;

        public EventHandlerResolver() : this(FindAllHandlersInAppDomain)
        {
        }

        public EventHandlerResolver(Func<IEnumerable<Type>> discoverAllIHandlers)
        {
            _allHandlers = new Lazy<IEnumerable<Type>>(discoverAllIHandlers);
            _cache = new ConcurrentDictionary<Type, List<Type>>();
        }

        public IEnumerable<Type> GetHandlerTypesForEvent(Type eventType)
        {
            return _cache.GetOrAdd(eventType, unused =>
                                              _allHandlers.Value.Where(
                                                  handlerType => handlerType.IsAMessageHandlerFor(eventType)).ToList());
        }

        private static List<Type> FindAllHandlersInAppDomain()
        {
            return (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                    from type in assembly.GetTypes()
                    let interfaces = type.GetInterfaces()
                    where interfaces.Any(x => x.IsAMessageHandler())
                    select type).ToList();
        }
    }
}
