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
    }
}
