using System;
using System.Collections.Generic;
using System.Linq;

namespace ReallySimpleEventing
{
    public static class AppDomainExtensionsNet4x
    {
        public static IEnumerable<Type> GetAllTypes(this AppDomain appDomain)
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes());
        }
    }
}