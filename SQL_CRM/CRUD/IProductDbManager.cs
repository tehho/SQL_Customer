using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SQL_CRM.DataObjects;

namespace SQL_CRM.CRUD
{
    public interface IProductDbManager : ICrud<IProduct>
    {
        void Query(string sqlQuery, Action<SqlCommand> method);
    }
}