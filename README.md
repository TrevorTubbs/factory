*How does the factory find to instantiate?*
Add the ClassDefinitionAttribute to any class you want to be available to the factory. Use the Code property to add an additional restriction to types that will be selected by the factory. If the Code property is set the factory will only instantiate it for clients that provide the code. The code is not case sensitive.

*How do I use the factory to create an instance of an object?*
The generic parameter designates the type of object to create. It does not need to be a concrete type. The dictionary provides key-value pairs corresponding to properties on the object.

Here is an example that expects an instance of the IName interface. It will look for a class with a 'Name' property that can be set to 'Sam'. The default constructor for the class will be called and then the setter for the Name property will be used to set the name to 'Sam'.
Factory.Create<IName>(new Dictionary<string, object>() { { "Name", "Sam" } });

*What properties are available?*
Add the PropertyDefinitionAttribute to any property you want to be available to the factory.

*How do I control what values can be assigned to a property?*
There are two properties on PropertyDefinitionAttribute; ValidationFunction and Value. ValidationFunction is checked first. The property will only be assigned with a value that is valid according to one of these two properties or if both properties are NULL.
- ValidationFunction
 A function to determine if a value is valid for the property. The function must take the proposed value as an argument and return a boolean. If the function returns true, the value is considered a valid value for the property. The function must be defined in the class. It does not need to be public. It must be static.
- Value
A valid value.
