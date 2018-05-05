namespace tt.factory.tests.Mocks {
    [ClassDefinition(Code = "Adult")]
    internal class CorporateEmployee : IName, INicknameFlag {
        [PropertyDefinition]
        public string Name { get; set; }

        [PropertyDefinition]
        public bool Nickname { get; set; }
    }
}