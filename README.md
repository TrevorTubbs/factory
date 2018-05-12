*How does the factory find a type to instantiate?*
Add the ClassDefinitionAttribute to any class you want to be available to the factory. Use an instance of TypePreferences to specify type requirements. If the Code property is set the factory will only instantiate it for clients that provide the code. The code is not case sensitive. The dictionary of properties will be used to find a type with corresponding properties that allows the values you have specified in the dictionary.

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

*How do I create a type that is defined in an assembly that isn't loaded in memory?*
Use the overload that takes an instance of TypePreferences. Add the path of each directory you would like to search to the SearchPaths list. If a matching type is not found in the assemblies that are loaded then these paths will be used to find a matching type.

*Repository*
https://github.com/TrevorTubbs/factory