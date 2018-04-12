using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SQL_CRM
{
    public interface IProductDbManager
    {
        void Create(IProduct product);
        List<IProduct> Read(IProduct product);
        void Update(IProduct product);
        void Delete(IProduct product);
        void Query(string sqlQuery, Action<SqlCommand> method);
    }
}