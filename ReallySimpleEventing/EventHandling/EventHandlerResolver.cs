using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ReallySimpleEventing.EventHandling
{
    public class EventHandlerResolver : IEventHandlerResolver
    {
        private readonly Lazy<List<Type>> _allHandlers;
        private readonly ConcurrentDictionary<Type, List<Type>> _cache;

        public EventHandlerResolver()
        {
            _allHandlers = new Lazy<List<Type>>(FindAllHandlersInAppDomain);
            _cache = new ConcurrentDictionary<Type, List<Type>>();
        }

        public IEnumerable<Type> GetHandlersForEvent(object @event)
        {
            return _cache.GetOrAdd(@event.GetType(),
                                   eventType => _allHandlers.Value.Where(type => type.HandledType() == eventType).ToList());
        }

        private static List<Type> FindAllHandlersInAppDomain()
        {
            var handlers = new List<Type>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var allTypes = assembly.GetTypes();

                foreach (var type in allTypes)
                {
                    var interfaces = type.GetInterfaces();
                    if (!interfaces.Any(x=>x.IsHandler()))
                    {
                        continue;
                    }

                    handlers.Add(type);
                }
            }

            return handlers;
        }

    }

    public static class TypeExtensions
    {
        private static readonly Type HandlerType = typeof(IHandle<>);

        public static Type HandledType(this Type type)
        {
            var interfaces = type.GetInterfaces();
            var handlingInterface = interfaces.First(x => x.IsHandler());
            return handlingInterface.GetGenericArguments()[0];
        }

        public static bool IsHandler(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == HandlerType;
        }
    }
}
