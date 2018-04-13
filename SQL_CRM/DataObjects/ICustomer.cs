using System.Collections.Generic;
using SQL_CRM.ConsoleClasses;

namespace SQL_CRM.DataObjects
{
    public interface ICustomer : IPrintable
    {
        int? CustomerId { get; set; }

        string FirstName { get; set; }

        string LastName { get; set; }
        
        string FullName { get;}

        string Email { get; set; }

        List<string> PhoneNumbers { get; set; }

        List<Product> LikedProducts { get; set; }

        string PhoneNumber
        {
            get;
            set;
        }
        
        string ToString();
    }
}