using System;
using System.Linq;

namespace SQL_CRM
{
    public class Program
    {
        static readonly ConsoleWindowFrame mainWindow = new ConsoleWindowFrame();

        private static CustomerDbManager _dbManager;

        private static void Main(string[] args)
        {
            _dbManager =
                new CustomerDbManager(System.Configuration.ConfigurationManager.ConnectionStrings["Kundregister"].ConnectionString);

            mainWindow.Width = 80;
            mainWindow.Height = 20;

            mainWindow.StartRender();

            var mainQuestion = new Question(
                "Skriv in ett val",
                "Skapa en kund,Ny kund,Ny",
                "Visa alla kunder,Visa",
                "Ändra en kund,Ändra",
                "Tabort en kund,Tabort",
                "Rensa skärmen,Rensa,Cls",
                "Avsluta,Quit,Exit"
            );

            var running = true;

            while (running)
            {
                try
                {
                    var input = mainWindow.GetInputWithQuestion(mainQuestion);
                    if (input == "Skapa en kund")
                    {
                        SystemMessage("Skapa en kund!");

                        var customer = CreateCustomer();

                        _dbManager.Create(customer);

                        SystemMessage($"Ny kund skapad {customer}");
                    }
                    else if (input == "Ändra en kund")
                    {
                        SystemMessage("Ändra en kund");

                        var customer = FindCustomer();

                        SystemMessage("Hittat kund:");
                        PrintCustomer(customer);

                        var newcustomer = ChangeCustomer(customer);

                        _dbManager.Update(newcustomer);


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
                    else if (input == "Tabort en kund")
                    {
                        SystemMessage("Tabort en kund");


                        var customer = FindCustomer();
                        _dbManager.Delete(customer);

                        SystemMessage("Tog bort kund:");
                        PrintCustomer(customer);
                    }
                    else if (input == "Visa alla kunder")
                    {
                        SystemMessage("Hämtar alla kunder:");

                        var list = _dbManager.GetAllCustomer();

                        foreach (var customer in list)
                        {
                            PrintCustomer(customer);
                        }
                    }
                    else if (input == "Rensa skärmen")
                    {
                        mainWindow.Clear();
                    }
                    else if (input == "Avsluta")
                    {
                        running = false;
                    }
                    else
                    {
                        ErrorMessage("No valid input");
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

        private static ICustomer ChangeCustomer(ICustomer customer)
        {
            PrintCustomer(customer);

            var ret = FillCustomer("Vad vill du ändra, spara ändringarna med Ändra", "Ändra");
            ret.CustomerId = customer.CustomerId;
            return ret;
        }

        private static void ChangeCustomerFirstName(ICustomer customer)
        {
            customer.FirstName = mainWindow.GetInputWithQuestion("Vad är kundens förnamn:");
        }

        private static void ChangeCustomerLastName(ICustomer customer)
        {
            customer.LastName = mainWindow.GetInputWithQuestion("Vad är kundens efternamn:");
        }

        private static void ChangeCustomerEmail(ICustomer customer)
        {
            customer.Email = mainWindow.GetInputWithQuestion("Vad är kundens epost:");
        }

        private static void ChangeCustomerPhoneNumber(ICustomer customer)
        {
            customer.PhoneNumber = mainWindow.GetInputWithQuestion("Vad är kundens telefonnumer:");
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
                    customer.PhoneNumber = mainWindow.GetInputWithQuestion("Skriv in ett telefonnummer:");
                }
            } while (input != exit);

            return customer;
        }

        private static ICustomer FindCustomer()
        {

            var customer = FillCustomer("Vad vill du söka på, välj sök när du är klar", "Sök");

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
                            var ret = $"{item.ToString()}";

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

        private static void DeleteCustomer()
        {

        }

        private static Customer CreateCustomer()
        {

            var firstName = mainWindow.GetInputWithQuestion("Skriv in kundens förnamn:");
            var lastName = mainWindow.GetInputWithQuestion("Skriv in kundens efternamn:");
            var Email = mainWindow.GetInputWithQuestion("Skriv in kundens email, lämna tomt om saknas:");
            var PhoneNumber = mainWindow.GetInputWithQuestion("Skriv in kundens telefonnummer, lämna tomt om saknas:");

            var customer = new Customer(firstName, lastName, Email, PhoneNumber);

            return customer;
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

