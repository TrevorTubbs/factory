using System;
using System.Reflection;

using System.Collections.Generic;

namespace tt.factory {
    /// <summary>
    /// Factory for creating objects.
    /// </summary>
    public static class Factory {
        /// <summary>
        /// Creates an instance of the specified type.
        /// </summary>
        /// <typeparam name="T">The type to create.</typeparam>
        /// <param name="properties">The properties to set.</param>
        public static T Create<T>(Dictionary<string, object> properties = null) where T : class {
            T rtn = null;
            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies()) {
                rtn = Create<T>(a, properties);
                if (rtn != null)
                    return rtn;
            }

            return null;
        }
        /// <summary>
        /// Creates an instance of the specified type.
        /// </summary>
        /// <typeparam name="T">The type to create.</typeparam>
        /// <param name="assembly">The assmblies to search.</param>
        /// <param name="properties">The properties to set.</param>
        public static T Create<T>(Assembly assembly, Dictionary<string, object> properties = null) where T : class {
            return Create<T>(assembly.GetTypes(), properties);
        }
        /// <summary>
        /// Creates an instance of the specified type.
        /// </summary>
        /// <typeparam name="T">The type to create.</typeparam>
        /// <param name="types">The types to search.</param>
        /// <param name="properties">The properties to set.</param>
        public static T Create<T>(Type[] types, Dictionary<string, object> properties = null) where T : class {
            foreach (Type t in types) {
                if (typeof(T).IsAssignableFrom(t) && t.GetCustomAttribute<ClassDefinitionAttribute>() != null) {
                    T rtn = CreateFromType<T>(t, properties);
                    if (rtn != null) {
                        return rtn;
                    }
                }
            }

            return null;
        }

        private static T CreateFromType<T>(Type type, Dictionary<string, object> properties) where T : class {
            var c = type.GetConstructor(new Type[0]);

            int expectedAssignments = properties?.Count ?? 0;
            List<Tuple<PropertyInfo, object>> assignments = null;
            if (properties != null) {
                var propertyNames = properties.Keys;
                foreach (string propertyName in propertyNames) {
                    var property = type.GetProperty(propertyName);
                    if (property != null && PropertyAllowsValue(property, properties[propertyName])) {
                        if (assignments == null) {
                            assignments = new List<Tuple<PropertyInfo, object>>();
                        }
                        assignments.Add(new Tuple<PropertyInfo, object>(property, properties[propertyName]));
                    }
                }
            }

            if (expectedAssignments != (assignments?.Count ?? 0)) {
                return null;
            }

            T rtn = c.Invoke(new object[0]) as T;
            if (assignments != null) {
                foreach (Tuple<PropertyInfo, object> assingment in assignments) {
                    assingment.Item1.SetMethod.Invoke(rtn, new[] { assingment.Item2 });
                }
            }
            
            return rtn;
        }

        private static bool PropertyAllowsValue(PropertyInfo property, object value) {
            var attributes = property.GetCustomAttributes<PropertyDefinitionAttribute>();
            if (attributes != null) {
                foreach (PropertyDefinitionAttribute attribute in attributes) {
                    if ((value == null && attribute.Value == null) || (value != null && attribute.Value != null && value.Equals(attribute.Value))) {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
