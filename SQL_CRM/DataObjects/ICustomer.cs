using System.Collections.Generic;

namespace SQL_CRM
{
    public interface ICustomer
    {
        int? CustomerId { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
        List<string> PhoneNumbers { get; set; }

        string PhoneNumber
        {
            get;
            set;
        }

        string ToString();
    }
}