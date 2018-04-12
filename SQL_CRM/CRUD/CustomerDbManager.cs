﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SQL_CRM
{
    public class CustomerDbManager : DbManager, ICustomerDbManager
    {
        public CustomerDbManager(string conString) : base(conString)
        {

        }
        
        public void CreateCustomer(ICustomer customer)
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
        
        public List<ICustomer> GetCustomersFromCustomer(ICustomer customer)
        {
            var list = new List<ICustomer>();

            var where = new List<string>();
            Action<SqlCommand> setParameters = null;

            var sql = "SELECT [Customer].Id, [Customer].FirstName, [Customer].LastName, [Customer].Email, " +
                        "stuff(" +
                        "( " +
                            "SELECT cast(', ' AS VARCHAR(max)) + [PhoneNr].PhoneNr " +
                            "FROM PhoneNr " +
                            "WHERE PhoneNr.CustomerId = Customer.Id ";

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
                    sql += " AND PhoneNr LIKE @PhoneNr ";
                    setParameters += (command) =>
                    {
                        command.Parameters.Add(new SqlParameter("PhoneNr", $"%{customer.PhoneNumber}%"));
                    };
                }
            }

            sql += "FOR xml path('')" +
                   ")" +
                   ", 1, 1, ''" +
                   ") AS Phone " +
                   "FROM Customer";
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
                    list.Add(CreateCustomerFromSqlReader(reader));
                }
            });

            return list;
        }
        
        public List<ICustomer> GetAllCustomer()
        {
            return GetCustomersFromCustomer(null);
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
            return GetCustomersFromCustomer(new Customer()
            {
                PhoneNumber = phoneNumber
            });
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