using System;

namespace tt.factory {
    /// <summary>
    /// Add this attribute to a class that can be constructed from Factory.Create().
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ClassDefinitionAttribute : Attribute {
    }
}
