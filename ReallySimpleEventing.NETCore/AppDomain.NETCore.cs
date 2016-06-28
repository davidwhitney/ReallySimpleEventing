using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyModel;

namespace ReallySimpleEventing
{
    public class AppDomain
    {
        public static AppDomain CurrentDomain { get; set; }

        static AppDomain()
        {
            CurrentDomain = new AppDomain();
        }

        public Assembly[] GetAssemblies()
        {
            return DependencyContext.Default
                .RuntimeLibraries
                .ToList()
                .Select(info => info.GetType().GetTypeInfo().Assembly)
                .ToArray();
        }

        public IEnumerable<Type> GetAllTypes()
        {
            return GetAssemblies().SelectMany(assembly => assembly.GetTypes());
        }
    }
}