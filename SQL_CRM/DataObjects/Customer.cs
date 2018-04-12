namespace SQL_CRM
{
    public class Customer : ICustomer
    {
        private int? _customerId;
        private string _firstName;
        private string _lastName;
        private string _email;
        private string _phoneNr;

        public int? CustomerId
        {
            get { return _customerId; }
            set { _customerId = value; }
        }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value?.Trim();
            }
        }

        public string LastName
        {
            get => _lastName;
            set => _lastName = value?.Trim();
        }
        public string Email
        {
            get => _email;
            set => _email = value?.Trim();
        }

        public string PhoneNumber
        {
            get => _phoneNr;
            set => _phoneNr = value?.Trim();
        }

        public Customer()
            : this(null, null, null, null, null)
        {
        }

        public Customer(ICustomer cust)
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
            var ret = $"{FirstName} {LastName}";

            if (!string.IsNullOrEmpty(Email))
                ret += $", {Email}";

            if (!string.IsNullOrEmpty(PhoneNumber))
                ret += $", {PhoneNumber}";
            return ret;
        }
    }
}