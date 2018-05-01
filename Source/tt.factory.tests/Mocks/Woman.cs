namespace tt.factory.tests.Mocks {
    internal class Woman : GirlName, IAge {
        [PropertyDefinition(ValidationFunction = "IsAgeValid")]
        public int Age { get; set; }

        private static bool IsAgeValid(int value) {
            return value >= 18;
        }
    }
}
