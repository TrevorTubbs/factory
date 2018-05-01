namespace tt.factory.tests.Mocks {
    internal class TeenGirl : GirlName, IAge {
        [PropertyDefinition(ValidationFunction = "IsTeenAge")]
        public int Age { get; set; }

        private static bool IsTeenAge(int value) {
            return value < 18 && value > 12;
        }
    }
}
