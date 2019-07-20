using System;
using tt.factory.tests.resources;

namespace tt.factory.tests.Mocks {
	[ClassDefinition]
	class NamedIdentifier : INamedIdentifier {
		public int ID { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
	}
}
