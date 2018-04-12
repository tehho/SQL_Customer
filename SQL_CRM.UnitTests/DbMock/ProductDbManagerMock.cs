using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SQL_CRM.UnitTests
{
    public class ProductDbManagerMock : IProductDbManager
    {
        public List<IProduct> products = new List<IProduct>
        {
            new Product{Id = 1, Name = "Test1"},
            new Product{Id = 2, Name = "Test2"},
            new Product{Id = 3, Name = "Test3"}
        };
        public ProductDbManagerMock()
        {
        }
        public void Create(IProduct product)
        {
            throw new NotImplementedException();
        }

        public List<IProduct> Read(IProduct product)
        {
            throw new NotImplementedException();
        }

        public void Update(IProduct product)
        {
            throw new NotImplementedException();
        }

        public void Delete(IProduct product)
        {
            throw new NotImplementedException();
        }

        public void Query(string sqlQuery, Action<SqlCommand> method)
        {
            throw new NotImplementedException();
        }
    }
}
