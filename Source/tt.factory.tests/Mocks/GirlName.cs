namespace tt.factory.tests.Mocks {
    [ClassDefinition]
    internal class GirlName : IName {
        [PropertyDefinition(Value = "Sally")]
        [PropertyDefinition(Value = "Penny")]
        public string Name { get; set; }
    }
}
