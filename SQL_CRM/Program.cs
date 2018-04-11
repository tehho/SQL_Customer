using System;
using System.Collections.Generic;
using System.Linq;

namespace SQL_CRM
{
    internal class Program
    {
        static ConsoleWindowFrame mainWindow = new ConsoleWindowFrame();

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
                "Ändra en kund,Ändra",
                "Tabort en kund,Tabort",
                "Visa alla kunder,Visa",
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
                        var cust = CreateCustomer();

                        _dbManager.CreateCustomer(cust);
                    }
                    else if (input == "Ändra en kund")
                    {
                        var customer = FindCustomer();
                        
                        customer = ChangeCustomer(customer);

                        _dbManager.UpdateCustomer(customer);
                    }
                    else if (input == "Tabort en kund")
                    {
                        DeleteCustomer();

                    }
                    else if (input == "Visa alla kunder")
                    {
                        var list = _dbManager.GetAllCustomer();

                        foreach (var customer in list)
                        {
                            mainWindow.Add(new WebMessage("Customer", customer.ToString(), ConsoleColor.Green));
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
                }
                catch (Exception e)
                {
                    ErrorMessage(e.Message);
                }
            }

            mainWindow.PressAnyKeyToContinue();

            mainWindow.Abort();
        }

        private static Customer ChangeCustomer(Customer customer)
        {
            var ret = FillCustomer("Vad vill du ändra, spara ändringarna med Ändra", "Ändra");
            ret.CustomerId = customer.CustomerId;
            return ret;
        }

        private static void ChangeCustomerFirstName(Customer customer)
        {
            customer.FirstName = mainWindow.GetInputWithQuestion("Vad är kundens förnamn:");
        }

        private static void ChangeCustomerLastName(Customer customer)
        {
            customer.LastName = mainWindow.GetInputWithQuestion("Vad är kundens efternamn:");
        }

        private static void ChangeCustomerEmail(Customer customer)
        {
            customer.Email = mainWindow.GetInputWithQuestion("Vad är kundens epost:");
        }

        private static void ChangeCustomerPhoneNumber(Customer customer)
        {
            customer.PhoneNumber = mainWindow.GetInputWithQuestion("Vad är kundens telefonnumer:");
        }

        private static Customer FillCustomer(string question, string exit)
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
                    customer.LastName = mainWindow.GetInputWithQuestion("Skriv in ett namn:");
                }
                else if (input == "Email")
                {
                    customer.Email = mainWindow.GetInputWithQuestion("Skriv in ett namn:");
                }
                else if (input == "Telefonnummer")
                {
                    customer.PhoneNumber = mainWindow.GetInputWithQuestion("Skriv in ett namn:");
                }
            } while (input != exit);

            return customer;
        }

        private static Customer FindCustomer()
        {
            Customer customer = FillCustomer("Vad vill du söka på, välj sök när du är klar", "Sök");
            
            var list = _dbManager.GetCustomersFromCustomer(customer);

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
                    var input = mainWindow.GetInputWithQuestion(new Question("Vilken kund vill du välja", list.Select(
                            (item) =>
                            {
                                var ret = $"{item.FirstName} {item.FirstName},{item.FirstName},{item.LastName}";

                                if (item.Email != null)
                                    ret += $",{item.Email}";

                                if (item.PhoneNumber != null)
                                    ret += $",{item.PhoneNumber}";

                                return ret;
                            }).ToArray()));

                    return list.Find((item) => item.ToString() == input);
                }
                catch (FormatException e)
                {
                    ErrorMessage("Not a valid input");
                }
            }
        }

        private static void DeleteCustomer()
        {
            Customer customer = FindCustomer();
            if (customer == null)
            {
                ErrorMessage("Invalid customer");
                return;
            }

            _dbManager.DeleteCustomer(customer);
        }

        private static Customer CreateCustomer()
        {
            SystemMessage("Skapa en kund!");
            var firstName = mainWindow.GetInputWithQuestion("Skriv in kundens förnamn:");
            var lastName = mainWindow.GetInputWithQuestion("Skriv in kundens efternamn:");
            var Email = mainWindow.GetInputWithQuestion("Skriv in kundens email, lämna tomt om saknas:");
            var PhoneNumber = mainWindow.GetInputWithQuestion("Skriv in kundens telefonnummer, lämna tomt om saknas:");

            return new Customer(firstName, lastName, Email, PhoneNumber);
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

