using System.Collections.Generic;

namespace tt.factory {
    /// <summary>
    /// Preferences to use in selecting a type to create.
    /// </summary>
    public class TypePreferences {
        /// <summary>
        /// The factory will only select classes that are defined with a matching code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// The properties to set on the newly created instance of the selected type.
        /// </summary>
        public Dictionary<string, object> Properties { get; set; }

        /// <summary>
        /// Paths to search for assemblies with matching types.
        /// </summary>
        public List<string> SearchPaths { get; set; }
    }
}
