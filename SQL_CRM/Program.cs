using System;
using System.Linq;

namespace SQL_CRM
{
    public class Program
    {
        static readonly ConsoleWindowFrame mainWindow = new ConsoleWindowFrame();

        private static CustomerDbManager CustomerDbManager;
        private static ProductDbManager ProductDbManager;

        private static void Main(string[] args)
        {
            CustomerDbManager =
                new CustomerDbManager(System.Configuration.ConfigurationManager.ConnectionStrings["Kundregister"].ConnectionString);

            ProductDbManager = 
                new ProductDbManager(System.Configuration.ConfigurationManager.ConnectionStrings["Kundregister"].ConnectionString);

            mainWindow.Width = 80;
            mainWindow.Height = 20;

            mainWindow.StartRender();

            var running = true;

            var mainQuestion = new Question("Vad vill du göra", "Administrera en kund,Kund", "Administrera en produkt,Produkt", "Rensa skärmen,CLS", "Avsluta,Quit,Exit");

            while (running)
            {
                try
                {
                    var input = mainWindow.GetInputWithQuestion(mainQuestion);

                    if (input == "Administrera en kund")
                    {
                        AdministrateCustomer();
                    }
                    else if (input == "Administrera en produkt")
                    {
                        AdministrateProduct();
                    }
                    else if (input == "Rensa skärmen")
                    {
                        mainWindow.Clear();
                    }
                    else if (input == "Avsluta")
                    {
                        running = false;
                    }
                    mainWindow.AddSeparator();
                }
                catch (Exception e)
                {
                    ErrorMessage(e.Message);
                }
            }

            mainWindow.PressAnyKeyToContinue();

            mainWindow.Abort();
        }

        private static void AdministrateCustomer()
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

            var input = mainWindow.GetInputWithQuestion(mainQuestion);

            if (input == "Skapa en kund")
            {
                CreateCustomer();
            }
            else if (input == "Visa alla kunder")
            {
                ReadAllCustomers();
            }
            else if (input == "Ändra en kund")
            {
                UpdateCustomer();
            }
            else if (input == "Lägg till ett telefonnummer på befintlig kund")
            {
                AddPhoneNr();
            }
            else if (input == "Tabort en kund")
            {
                DeleteCustomer();
            }
            else if (input == "Tabort ett telefonnummer på en kund")
            {
                DeletePhoneNr();
            }
        }
        
        private static void AdministrateProduct()
        {
            var mainQuestion = new Question("Skriv in ett val", "Skapa en produkt,Skapa", "Visa alla produkter,Visa", "Ändra en produkt,Ändra", "Tabort en produkt,Tabort", "Tillbaka");

            var input = mainWindow.GetInputWithQuestion(mainQuestion);

            if (input == "Skapa en produkt")
            {
                throw new NotImplementedException();
            }
            else if (input == "Visa alla produkter")
            {
                throw new NotImplementedException();
            }
            else if (input == "Ändra en produkt")
            {
                throw new NotImplementedException();
            }
            else if (input == "Tabort en produkt")
            {
                throw new NotImplementedException();
            }
        }

        private static void CreateCustomer()
        {
            SystemMessage("Skapa en kund!");
            
            var firstName = mainWindow.GetInputWithQuestion("Skriv in kundens förnamn:");
            var lastName = mainWindow.GetInputWithQuestion("Skriv in kundens efternamn:");
            var email = mainWindow.GetInputWithQuestion("Skriv in kundens email, lämna tomt om saknas:");
            var phoneNumber = mainWindow.GetInputWithQuestion("Skriv in kundens telefonnummer, lämna tomt om saknas:");

            var customer = new Customer(firstName, lastName, email, phoneNumber);

            CustomerDbManager.Create(customer);

            SystemMessage($"Ny kund skapad {customer}");
        }

        private static void ReadAllCustomers()
        {
            SystemMessage("Hämtar alla kunder:");

            var list = CustomerDbManager.GetAllCustomer();

            foreach (var customer in list)
            {
                PrintCustomer(customer);
            }
        }

        private static void UpdateCustomer()
        {
            SystemMessage("Ändra en kund");

            var customer = FindCustomer();

            if (customer != null)
            {
                SystemMessage("Hittat kund:");
                PrintCustomer(customer);

                var newcustomer = ChangeCustomer(customer);

                CustomerDbManager.Update(newcustomer);


                SystemMessage("Ändrat värden:");

                if (newcustomer.FirstName != null)
                    SystemMessage($"Förnamn: {customer.FirstName} {newcustomer.FirstName}");
                if (newcustomer.LastName != null)
                    SystemMessage($"Efternamn: {customer.LastName} {newcustomer.LastName}");
                if (newcustomer.Email != null)
                    SystemMessage($"Epost: {customer.Email} {newcustomer.Email}");
                if (newcustomer.PhoneNumber != null)
                    SystemMessage($"Telefonnummer: {customer.PhoneNumber} {newcustomer.PhoneNumber}");
            }
            else
            {
                ErrorMessage("No customer found");
            }
        }

        private static void AddPhoneNr()
        {
            SystemMessage("Lägg till telefonnummer på befintligt kund");

            var customer = new Customer()
            {
                CustomerId = FindCustomer().CustomerId,
                PhoneNumber = mainWindow.GetInputWithQuestion("Skriv in ett telefonnummer:")

            };
            
            CustomerDbManager.Update(customer);
        }

        private static void DeleteCustomer()
        {
            SystemMessage("Tabort en kund");
            
            var customer = FindCustomer();
            CustomerDbManager.Delete(customer);

            SystemMessage("Tog bort kund:");
            PrintCustomer(customer);
        }

        private static void DeletePhoneNr()
        {
            SystemMessage("Tabort ett telefonnummer på en kund");

            var customer = FindCustomer();

            if (customer.PhoneNumbers.Count == 1)
            {
                customer = new Customer()
                {
                    CustomerId = customer.CustomerId,
                    PhoneNumber =  customer.PhoneNumber
                };
                CustomerDbManager.DeletePhoneNr(customer);
            }

        }

        private static ICustomer ChangeCustomer(ICustomer customer)
        {
            PrintCustomer(customer);

            var ret = FillCustomer("Vad vill du ändra, spara ändringarna med Ändra", "Ändra");
            ret.CustomerId = customer.CustomerId;
            return ret;
        }

        private static ICustomer FillCustomer(string question, string exit)
        {
            Customer customer = new Customer();

            Question quest = new Question(question, "Förnamn", "Efternamn", "Email", "Telefonnummer", exit);

            var input = "";
            do
            {
                input = mainWindow.GetInputWithQuestion(quest);
                if (input == "Förnamn")
                {
                    customer.FirstName = mainWindow.GetInputWithQuestion("Skriv in ett namn:");
                }
                else if (input == "Efternamn")
                {
                    customer.LastName = mainWindow.GetInputWithQuestion("Skriv in ett efternamn:");
                }
                else if (input == "Email")
                {
                    customer.Email = mainWindow.GetInputWithQuestion("Skriv in en epost:");
                }
                else if (input == "Telefonnummer")
                {
                    customer.AddPhoneNumber = mainWindow.GetInputWithQuestion("Skriv in ett telefonnummer:");
                }
            } while (input != exit);

            return customer;
        }

        private static ICustomer FindCustomer()
        {

            var customer = FillCustomer("Vad vill du söka på, välj sök när du är klar", "Sök");

            var list = CustomerDbManager.Read(customer);

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
                    var input = mainWindow.GetInputWithQuestion(new Question("Vilken kund vill du välja", temp_List.ToArray()));

                    customer = list.Find((item) => item.ToString() == input);
                    return customer;
                }
                catch (FormatException e)
                {
                    ErrorMessage("Not a valid input");
                }
            }
        }
        
        public static void PrintCustomer(ICustomer customer)
        {
            mainWindow.Add(new WebMessage("Customer", customer.ToString(), ConsoleColor.Green));
        }

        public static void SystemMessage(string message)
        {
            mainWindow.Add(new WebMessage("System", message));
        }

        public static void ErrorMessage(string message)
        {
            mainWindow.Add(new WebMessage("Error", message, ConsoleColor.Red));
        }
    }
}

