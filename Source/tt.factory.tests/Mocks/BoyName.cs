namespace tt.factory.tests.Mocks {
    [ClassDefinition]
    internal class BoyName : IName {
        [PropertyDefinition(Value = "George")]
        [PropertyDefinition(Value = "Albert")]
        public string Name { get; set; }

        [PropertyDefinition]
        public string Nickname { get; set; }
    }
}
