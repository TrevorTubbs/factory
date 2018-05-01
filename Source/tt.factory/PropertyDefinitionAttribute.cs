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
    }
}
