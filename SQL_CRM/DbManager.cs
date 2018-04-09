using System;
using System.Data.SqlClient;

namespace SQL_CRM
{
    internal class DbManager
    {
        private string ConString { get; set; }

        public DbManager(string conString)
        {
            ConString = conString;
        }

        public void Query(string sqlQuery, Action<SqlCommand> method)
        {
            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand command = new SqlCommand(sqlQuery, con))
            {
                con.Open();

                method(command);
            }
        }
    }
}