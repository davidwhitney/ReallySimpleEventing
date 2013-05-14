using System;
using System.Linq;
using ReallySimpleEventing.EventHandling;

namespace ReallySimpleEventing
{
    public static class TypeExtensions
    {
        private static readonly Type HandlerType = typeof(IHandle<>);

        public static bool IsAMessageHandlerFor(this Type handlerType, Type messageType)
        {
            var handlerInterfaces = handlerType.GetInterfaces().Where(i=>i.IsAMessageHandler());

            foreach (var handlerInterface in handlerInterfaces)
            {
                var handledType = handlerInterface.GetGenericArguments()[0];
                if (handledType.IsAssignableFrom(messageType))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsAMessageHandler(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == HandlerType;
        }
    }
}