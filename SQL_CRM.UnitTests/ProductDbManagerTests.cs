using Microsoft.VisualStudio.TestTools.UnitTesting;

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

        [TestMethod]
        public void TestConnection()
        {
            var list = _manager.Read(new Product());

            Assert.AreEqual(4, list.Count);
        }

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
    }
}
