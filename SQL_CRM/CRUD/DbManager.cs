using System;
using System.Data.SqlClient;

namespace SQL_CRM.CRUD
{
    public class DbManager
    {
        public string ConnectionString { get; set; }

        public DbManager(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public void Query(string sqlQuery, Action<SqlCommand> method)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            using (SqlCommand command = new SqlCommand(sqlQuery, con))
            {
                con.Open();

                method(command);
            }
        }
    }
}