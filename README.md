ValueObject
===========

ValueObject is a micro library for easily creating C#/VB classes with value semantics. The library provides an abstract base class that overrides Equals, GetHashCode, and the `==` and `!=` operators. It also implements IEquatable.

Install
------------
The library is provided as a [NuGet package](https://www.nuget.org/packages/ValueObject/). To install, run the following command in your package manager console:

```
Install-Package ValueObject
```

Use
------------
To create a value object, simply inherit from ValueObject. By default, two objects will be considered equal if they are the same type and all of their public properties and fields are equal.

To prevent a public property or field from being considered in memberwise comparisons, decorate it with an `IgnoreMember` attribute.

```c#
class Customer : ValueObject
{
    public string Name { get; set; }
    public int Age;

    private int ssn;  //ignored in comparisons
    
    [IgnoreMember]
    public string Address;  //also ignored because of attribute
}
```
```c#

var customer1 = new Customer{ Name = "John", Age = 13, Address = "California" };
var customer2 = new Customer{ Name = "John", Age = 13, Address = "Florida" };

Debug.Assert(customer1 == customer2);
```

Use F# Records?
---------------
Are you sure you don't want to just [create F# records in a F# project, and consume them from C#](https://www.pluralsight.com/tech-blog/immutable-objects-in-csharp/)? 
