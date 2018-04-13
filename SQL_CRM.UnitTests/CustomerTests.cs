using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using SQL_CRM.DataObjects;

namespace SQL_CRM.UnitTests
{
    [TestClass]
    public class CustomerTests
    {
        [TestMethod]
        public void TestCustomerCreation_PhoneNrLogic()
        {
            Assert.AreEqual("123", new Customer("Test", "asdf", "fdsafdsa", "123 ").PhoneNumber);
            Assert.AreEqual("123", new Customer("Test", "asdf", "fdsafdsa", " 123").PhoneNumber);
            Assert.AreEqual(null, new Customer("Test", "asdf", "fdsafdsa", " ").PhoneNumber);
            Assert.AreEqual("1234, 4321", new Customer("Test", "asdf", "fdsafdsa", "1234, 4321 ").PhoneNumber);


            //Assert.AreEqual(2, new Customer(null, "asdf", "asdf", "fdsa", new List<string> { "123", "543" }, new List<Product>()).PhoneNumbers.Count);
            //Assert.AreEqual(new List<string> { "123", "543" }, new Customer(null, "asdf", "asdf", "fdsa", new List<string> { "123", "543" }, new List<Product>()).PhoneNumbers);
        }
    }
}
