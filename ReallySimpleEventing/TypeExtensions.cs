using System;
using System.Linq;
using ReallySimpleEventing.EventHandling;

namespace ReallySimpleEventing
{
    public static class TypeExtensions
    {
        private static readonly Type HandlerType = typeof(IHandle<>);

        public static Type HandledType(this Type type)
        {
            var interfaces = type.GetInterfaces();
            var handlingInterface = interfaces.First(IsHandler);
            return handlingInterface.GetGenericArguments()[0];
        }

        public static bool IsHandler(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == HandlerType;
        }
    }
}