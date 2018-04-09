using System.IO;
using System.Runtime.ExceptionServices;

namespace SQL_CRM
{
    internal class Customer
    {
        public  int? CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public Customer()
            : this(-1, "UNKNOWN", "UNKNOWN","", "")
        {
        }

        public Customer(Customer cust)
            : this(cust.CustomerId, cust.FirstName, cust.LastName, cust.Email, cust.PhoneNumber)
        {
        }

        public Customer(int? id, string firstName, string lastName, string email, string phoneNumber)
        {
            CustomerId = id;
            FirstName = firstName;
            LastName = lastName;

            Email = string.IsNullOrEmpty(email) ? null: email;
            PhoneNumber = string.IsNullOrEmpty(phoneNumber) ? null: phoneNumber;
        }

        public Customer(string firstName, string lastName, string email, string phoneNumber)
            : this(null, firstName, lastName, email, phoneNumber)
        {
        }

        public override string ToString()
        {
            string ret = $"{CustomerId}: {FirstName} {LastName}";

            if (!string.IsNullOrEmpty(Email))
                ret += $", {Email}";

            if (!string.IsNullOrEmpty(PhoneNumber))
                ret += $", {PhoneNumber}";
            return ret;
        }
    }
}