using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ReallySimpleEventing.Client;
using ReallySimpleEventing.EventHandling;

namespace ReallySimpleEventing.EventHandlerResolution
{
    public class EventHandlerResolver
    {        
        public List<TypeToHandlerMapping> TypeHandlerMap { get; private set; }

        public EventHandlerResolver()
        {
            TypeHandlerMap = BuildHandlerCache();
        }

        public IEnumerable<Type> GetHandlersForEvent(object @event)
        {
            var eventType = @event.GetType();
            return GetHandlersForEvent(eventType);
        }

        private static List<TypeToHandlerMapping> BuildHandlerCache()
        {
            var events = typeof(EventBrokerClient).Assembly.GetTypes().Where(x => x.Namespace.Contains("Events"));
            return events.Select(type => new TypeToHandlerMapping(type)
            {
                HandlerTypes = GetHandlersForEvent(type).OrderBy(x => x.Name).ToList()
            }).ToList();
        }

        private static IEnumerable<Type> GetHandlersForEvent(Type eventType)
        {
            var handlers = new List<Type>();
            var allTypes = Assembly.GetExecutingAssembly().GetTypes();

            foreach (var type in allTypes)
            {
                var interfaces = type.GetInterfaces();
                var isHandler = interfaces.Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IHandle<>));

                if (!isHandler)
                {
                    continue;
                }

                var handlingInterface = interfaces.First(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IHandle<>));
                var genericArg = handlingInterface.GetGenericArguments()[0];

                if (genericArg == eventType)
                {
                    handlers.Add(type);
                }
            }

            return handlers;
        }
    }
}
