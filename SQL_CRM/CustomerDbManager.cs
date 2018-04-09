using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace SQL_CRM
{
    internal class CustomerDbManager : DbManager
    {
        public CustomerDbManager(string conString) : base(conString)
        {

        }

        public void UpdateCustomer(Customer customer)
        {
            var sqlQuery = $"UPDATE Customer " +
                           $"SET FirstName = @FName, LastName = @LName";

            if (customer.Email != null)
                sqlQuery += ", Email = @Email";
            if (customer.PhoneNumber != null)
                sqlQuery += $", PhoneNr = @PhoneNr ";

            sqlQuery += $"WHERE Customer.Id = {customer.CustomerId}";

            Query(sqlQuery,
                (command) =>
                {
                    command.Parameters.Add(new SqlParameter("FName", customer.FirstName));
                    command.Parameters.Add(new SqlParameter("LName", customer.LastName));

                    if (customer.Email != null)
                        command.Parameters.Add(new SqlParameter("Email", customer.Email));
                    if (customer.PhoneNumber != null)
                        command.Parameters.Add(new SqlParameter("PhoneNr", customer.PhoneNumber));

                    command.ExecuteNonQuery();
                });
        }

        public void DeleteCustomer(Customer customer)
        {
            Query("DELETE FROM Customer WHERE Id = @ID", (command) =>
                {
                    command.Parameters.Add(new SqlParameter("ID", customer.CustomerId));

                    command.ExecuteNonQuery();
                });
        }

        public void CreateCustomer(Customer customer)
        {
            var sqlQuery = $"INSERT INTO Customer (FirstName, LastName";

            if (customer.Email != null)
                sqlQuery += ", Email";
            if (customer.PhoneNumber != null)
                sqlQuery += ", PhoneNr";

            sqlQuery += $") VALUES (@FName, @LName";

            if (customer.Email != null)
                sqlQuery += ", @Email";
            if (customer.PhoneNumber != null)
                sqlQuery += ", @PhoneNr";

            sqlQuery += ")";

            Query(sqlQuery,
                (command) =>
                {
                    command.Parameters.Add(new SqlParameter("FName", customer.FirstName));
                    command.Parameters.Add(new SqlParameter("LName", customer.LastName));

                    if (customer.Email != null)
                        command.Parameters.Add(new SqlParameter("Email", customer.Email));
                    if (customer.PhoneNumber != null)
                        command.Parameters.Add(new SqlParameter("PhoneNr", customer.PhoneNumber));

                    command.ExecuteNonQuery();
                });
        }

        public List<Customer> GetCustomerFromFirstName(string firstName)
        {
            return GetCustomers("SELECT * FROM Customer WHERE FirstName = @firstName ",
                command =>
                {
                    command.Parameters.Add(new SqlParameter("firstName ", firstName));
                },
                CreateCustomerFromSqlReader);
        }

        public List<Customer> GetCustomerFromLastName(string lastName)
        {
            return GetCustomers("SELECT * FROM Customer WHERE LastName = @lastName",
                command =>
                {
                    command.Parameters.Add(new SqlParameter("LastName", lastName));
                },
                CreateCustomerFromSqlReader);
        }

        public List<Customer> GetCustomerFromEmail(string email)
        {
            return GetCustomers("SELECT * FROM Customer WHERE Email = @Email",
                command =>
                {
                    command.Parameters.Add(new SqlParameter("Email", email));
                },
                CreateCustomerFromSqlReader);
        }

        public List<Customer> GetCustomerFromPhoneNumber(string phoneNumber)
        {
            return GetCustomers("SELECT * FROM Customer WHERE PhoneNr = @PhoneNr",
                command =>
                {
                    command.Parameters.Add(new SqlParameter("PhoneNr", phoneNumber));
                },
                CreateCustomerFromSqlReader);
        }

        public List<Customer> GetAllCustomer()
        {
            return GetCustomers("SELECT * FROM Customer", null, CreateCustomerFromSqlReader);
        }

        private List<Customer> GetCustomers(string sql, Action<SqlCommand> setParameter, Func<SqlDataReader, Customer> readerMethod)
        {
            List<Customer> list = new List<Customer>();

            Query(sql, (command) =>
            {
                setParameter?.Invoke(command);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(readerMethod(reader));
                }

            });

            return list;
        }

        private Customer CreateCustomerFromSqlReader(SqlDataReader reader)
        {
            string email = null;
            string phoneNumber = null;
            var id = reader.GetInt32(0);
            var firstName = reader.GetString(1);
            var lastName = reader.GetString(2);

            try
            {
                email = reader.GetString(3);
            }
            catch (Exception e)
            {
            }

            try
            {
                phoneNumber = reader.GetString(4);
            }
            catch (Exception e)
            {
            }

            return new Customer(id, firstName, lastName, email, phoneNumber);
        }

    }
}