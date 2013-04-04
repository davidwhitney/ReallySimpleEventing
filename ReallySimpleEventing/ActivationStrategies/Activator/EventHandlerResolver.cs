using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ReallySimpleEventing.ActivationStrategies.Activator
{
    public class EventHandlerResolver : IEventHandlerResolver
    {
        private readonly Lazy<List<Type>> _allHandlers;
        private readonly ConcurrentDictionary<Type, List<Type>> _cache;

        public EventHandlerResolver() : this(FindAllHandlersInAppDomain)
        {
        }

        public EventHandlerResolver(Func<List<Type>> discoverAllIHandlers)
        {
            _allHandlers = new Lazy<List<Type>>(discoverAllIHandlers);
            _cache = new ConcurrentDictionary<Type, List<Type>>();
        }

        public IEnumerable<Type> GetHandlerTypesForEvent(Type eventType)
        {
            return _cache.GetOrAdd(eventType,
                                   x =>
                                       {
                                           var types = new List<Type>();
                                           foreach (var type in _allHandlers.Value)
                                           {
                                               if (type.HandledType() == x)
                                               {
                                                   types.Add(type);
                                               }
                                           }
                                           return types;
                                           //_allHandlers.Value.Where(type => ).ToList()
                                       });
        }

        private static List<Type> FindAllHandlersInAppDomain()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    foreach (var @interface in type.GetInterfaces())
                    {
                        if (@interface.IsHandler())
                        {
                            var x = 0;
                        }
                    }
                }
            }
            return (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                    from type in assembly.GetTypes()
                    let interfaces = type.GetInterfaces()
                    where interfaces.Any(x => x.IsHandler())
                    select type).ToList();
        }
    }
}
