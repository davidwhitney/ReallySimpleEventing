ReallySimpleEventing
====================

* Intro
* Why Do I Need It?
* Why Would I Want That?
* Why Don't I Just Use [Some Enterprise Service Bus]?
* What This Isn't
* Configuring My Container
* Containerless Usage

A tiny set of classes that add infrastructure that auto-registers events and event handlers and executes them either on the current thread or async, without any pesky bindings.

The idea is that you bind the type:

    IEventStream 
  
in your IoC container per request scope (or whatever your unit of work is).

To the method:

    _eventStream = ReallySimpleEventing.CreateEventStream();

You can then:

    _eventStream.Raise(new AnyTypeAtAll());

Anywhere in your codebase.

ReallySimpleEventing will then look for any types that implement IHandle<TTypeName> and execute them all.
Any types that implement IHandleAsync<TTypeName>() will be executed async.

An event handler is very simple, it looks like this:

    public class TestHandler<TEventType> : IHandle<TEventType>
    {
        public void Handle(TEventType @event)
        {
            // Do stuff
        }
        
        public void OnError(TEventType @event, Exception ex)
        {
            // All exceptions are surpressed by default
            // If you wish to really blow the stack, re-throw the ex here.
        }
    }
    
And they can be found anywhere in your appdomain. Obviously, multiple handlers for any given message are supported.

We also support extensibility, you can implement:

* IEventHandlerResolver - to override the handler detection
* IHandlerActivationStrategy - to hook in IoC containers
* IHandlerThreadingStrategy - to change the way the handlers are executed

You can configure these things in your bootstrapping code by setting their properties in the type

    ReallySimpleEventing
    
Apart from that, a single line IoC binding will get you up and running.

Why Do I Need It?
=================

Events, Event Sourcing, CQRS, all the rage. But you know what's really nice? Instead of having linear procedural code, having a single, obvious atomic operation (i.e. "CreateCustomer") and then having all the side effects fire off discretely, sometimes asyncly, and not right there cluttering up the place.

You can turn:

    var customer = new Customer();
    using(var tx = _session.BeginTransaction())
    {
      _session.Save(customer);
      tx.Commit();
    }

    _searchIndexService.Add(customer);
    var email = new WelcomeEmail(customer);
    _welcomeEmailService.Send(email);
    _someOtherThing.DoStuff();
    _someOtherOtherThing.DoStuff();

To:

    var customer = new Customer();
    using(var tx = _session.BeginTransaction())
    {
      _session.Save(customer);
      tx.Commit();
    }
    _events.Raise(new CustomerCreatedEvent());
  
  
And have all the event handlers live elsewhere.

Why Would I Want That?
====================

- Code cleanliness / sep. of concerns => Make sure methods do only what they say they do
- Testability => Unit tests need only test what the method does and that an event is raised
- Performance => Events can be handled off the main worker thread in web apps
- Open/Closed Principle => Code is "fixed" when complete, and extension is available by adding new event handlers.

Why Don't I Just Use [Some Enterprise Service Bus]?
===================================================

- They're big
- Involve extra infrastructure
- Require different skills for maintainance
- Often not needed

What This Isn't
===============

- A resiliant messaging thing
- EventSourcing
- CQRS
- A replayable event log


Configuring My Container
========================

By default, we use an "ActivatorActivation" strategy to create your event handlers.
What this means it that your event handler must have a public parameterless constructor so that Activator.CreateInstance works.

If you have a DI container, you can either:

* Write your own ActivationStrategy that Implements IHandlerActivationStrategy
* Use our DelegatedActivation strategy with a Func passed to it's contructor binding to your container.

You can override the ActivationStrategy by doing this in your App_Start / Bootstrapping code

    ReallySimpleEventing.ActivationStrategy = new MyActivationStrategy(); // Your own
    ReallySimpleEventing.ActivationStrategy = new DelegatedActivation(Kernel.GetService); // Delegated activation
	
The constructor for DelegatedActivation takes

	Func<Type, object> createHandler

Which tends to match a lot of containers. If you want to use a particular feature of your DI container during activation (say "scope per handler") you'll need to implement your own ActivationStrategy. Here's a quick example for Ninject...

	public class MyCleverActivationStrategyForNinject : IHandlerActivationStrategy
    {
        private readonly IKernel _ninjectKernel;

        public DelegatedActivation(IKernel ninjectKernel)
        {
            _ninjectKernel = ninjectKernel;
        }

        public void ExecuteHandler<TEventType>(Type handlerType, Action<IHandle<TEventType>> handle)
        {
			using(var scope = _ninjectKernel.BeginScope())
			{
				var handler = (IHandle<TEventType>)scope.GetService(handlerType);
				handle(handler);
			}
        }
    }
	
Containerless Usage
====================

If you're not using a DI container, you can create the IEventStream where you want to call it.

Creating an EventStream is a cheap operation, as the EventHandlerResolver is statically cached so your handler registrations and the associated reflection will only ever run once. The only expensive operation is the initial static constructor on the ReallySimpleEventing type.

You can basically do this:

	ReallySimpleEventing.CreateEventStream().Raise(new MyEvent());
	
Anywhere in your codebase to raise events. 
Obviously, due to the use of the static directly, you're going to struggle to unit test the raising of events.

It's worth bearing in mind, that when you don't use a DI container, you're bound the the default "Activator" activation strategy, meaning that any of your event handlers *must* contain a public parameterless contstructor.
