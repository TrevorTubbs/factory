using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using tt.factory.tests.Mocks;
using System.Collections.Generic;

namespace tt.factory.tests {
    [TestClass]
    public class FactoryTests {
        [TestMethod]
        public void CreateReturnsNullWhenNoTypeDefined() {
            object obj = Factory.Create<TestMethodAttribute>();

            Assert.IsNull(obj, "Create() should return null when not type is defined.");
        }

        [DataRow("Sally", typeof(GirlName))]
        [DataRow("Penny", typeof(GirlName))]
        [DataRow("George", typeof(BoyName))]
        [DataRow("Albert", typeof(BoyName))]
        [DataTestMethod]
        public void CreateNameInstance(string name, Type implementationType) {
            IName actual;

            actual = Factory.Create<IName>(new Dictionary<string, object>() { { "Name", name } });

            Assert.IsNotNull(actual, "An instance of the type should have been created.");
            Assert.IsInstanceOfType(actual, implementationType, "The instance is not the expected type.");
            Assert.AreEqual(name, actual.Name, "Name");
        }

        [DataRow("Sally", 21, typeof(Woman))]
        [DataRow("Sally", 18, typeof(Woman))]
        [DataRow("Sally", 15, typeof(TeenGirl))]
        [DataTestMethod]
        public void CreateAgeInstance(string name, int age, Type implementationType) {
            IAge actual;

            actual = Factory.Create<IAge>(new Dictionary<string, object>() { { "Name", name }, { "Age", age } });

            Assert.IsNotNull(actual, "An instance of the type should have been created.");
            Assert.IsInstanceOfType(actual, implementationType, "The instance is not the expected type.");
            Assert.AreEqual(age, actual.Age, "Age");
            Assert.AreEqual(name, ((IName)actual).Name, "Name");
        }

        [DataRow("Adult", typeof(Man))]
        [DataRow("ADULT", typeof(Man))]
        [DataRow("adult", typeof(Man))]
        [DataRow("Teen", typeof(TeenBoy))]
        [DataTestMethod]
        public void CreateLimitsToMatchingCode(string code, Type implementationType) {
            IAge actual;

            actual = Factory.Create<IAge>(new TypePreferences() {
                Code = code
            });

            Assert.IsNotNull(actual, "An instance of the type should have been created.");
            Assert.IsInstanceOfType(actual, implementationType, "The instance is not the expected type.");
        }

        [DataRow("Adult", 56, typeof(Man))]
        [DataRow("Teen", 13, typeof(TeenBoy))]
        [DataTestMethod]
        public void CreateWithCodeSetsProperties(string code, int age, Type implementationType) {
            IAge actual;

            actual = Factory.Create<IAge>(new TypePreferences() {
                Code = code,
                Properties = new Dictionary<string, object>() { { "Age", age } }
            });

            Assert.IsNotNull(actual, "An instance of the type should have been created.");
            Assert.IsInstanceOfType(actual, implementationType, "The instance is not the expected type.");
            Assert.AreEqual(age, actual.Age, "Age");
        }

        [DataRow("Adult", "Honest John", typeof(Man))]
        [DataRow("Teen", "Cool Joe", typeof(TeenBoy))]
        [DataTestMethod]
        public void CreateSetsPropertyWithoutRestrictions(string code, string nickname, Type implementationType) {
            IName actual;

            actual = Factory.Create<IName>(new TypePreferences() {
                Code = code,
                Properties = new Dictionary<string, object>() { { "Nickname", nickname } }
            });

            Assert.IsNotNull(actual, "An instance of the type should have been created.");
            Assert.IsInstanceOfType(actual, implementationType, "The instance is not the expected type.");
            Assert.AreEqual(nickname, ((BoyName)actual).Nickname, "Nickname");
        }

        [DataRow("Adult", true, typeof(CorporateEmployee))]
        [DataTestMethod]
        public void CreateSetsPropertyWithoutRestrictionsMatchingType(string code, bool nickname, Type implementationType) {
            IName actual;

            actual = Factory.Create<IName>(new TypePreferences() {
                Code = code,
                Properties = new Dictionary<string, object>() { { "Nickname", nickname } }
            });

            Assert.IsNotNull(actual, "An instance of the type should have been created.");
            Assert.IsInstanceOfType(actual, implementationType, "The instance is not the expected type.");
            Assert.AreEqual(nickname, ((INicknameFlag)actual).Nickname, "Nickname");
        }

        [TestMethod]
        public void CreateFindsTypeInReferencedAssemblyThatHasNotBeenLoaded() {
            object actual;

            actual = Factory.Create<object>(new TypePreferences() {
                Code = "HiddenType",
                SearchPaths = new List<string>() { AppDomain.CurrentDomain.BaseDirectory }
            });

            Assert.IsNotNull(actual, "An instance of the type should have been created.");
        }
    }
}
