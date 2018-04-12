using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace SQL_CRM.UnitTests
{
    [TestClass]
    public class CustomerDbManagerTests
    {
        [TestMethod]
        public void GetCustomerFromFirstNameTest()
        {
            var fakecustomer = A.Fake<ICustomer>();
            var fakelist = new List<ICustomer>();
            fakelist.Add(fakecustomer);
            var fakemanager = A.Fake<ICustomerDbManager>();

            A.CallTo(() => fakemanager.GetCustomerFromFirstName("Victor")).Returns(fakelist);
        }
    }
}
