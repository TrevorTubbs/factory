using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.IO;

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
            return Create<T>(properties != null ? new TypePreferences() { Properties = properties } : null);
        }
        /// <summary>
        /// Creates an instance of the specified type.
        /// </summary>
        /// <typeparam name="T">The type to create.</typeparam>
        /// <param name="preferences">Preferences to use in selecting a type to create.</param>
        public static T Create<T>(TypePreferences preferences) where T : class {
            T rtn = null;
            foreach (Assembly assembly in from a in AppDomain.CurrentDomain.GetAssemblies() select a) {
                rtn = Create<T>(assembly, preferences);
                if (rtn != null)
                    return rtn;
            }
            
            if (preferences?.SearchPaths != null && preferences.SearchPaths.Count > 0) {
                var loadedFileNames = from a in AppDomain.CurrentDomain.GetAssemblies() where !a.IsDynamic select a.Location;
                foreach (string path in from p in preferences.SearchPaths.Distinct() where !string.IsNullOrWhiteSpace(p) select p) {
                    var fileNames = from n in Directory.GetFiles(path, "*.dll") where !loadedFileNames.Contains(n) select n;
                    if (fileNames != null) {
                        foreach (string fileName in fileNames) {
                            rtn = Create<T>(Assembly.LoadFile(fileName), preferences);
                            if (rtn != null)
                                return rtn;
                        }
                    }
                }
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
            return Create<T>(assembly, properties != null ? new TypePreferences() { Properties = properties } : null);
        }
        /// <summary>
        /// Creates an instance of the specified type.
        /// </summary>
        /// <typeparam name="T">The type to create.</typeparam>
        /// <param name="assembly">The assmblies to search.</param>
        /// <param name="preferences">Preferences to use in selecting a type to create.</param>
        public static T Create<T>(Assembly assembly, TypePreferences preferences) where T : class {
			Type[] types;
			try {
				types = assembly.GetTypes();
			} catch {
				try {
					types = assembly.GetExportedTypes();
				} catch {
					types = Array.Empty<Type>();
				}
			}
			return Create<T>(types, preferences);
        }
        /// <summary>
        /// Creates an instance of the specified type.
        /// </summary>
        /// <typeparam name="T">The type to create.</typeparam>
        /// <param name="types">The types to search.</param>
        /// <param name="properties">The properties to set.</param>
        public static T Create<T>(Type[] types, Dictionary<string, object> properties = null) where T : class {
            return Create<T>(types, properties != null ? new TypePreferences() { Properties = properties } : null);
        }
        /// <summary>
        /// Creates an instance of the specified type.
        /// </summary>
        /// <typeparam name="T">The type to create.</typeparam>
        /// <param name="types">The types to search.</param>
        /// <param name="preferences">Preferences to use in selecting a type to create.</param>
        public static T Create<T>(Type[] types, TypePreferences preferences) where T : class {
            foreach (Type t in types) {
                var definitions = t.GetCustomAttributes<ClassDefinitionAttribute>();
                if (definitions != null) {
                    foreach (ClassDefinitionAttribute definition in definitions) {
                        if (typeof(T).IsAssignableFrom(t) && string.Compare(preferences?.Code, definition.Code, StringComparison.CurrentCultureIgnoreCase) == 0) {
                            T rtn = CreateFromType<T>(t, preferences);
                            if (rtn != null) {
                                return rtn;
                            }
                        }
                    }
                }
            }

            return null;
        }

        private static T CreateFromType<T>(Type type, TypePreferences preferences) where T : class {
            var c = type.GetConstructor(new Type[0]);
            if (c == null) {
                return null;
            }

            int expectedAssignments = preferences?.Properties?.Count ?? 0;
            List<Tuple<PropertyInfo, object>> assignments = null;
            if (preferences?.Properties != null) {
                var propertyNames = preferences.Properties.Keys;
                foreach (string propertyName in propertyNames) {
                    var property = type.GetProperty(propertyName);
                    if (property != null && PropertyAllowsValue(property, preferences.Properties[propertyName])) {
                        if (assignments == null) {
                            assignments = new List<Tuple<PropertyInfo, object>>();
                        }
                        assignments.Add(new Tuple<PropertyInfo, object>(property, preferences.Properties[propertyName]));
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
            if ((value == null && property.PropertyType.IsValueType) || 
                (value != null && !property.PropertyType.IsAssignableFrom(value.GetType()))) {
                return false;
            }

            var attributes = property.GetCustomAttributes<PropertyDefinitionAttribute>();
            if (attributes != null) {
                foreach (PropertyDefinitionAttribute attribute in attributes) {
                    if (!string.IsNullOrWhiteSpace(attribute.ValidationFunction)) {
                        var validation = property.ReflectedType.GetMethod(attribute.ValidationFunction, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new[] { property.PropertyType }, null);
                        if (validation != null && validation.IsStatic && validation.ReturnType == typeof(bool)) {
                            try {
                                return (bool)validation.Invoke(null, new[] { value });
                            } catch { } // Return false if no other options are successful.
                        }
                    }
                    else if (attribute.Value == null || (attribute.Value != null && attribute.Value.Equals(value))) {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
