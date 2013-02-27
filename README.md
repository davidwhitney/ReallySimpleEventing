ReallySimpleEventing
====================

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

Why Don't I Just Use <Some Enterprise Service Bus>?
===================================================

- They're big
- Involve extra infrastructure
- Require different skills for maintainance
- Often not needed

