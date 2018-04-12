using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace SQL_CRM.UnitTests
{
    [TestClass]
    public class ProductDbManagerTests
    {
        ProductDbManager _manager = new ProductDbManager(@"Server = kaffedbserver.database.windows.net; 
            Database = KundregisterAndreasOVictor;
            Trusted_Connection = false;
            Encrypt = true;
            UID = CustomerWriter;
            Pwd = 1234test_!; ");

        //[TestMethod]
        //public void TestConnection()
        //{
        //    var list = _manager.Read(new Product());

        //    Assert.AreEqual(4, list.Count);
        //}

        //[TestMethod]
        //public void TestMock()
        //{
        //    var list = new List<IProduct>();
        //    var fakeproduct = A.Fake<IProduct>();
        //    var fakemanager = A.Fake<ICrud<IProduct>>();

        //    A.CallTo(() => fakemanager.Read(fakeproduct)).Returns(list);

        //    var reallist = fakemanager.Read(fakeproduct);

        //    A.CallTo(() => fakemanager.Read(fakeproduct)).MustHaveHappened();

        //}

        [TestMethod]
        public void TestUpdate()
        {
            var prod = new Product { Id = 1, Name = "Updated" };

            _manager.Update(prod);
            var list = _manager.Read(new Product());
            var single = list.Where(x => x.Id == 1).ToList().Single();

            Assert.AreEqual("Updated", single.Name);

            var reset = new Product { Id = 1, Name = "Resetted" };
            _manager.Update(reset);
        }
    }
}
