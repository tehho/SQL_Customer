using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SQL_CRM
{
    public class ProductDbManager : DbManager, ICrud<IProduct>, IProductDbManager
    {
        public ProductDbManager(string conString) : base(conString)
        {
        }

        public void Create(IProduct product)
        {
            throw new NotImplementedException();
        }

        public List<IProduct> Read(IProduct product)
        {
            var list = new List<IProduct>();

            var where = new List<string>();
            Action<SqlCommand> setParameters = null;

            var sql = "SELECT [Product].Id, [Product].Name " +
                      "FROM Product";

            if (product != null)
            {
                if (product.Name != null)
                {
                    where.Add("Customer.Name = @Name");
                    setParameters += (command) =>
                    {
                        command.Parameters.Add(new SqlParameter("Name", product.Name));
                    };
                }
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
            throw new NotImplementedException();
        }

        public void Delete(IProduct product)
        {
            throw new NotImplementedException();
        }


        private IProduct CreateProductFromSqlReader(SqlDataReader reader)
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