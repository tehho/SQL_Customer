using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SQL_CRM
{
    public interface ICustomerDbManager
    {
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(Customer customer);
        void CreateCustomer(Customer customer);
        List<ICustomer> GetCustomerFromFirstName(string firstName);
        List<Customer> GetCustomerFromLastName(string lastName);
        List<Customer> GetCustomerFromEmail(string email);
        List<Customer> GetCustomerFromPhoneNumber(string phoneNumber);
        List<Customer> GetCustomersFromCustomer(Customer customer);
        List<Customer> GetAllCustomer();
        void Query(string sqlQuery, Action<SqlCommand> method);
    }
}