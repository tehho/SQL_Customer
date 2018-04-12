using System;
using System.Linq;

namespace SQL_CRM
{
    public class CustomerGui : AdministrateGui<ICustomer>
    {

        public CustomerGui(ConsoleWindowFrame mainWindow) 
            : base(mainWindow, 
                new CustomerDbManager(System.Configuration.ConfigurationManager.ConnectionStrings["Kundregister"].ConnectionString))
        {
        }

        public override void Administrate()
        {
            var mainQuestion = new Question(
                "Skriv in ett val",
                "Skapa en kund,Ny kund,Ny",
                "Visa alla kunder,Visa",
                "Ändra en kund,Ändra",
                "Lägg till ett telefonnummer på befintlig kund, Lägg till, Telefon",
                "Tabort en kund,Tabort",
                "Tabort ett telefonnummer på en kund,Tabort telefon",
                "Tillbaka"
            );

            var input = _mainWindow.GetInputWithQuestion(mainQuestion);

            if (input == "Skapa en kund")
            {
                Create();
            }
            else if (input == "Visa alla kunder")
            {
                ReadAll();
            }
            else if (input == "Ändra en kund")
            {
                Update();
            }
            else if (input == "Lägg till ett telefonnummer på befintlig kund")
            {
                AddPhoneNr();
            }
            else if (input == "Tabort en kund")
            {
                Delete();
            }
            else if (input == "Tabort ett telefonnummer på en kund")
            {
                DeletePhoneNr();
            }
        }

        public override void Create()
        {
            _mainWindow.SystemMessage("Skapa en kund!");

            var firstName = _mainWindow.GetInputWithQuestion("Skriv in kundens förnamn:");
            var lastName = _mainWindow.GetInputWithQuestion("Skriv in kundens efternamn:");
            var email = _mainWindow.GetInputWithQuestion("Skriv in kundens email, lämna tomt om saknas:");
            var phoneNumber = _mainWindow.GetInputWithQuestion("Skriv in kundens telefonnummer, lämna tomt om saknas:");

            var customer = new Customer(firstName, lastName, email, phoneNumber);

            _dbManager.Create(customer);

            _mainWindow.SystemMessage($"Ny kund skapad {customer}");
        }

        public override void ReadAll()
        {
            _mainWindow.SystemMessage("Hämtar alla kunder:");

            var list = _dbManager.Read(null);

            foreach (var customer in list)
            {
                Program.Print(customer);
            }
        }

        public override void Update()
        {
            _mainWindow.SystemMessage("Ändra en kund");

            var customer = Find();

            if (customer != null)
            {
                _mainWindow.SystemMessage("Hittat kund:");
                Program.Print(customer);

                var newcustomer = ChangeCustomer(customer);

                _dbManager.Update(newcustomer);


                _mainWindow.SystemMessage("Ändrat värden:");

                if (newcustomer.FirstName != null)
                    _mainWindow.SystemMessage($"Förnamn: {customer.FirstName} {newcustomer.FirstName}");
                if (newcustomer.LastName != null)
                    _mainWindow.SystemMessage($"Efternamn: {customer.LastName} {newcustomer.LastName}");
                if (newcustomer.Email != null)
                    _mainWindow.SystemMessage($"Epost: {customer.Email} {newcustomer.Email}");
                if (newcustomer.PhoneNumber != null)
                    _mainWindow.SystemMessage($"Telefonnummer: {customer.PhoneNumber} {newcustomer.PhoneNumber}");
            }
            else
            {
                _mainWindow.ErrorMessage("No product found");
            }
        }

        public override void Delete()
        {
            _mainWindow.SystemMessage("Tabort en kund");

            var customer = Find();
            _dbManager.Delete(customer);

            _mainWindow.SystemMessage("Tog bort kund:");
            Program.Print(customer);
        }

        public override ICustomer Find()
        {
            var customer = Fill("Vad vill du söka på, välj sök när du är klar", "Sök");

            var list = _dbManager.Read(customer);

            if (list.Count == 1)
                return list[0];

            if (list.Count == 0)
            {
                return null;
            }

            while (true)
            {
                try
                {
                    var temp_List = list.Select(
                        (item) =>
                        {
                            var ret = $"'{item.ToString()}'";

                            return ret;
                        }).ToList();
                    var input = _mainWindow.GetInputWithQuestion(new Question("Vilken kund vill du välja", temp_List.ToArray()));

                    customer = list.Find((item) => item.ToString() == input);
                    return customer;
                }
                catch (FormatException e)
                {
                    _mainWindow.ErrorMessage("Not a valid input");
                }
            }
        }
        
        private void AddPhoneNr()
        {
            _mainWindow.SystemMessage("Lägg till telefonnummer på befintligt kund");

            var customer = new Customer()
            {
                CustomerId = Find().CustomerId,
                PhoneNumber = _mainWindow.GetInputWithQuestion("Skriv in ett telefonnummer:")
            };

            _dbManager.Update(customer);
        }
        private void DeletePhoneNr()
        {
            _mainWindow.SystemMessage("Tabort ett telefonnummer på en kund");

            var customer = Find();

            if (customer.PhoneNumbers.Count == 1)
            {
                customer = new Customer()
                {
                    CustomerId = customer.CustomerId,
                    PhoneNumber = customer.PhoneNumber
                };
                ((CustomerDbManager)_dbManager).DeletePhoneNr(customer);
            }
        }
        private ICustomer Fill(string question, string exit)
        {
            Customer customer = new Customer();

            Question quest = new Question(question, "Förnamn", "Efternamn", "Email", "Telefonnummer", exit);

            var input = "";
            do
            {
                input = _mainWindow.GetInputWithQuestion(quest);
                if (input == "Förnamn")
                {
                    customer.FirstName = _mainWindow.GetInputWithQuestion("Skriv in ett namn:");
                }
                else if (input == "Efternamn")
                {
                    customer.LastName = _mainWindow.GetInputWithQuestion("Skriv in ett efternamn:");
                }
                else if (input == "Email")
                {
                    customer.Email = _mainWindow.GetInputWithQuestion("Skriv in en epost:");
                }
                else if (input == "Telefonnummer")
                {
                    customer.AddPhoneNumber = _mainWindow.GetInputWithQuestion("Skriv in ett telefonnummer:");
                }
            } while (input != exit);

            return customer;
        }
        private ICustomer ChangeCustomer(ICustomer customer)
        {
            Program.Print(customer);

            var ret = Fill("Vad vill du ändra, spara ändringarna med Ändra", "Ändra");
            ret.CustomerId = customer.CustomerId;
            return ret;
        }


    }
}