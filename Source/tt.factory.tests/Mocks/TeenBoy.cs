namespace tt.factory.tests.Mocks {
    [ClassDefinition(Code = "Teen")]
    internal class TeenBoy : BoyName, IAge {
        [PropertyDefinition(ValidationFunction = "IsValid")]
        public int Age { get; set; }

        private static bool IsValid(int age) {
            return age <= 18 && age >= 12;
        }
    }
}
