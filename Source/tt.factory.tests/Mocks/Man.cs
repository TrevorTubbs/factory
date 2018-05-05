namespace tt.factory.tests.Mocks {
    [ClassDefinition(Code = "Adult")]
    internal class Man : BoyName, IAge {
        [PropertyDefinition(ValidationFunction = "IsValid")]
        public int Age { get; set; }

        private static bool IsValid(int age) {
            return age >= 18;
        }
    }
}