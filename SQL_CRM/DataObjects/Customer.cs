using System.Collections.Generic;

namespace SQL_CRM
{
    public class Customer : ICustomer
    {
        private string _firstName;
        private string _lastName;
        private string _email;
        private List<string> _phoneNr;

        public int? CustomerId { get; set; }

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

        public List<string> PhoneNumbers
        {
            get => _phoneNr;
            set => _phoneNr = value == null ? null : new List<string>(value);
        }

        public string PhoneNumber
        {
            get => _phoneNr?.Count == 0 ? null : _phoneNr?[0];
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    if (_phoneNr == null)
                    {
                        _phoneNr = new List<string>()
                        {
                            value
                        };
                    }
                    else
                    {
                        _phoneNr.Add(value.Trim());
                    }
                }
            }
        }

        public string AddPhoneNumber { set => _phoneNr.Add(value.Trim()); }

        public Customer()
        {
            CustomerId = null;
            _firstName = null;
            _lastName = null;
            _email = null;
            _phoneNr = null;
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
            if (!string.IsNullOrWhiteSpace(phoneNumber))
            {
                _phoneNr = new List<string>();
                AddPhoneNumber = phoneNumber;
            }
            else
            {
                _phoneNr = null;
            }
        }
        public Customer(int? id, string firstName, string lastName, string email, List<string> phoneNumber)
        {
            CustomerId = id;
            FirstName = firstName;
            LastName = lastName;

            Email = string.IsNullOrWhiteSpace(email) ? null : email;
            PhoneNumbers = phoneNumber == null ? null : new List<string>(phoneNumber);
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

            if (PhoneNumbers?.Count > 0)
            {
                foreach (var phone in PhoneNumbers)
                {
                    ret += ", " + phone;
                }
            }
            return ret;
        }
    }
}