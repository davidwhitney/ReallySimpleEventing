using System;
using System.Collections.Generic;

namespace ReallySimpleEventing.EventHandlerResolution
{
    public class TypeToHandlerMapping
    {
        public Type EventType { get; set; }
        public List<Type> HandlerTypes { get; set; }

        public TypeToHandlerMapping(Type type)
        {
            EventType = type;
            HandlerTypes = new List<Type>();
        }
    }
}