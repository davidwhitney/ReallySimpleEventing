using System;
using System.Collections.Generic;
using System.Reflection;

namespace ReallySimpleEventing
{
    public static class TypeExtensionsNetCore
    {
        public static IEnumerable<Type> GetInterfaces(this Type src)
        {
            return src.GetTypeInfo().ImplementedInterfaces;
        }

        public static bool IsGenericType(this Type src)
        {
            return src.GetTypeInfo().IsGenericType;
        }

        public static Type[] GetGenericArguments(this Type src)
        {
            return src.GetTypeInfo().GetGenericArguments();
        }

        public static bool IsAssignableFrom(this Type src, Type c)
        {
            return src.GetTypeInfo().IsAssignableFrom(c);
        }
    }
}