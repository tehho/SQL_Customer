using System;
using System.Collections.Generic;

namespace SQL_CRM
{
    public class ProductDbManager : DbManager, ICrud<IProduct>
    {
        public ProductDbManager(string conString) : base(conString)
        {
        }

        public string Create(IProduct obj)
        {
            throw new NotImplementedException();
        }

        public List<IProduct> Read(IProduct obj)
        {
            throw new NotImplementedException();
        }

        public void Update(IProduct obj)
        {
            throw new NotImplementedException();
        }

        public void Delete(IProduct obj)
        {
            throw new NotImplementedException();
        }
    }
}