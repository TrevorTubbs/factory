namespace tt.factory {
	internal class TypeCreationFeedback<T> where T : class {
		public T Instance { get; set; }
		public bool AttributesExisted { get; set; }
	}
}
