using System;

namespace tt.factory {
    /// <summary>
    /// Add this attribute to a class that can be constructed from Factory.Create().
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class ClassDefinitionAttribute : Attribute {
        /// <summary>
        /// The factory will only select this class for clients that provide a matching code.
        /// </summary>
        public string Code { get; set; }
    }
}
