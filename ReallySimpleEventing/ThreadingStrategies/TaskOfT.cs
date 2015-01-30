﻿using System;
using System.Threading.Tasks;
using ReallySimpleEventing.EventHandling;

namespace ReallySimpleEventing.ThreadingStrategies
{
    public class TaskOfT : IHandlerThreadingStrategy
    {
        public bool Supports<TEventType>(IHandle<TEventType> handler)
        {
            return handler is IHandleAsync<TEventType>;
        }

        public void Run(Action operation)
        {
            RunTask(operation);
        }

        public Task RunTask(Action operation)
        {
            return Task.Factory.StartNew(operation);
        }
    }
}