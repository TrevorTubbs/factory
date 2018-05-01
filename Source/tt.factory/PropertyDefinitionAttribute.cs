using System;

namespace tt.factory {
    /// <summary>
    /// Defines valid property values.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class PropertyDefinitionAttribute : Attribute {
        /// <summary>
        /// A valid value.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// A function to determine if a value is valid for the property.
        /// The function must take the proposed value as an argument
        /// and return a boolean. If the function returns true, the value
        /// is considered a valid value for the property. The function
        /// must be defined in the class. It does not need to be public.
        /// It must be static.
        /// </summary>
        public string ValidationFunction { get; set; }
    }
}
