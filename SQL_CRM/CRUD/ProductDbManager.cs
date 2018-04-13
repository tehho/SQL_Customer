using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SQL_CRM.CRUD;
using SQL_CRM.DataObjects;

namespace SQL_CRM
{
    public class ProductDbManager : DbManager, IProductDbManager
    {
        public ProductDbManager(string connectionString) : base(connectionString)
        {
        }

        public void Create(IProduct product)
        {
            var sql = $"INSERT INTO Product (Name";

            sql += $") VALUES (@Name)";

            Query(sql,
                (command) =>
                {
                    command.Parameters.Add(new SqlParameter("@Name", product.Name));

                    command.ExecuteNonQuery();
                });
        }

        public List<IProduct> Read(IProduct product)
        {
            var list = new List<IProduct>();

            var where = new List<string>();
            Action<SqlCommand> setParameters = null;

            var sql = "SELECT [Product].Id, [Product].Name " +
                      "FROM Product ";

            if (product?.Name != null)
            {
                sql += "WHERE Product.Name LIKE %@Name%";
                setParameters += (command) =>
                {
                    command.Parameters.Add(new SqlParameter("Name", product.Name));
                };
            }

            Query(sql, (command) =>
            {
                setParameters?.Invoke(command);

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(CreateProductFromSqlReader(reader));
                }
            });

            return list;
        }

        public void Update(IProduct product)
        {
            var update = new List<string>();
            Action<SqlCommand> setParameters = null;

            if (product != null)
            {
                if (product.Name != null)
                {
                    update.Add("Product.Name = @Name");
                    setParameters += (command) => command.Parameters.Add(new SqlParameter("Name", product.Name));
                }

                string sql = "";
                if (update.Count != 0)
                {
                    sql += $"UPDATE Product " +
                              $"SET " + string.Join(", ", update);
                    sql += $" WHERE Product.Id = @ProductId;";
                }

                if (sql != "")
                {
                    Query(sql,
                        (command) =>
                        {
                            setParameters?.Invoke(command);

                            command.Parameters.Add(new SqlParameter("ProductId", product.Id));

                            command.ExecuteNonQuery();
                        });
                }
            }
        }

        public void Delete(IProduct product)
        {
            string sql = "DELETE FROM Product WHERE Id = @ID";

            Query(sql, (command) =>
            {
                command.Parameters.Add(new SqlParameter("ID", product.Id));

                command.ExecuteNonQuery();
            });
        }

        public List<ICustomer> GetAllCustomer(IProduct product)
        {
            var customers = new List<ICustomer>();

            if (product?.Id != null)
            {
                var sql = $"SELECT Customer.[Id], Customer.[FirstName], Customer.[LastName]" +
                          $"FROM CustomerLikesProduct " +
                          $"INNER JOIN Customer ON CustomerLikesProduct.CustomerId = Customer.Id " +
                          $"WHERE CustomerLikesProduct.ProductId = @ProductId";

                Query(sql,
                    (command) =>
                    {
                        command.Parameters.Add(new SqlParameter("ProductId", product.Id));

                        var reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            customers.Add(CustomerDbManager.CreateCustomerFromSqlReader(reader));
                        }

                    });

            }

            return customers;
        }
        public  static IProduct CreateProductFromSqlReader(SqlDataReader reader)
        {
            var id = reader.GetInt32(0);
            var name = reader.GetString(1);

            return new Product
            {
                Id = id,
                Name = name
            };
        }
    }
}