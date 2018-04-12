using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace SQL_CRM
{
    public class CustomerDbManager : DbManager, ICustomerDbManager, ICrud<ICustomer>
    {
        public CustomerDbManager(string conString) : base(conString)
        {

        }
<<<<<<< HEAD
        
        public void CreateCustomer(ICustomer customer)
=======

        public void Update(ICustomer customer)
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

        public void Delete(ICustomer customer)
        {
            string sql = "DELETE FROM PhoneNr WHERE CustomerId = @ID; DELETE FROM Customer WHERE Id = @ID";

            Query(sql, (command) =>
                {
                    command.Parameters.Add(new SqlParameter("ID", customer.CustomerId));

                    command.ExecuteNonQuery();
                });
        }

        public void Create(ICustomer customer)
>>>>>>> 1c0113c85cc49ae2500b6ead05752f7b9457ca64
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
<<<<<<< HEAD
        
        public List<ICustomer> GetCustomersFromCustomer(ICustomer customer)
=======

        public List<ICustomer> GetCustomerFromFirstName(string firstName)
        {
            return Read(new Customer()
            {
                FirstName = firstName
            });
        }

        public List<ICustomer> GetCustomerFromLastName(string lastName)
        {
            return Read(new Customer()
            {
                LastName = lastName
            });
        }

        public List<ICustomer> GetCustomerFromEmail(string email)
        {
            return Read(new Customer()
            {
                Email = email
            });
        }

        public List<ICustomer> GetCustomerFromPhoneNumber(string phoneNumber)
        {
            return Read(new Customer()
            {
                PhoneNumber = phoneNumber
            });
        }

        public List<ICustomer> Read(ICustomer customer)
>>>>>>> 1c0113c85cc49ae2500b6ead05752f7b9457ca64
        {
            var list = new Dictionary<int, ICustomer>();

            var where = new List<string>();
            Action<SqlCommand> setParameters = null;

            var sql =
                "SELECT [Customer].Id, [Customer].FirstName, [Customer].LastName, [Customer].Email, [PhoneNr].PhoneNr " +
                "FROM Customer ";


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
                    for (int i = 0; i < customer.PhoneNumbers.Count; i++)
                    {

                        where.Add("PhoneNr = @PhoneNr{i} ");
                        setParameters += (command) =>
                        {
                            command.Parameters.Add(new SqlParameter($"PhoneNr{i}", customer.PhoneNumber[i]));
                        };
                    }
                }
            }
            if (where.Count != 0)
            {
                sql += " Where " + string.Join(" AND ", where);
            }


            Query(sql, (command) =>
            {
                setParameters?.Invoke(command);

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    customer = CreateCustomerFromSqlReader(reader);

                    if (customer.PhoneNumber != null)
                    {
                        if (list.ContainsKey(customer.CustomerId.Value))
                        {
                            list[customer.CustomerId.Value].PhoneNumber = customer.PhoneNumber;
                        }
                        list.Add(customer.CustomerId.Value, customer);
                    }
                    else
                    {
                        list.Add(customer.CustomerId.Value, customer);
                    }
                }
            });

            return list.Values.ToList();
        }
        
        public List<ICustomer> GetAllCustomer()
        {
            return Read(null);
        }

        public void UpdateCustomer(ICustomer customer)
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

        public void DeleteCustomer(ICustomer customer)
        {
            string sql = "DELETE FROM PhoneNr WHERE CustomerId = @ID; DELETE FROM Customer WHERE Id = @ID";

            Query(sql, (command) =>
                {
                    command.Parameters.Add(new SqlParameter("ID", customer.CustomerId));

                    command.ExecuteNonQuery();
                });
        }

        public List<ICustomer> GetCustomerFromFirstName(string firstName)
        {
            return GetCustomersFromCustomer(new Customer()
            {
                FirstName = firstName
            });
        }

        public List<ICustomer> GetCustomerFromLastName(string lastName)
        {
            return GetCustomersFromCustomer(new Customer()
            {
                LastName = lastName
            });
        }

        public List<ICustomer> GetCustomerFromEmail(string email)
        {
            return GetCustomersFromCustomer(new Customer()
            {
                Email = email
            });
        }

        public List<ICustomer> GetCustomerFromPhoneNumber(string phoneNumber)
        {
            var customer = new Customer {AddPhoneNumber = phoneNumber};


            return GetCustomersFromCustomer(customer);
        }

        private static ICustomer CreateCustomerFromSqlReader(SqlDataReader reader)
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