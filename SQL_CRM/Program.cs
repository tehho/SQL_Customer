using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.Common;
using System.Linq;
using System.Net.Sockets;

namespace SQL_CRM
{
    internal class Program
    {
        static ConsoleWindowFrame mainWindow = new ConsoleWindowFrame();
        
        private static CustomerDbManager _dbManager;

        private static void Main(string[] args)
        {
            _dbManager =
                new CustomerDbManager(
                    "Server = (localdb)\\mssqllocaldb; Database = CRM - Kundregister; Trusted_Connection = true;");

            mainWindow.Width = 80;
            mainWindow.Height = 20;

            mainWindow.StartRender();

            var running = true;

            while (running)
            {
                SystemMessage("1. Skapa en ny kund");
                SystemMessage("2. Ändra en kund");
                SystemMessage("3. Tabort en kund");
                SystemMessage("4. Visa alla kunder");
                SystemMessage("5. Rensa skärmen");
                SystemMessage("6. Avsluta");

                var input = mainWindow.GetInputWithQuestion("Skriv in ett val:");

                if (input == "1")
                {
                    var cust = CreateCustomer();

                    _dbManager.CreateCustomer(cust);
                }
                else if (input == "2")
                {

                    input = mainWindow.GetInputWithQuestion("Vilken kund vill du ändra:");

                    List<Customer> list = _dbManager.GetCustomerFromFirstName(input);

                    if (list.Count == 0)
                    {
                        ErrorMessage("No customer with that name!");
                    }
                    else
                    {
                        var cust = ChangeCustomer(list[0]);
                        
                        _dbManager.UpdateCustomer(cust);
                    }


                }
                else if (input == "3")
                {
                    DeleteCustomer();

                }
                else if (input == "4")
                {
                    var list = _dbManager.GetAllCustomer();

                    foreach (var customer in list)
                    {
                        mainWindow.Add(new WebMessage("Customer", customer.ToString(), ConsoleColor.Green));
                    }

                }
                else if (input == "5")
                {
                    mainWindow.Clear();
                }
                else if (input == "6"
                            || input == "quit"
                            || input == "exit")
                {
                    running = false;
                }
                else
                {
                    ErrorMessage("No valid input");
                }
            }

            mainWindow.PressAnyKeyToContinue();

            mainWindow.Abort();
        }

        private static Customer ChangeCustomer(Customer customer)
        {
            SystemMessage("1. Förnamn");
            SystemMessage("2. Efternamn");
            SystemMessage("3. Epost");
            SystemMessage("4. Telefonnummer");

            var input = mainWindow.GetInputWithQuestion("Vad vill du ändra:");

            if (input == "1")
            {
                ChangeCustomerFirstName(customer);
            }
            else if (input == "2")
            {
                ChangeCustomerLastName(customer);
            }
            else if (input == "3")
            {
                ChangeCustomerEmail(customer);
            }
            else if (input == "4")
            {
                ChangeCustomerPhoneNumber(customer);
            }

            return customer;
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
        

        private static void DeleteCustomer()
        {
            SystemMessage("1. Förnamn");
            SystemMessage("2. Efternamn");
            SystemMessage("3. Epost");
            SystemMessage("4. Telefonnummer");
            

            var input = mainWindow.GetInputWithQuestion("Vad vill du söka på:");

            List<Customer> list = new List<Customer>();

            if (input == "1")
            {
                var name = mainWindow.GetInputWithQuestion("Skriv in kundens förnamn:");
                list = _dbManager.GetCustomerFromFirstName(name);
            }
            else if (input == "2")
            {
                var name = mainWindow.GetInputWithQuestion("Skriv in kundens efternamn:");
                list = _dbManager.GetCustomerFromLastName(name);
            }
            else if (input == "3")
            {
                var epost = mainWindow.GetInputWithQuestion("Skriv in kundens epost:");

                list = _dbManager.GetCustomerFromEmail(epost);
            }
            else if (input == "4")
            {
                var phone = mainWindow.GetInputWithQuestion("Skriv in kundens telefonnummer:");
                list = _dbManager.GetCustomerFromPhoneNumber(phone);
            }

            if (list.Count > 0)
                DeleteCustomer(list[0]);
        }

        private static void DeleteCustomer(Customer customer)
        {
            if (customer == null)
            {
                ErrorMessage("Invalid customer");
                return;
            }

            _dbManager.DeleteCustomer(customer);
        }

        private static Customer CreateCustomer()
        {
            var firstName = mainWindow.GetInputWithQuestion("Enter the customers first name:");
            var lastName = mainWindow.GetInputWithQuestion("Enter the customers last name:");
            var Email = mainWindow.GetInputWithQuestion("Enter the customers email, leave blank if no email:");
            var PhoneNumber = mainWindow.GetInputWithQuestion("Enter the customers phonenumber:");

            return new Customer(firstName, lastName, Email, PhoneNumber);
        }

        

        private static Customer CreateCustomerFromSqlReader(SqlDataReader reader)
        {
            string email = null;
            string phoneNumber = null;
            var id = reader.GetInt32(0);
            var firstName = reader.GetString(1);
            var lastName = reader.GetString(2);

            try
            {
                email = reader.GetString(3);
            }
            catch (Exception e)
            {
            }

            try
            {
                phoneNumber = reader.GetString(4);
            }
            catch (Exception e)
            {
            }

            return new Customer(id, firstName, lastName, email, phoneNumber);
        }

        private static List<Customer> GetCustomers(Action<SqlCommand> setParameters,
            Func<SqlDataReader, Customer> createCustomer, string sql)
        {
            var list = new List<Customer>();

            _dbManager.Query(sql, (command) =>
            {
                setParameters(command);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(createCustomer(reader));
                }

            });

            return list;
        }

        private static void SystemMessage(string message)
        {
            mainWindow.Add(new WebMessage("System", message));
        }

        private static void ErrorMessage(string message)
        {
            mainWindow.Add(new WebMessage("Error", message, ConsoleColor.Red));
        }
    }
}
