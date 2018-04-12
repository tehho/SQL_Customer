using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SQL_CRM
{
    public class CustomerDbManager : DbManager, ICustomerDbManager
    {
        public CustomerDbManager(string conString) : base(conString)
        {

        }

        public void UpdateCustomer(Customer customer)
        {
            var where = new List<string>();
            Action<SqlCommand> setParameters = null;

            if (customer != null)
            {
                if (customer.FirstName != null)
                {
                    where.Add("Customer.FirstName = @FName");
                    setParameters += (command) => command.Parameters.Add(new SqlParameter("FName", customer.FirstName));
                }

                if (customer.LastName != null)
                {
                    where.Add("Customer.LastName = @LName");
                    setParameters += (command) => command.Parameters.Add(new SqlParameter("LName", customer.LastName));
                }

                if (customer.Email != null)
                {
                    where.Add("Customer.Email = @Email");
                    setParameters += (command) => command.Parameters.Add(new SqlParameter("Email", customer.Email));
                }

                string sql = "";
                if (where.Count != 0)
                {
                    sql += $"UPDATE Customer " +
                              $"SET " + string.Join(", ", where);
                    sql += $" WHERE Customer.Id = @CustomerId;";
                }

                if (customer.PhoneNumber != null)
                {
                    sql += $" INSERT INTO PhoneNr (PhoneNr, CustomerId) VALUES (@PhoneNr, @CustomerId);";
                    setParameters += (command) => command.Parameters.Add(new SqlParameter("PhoneNr", customer.PhoneNumber));
                }

                if (sql != "")
                {
                    Query(sql,
                        (command) =>
                        {
                            setParameters?.Invoke(command);

                            command.Parameters.Add(new SqlParameter("CustomerId", customer.CustomerId));

                            command.ExecuteNonQuery();
                        });
                }
            }


        }

        public void DeleteCustomer(Customer customer)
        {
            string sql = "DELETE FROM PhoneNr WHERE CustomerId = @ID; DELETE FROM Customer WHERE Id = @ID";

            Query(sql, (command) =>
                {
                    command.Parameters.Add(new SqlParameter("ID", customer.CustomerId));

                    command.ExecuteNonQuery();
                });
        }

        public void CreateCustomer(Customer customer)
        {
            var sql = $"DECLARE @CustomerId int " +
                      $"INSERT INTO Customer (FirstName, LastName";

            if (customer.Email != null)
                sql += ", Email";

            sql += $") VALUES (@FName, @LName";

            if (customer.Email != null)
                sql += ", @Email";

            sql += ");" +
                   "SELECT @CustomerId = SCOPE_IDENTITY();";

            if (customer.PhoneNumber != null)
            {
                sql += " INSERT INTO PhoneNr (PhoneNr.PhoneNr, PhoneNr.CustomerId) VALUES (@PhoneNr, @CustomerId)";
            }

            Query(sql,
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
            return GetCustomersFromCustomer(new Customer()
            {
                FirstName = firstName
            });
        }

        public List<Customer> GetCustomerFromLastName(string lastName)
        {
            return GetCustomersFromCustomer(new Customer()
            {
                LastName = lastName
            });
        }

        public List<Customer> GetCustomerFromEmail(string email)
        {
            return GetCustomersFromCustomer(new Customer()
            {
                Email = email
            });
        }

        public List<Customer> GetCustomerFromPhoneNumber(string phoneNumber)
        {
            return GetCustomersFromCustomer(new Customer()
            {
                PhoneNumber = phoneNumber
            });
        }

        public List<Customer> GetCustomersFromCustomer(Customer customer)
        {
            var list = new List<Customer>();

            var where = new List<string>();
            Action<SqlCommand> setParameters = null;

            if (customer != null)
            {
                if (customer.FirstName != null)
                {
                    where.Add("Customer.FirstName = @FName");
                    setParameters += (command) =>
                    {
                        command.Parameters.Add(new SqlParameter("FName", customer.FirstName));
                    };
                }

                if (customer.LastName != null)
                {
                    where.Add("Customer.LastName = @LName");
                    setParameters += (command) =>
                    {
                        command.Parameters.Add(new SqlParameter("LName", customer.LastName));
                    };
                }

                if (customer.Email != null)
                {
                    where.Add("Customer.Email = @Email");
                    setParameters += (command) =>
                    {
                        command.Parameters.Add(new SqlParameter("Email", customer.Email));
                    };
                }

                if (customer.PhoneNumber != null)
                {
                    where.Add("PhoneNr.PhoneNr = @PhoneNr");
                    setParameters += (command) =>
                    {
                        command.Parameters.Add(new SqlParameter("PhoneNr", customer.PhoneNumber));
                    };
                }
            }
            var sql = "SELECT [Customer].Id, [Customer].FirstName, [Customer].LastName, [Customer].Email, " +
                      "stuff(" +
                      "     ( " +
                      "         SELECT cast(', ' AS VARCHAR(max)) + [PhoneNr].PhoneNr " +
                      "         FROM PhoneNr " +
                      "         WHERE PhoneNr.CustomerId = Customer.Id " +
                      "         FOR xml path('')" +
                      "     )" +
                      "     , 1, 1, ''" +
                      ") AS Phone" +
                      " FROM Customer";
            if (where.Count != 0)
            {
                sql += " Where " + string.Join(" and ", where);
            }


            Query(sql, (command) =>
            {
                setParameters?.Invoke(command);

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(CreateCustomerFromSqlReader(reader));
                }
            });

            return list;
        }

        public List<Customer> GetAllCustomer()
        {
            return GetCustomersFromCustomer(null);
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