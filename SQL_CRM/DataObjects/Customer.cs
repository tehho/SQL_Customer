namespace SQL_CRM
{
    public class Customer : ICustomer
    {
        public int? CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public Customer()
            : this(null, null, null, null, null)
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

            Email = string.IsNullOrWhiteSpace(email) ? null : email;
            PhoneNumber = string.IsNullOrWhiteSpace(phoneNumber) ? null : phoneNumber;
        }

        public Customer(string firstName, string lastName, string email, string phoneNumber)
            : this(null, firstName, lastName, email, phoneNumber)
        {
        }

        public override string ToString()
        {
            string ret = $"{FirstName} {LastName}";

            if (!string.IsNullOrEmpty(Email))
                ret += $", {Email}";

            if (!string.IsNullOrEmpty(PhoneNumber))
                ret += $", {PhoneNumber}";
            return ret;
        }
    }
}