using System;
using System.Collections.Generic;
using System.Linq;

namespace ReallySimpleEventing
{
    public static class TypeExtensionsNet4X
    {
        public static IEnumerable<Type> AllImplementedInterfaces(this Type src)
        {
            return src.GetInterfaces().ToList();
        }

        public static bool IsGenericType(this Type src)
        {
            return src.IsGenericType;
        }
    }
}