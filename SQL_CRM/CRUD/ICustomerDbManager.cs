using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SQL_CRM
{
    public interface ICustomerDbManager : ICrud<ICustomer>
    {
        List<ICustomer> GetCustomerFromFirstName(string firstName);
        List<ICustomer> GetCustomerFromLastName(string lastName);
        List<ICustomer> GetCustomerFromEmail(string email);
        List<ICustomer> GetCustomerFromPhoneNumber(string phoneNumber);
        List<ICustomer> GetAllCustomer();
        void Query(string sqlQuery, Action<SqlCommand> method);
    }
}