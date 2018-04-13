using System;
using System.Data.SqlClient;
using System.Linq;
using SQL_CRM.ConsoleClasses;
using SQL_CRM.CRUD;
using SQL_CRM.DataObjects;

namespace SQL_CRM.GUI
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
                "Lägg till att en kund gillar en produkt",
                "Visa alla produkter en kund är intresserad av",
                "Tillbaka"
            );

            var input = MainWindow.GetInputWithQuestion(mainQuestion);

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
            else if (input == "Lägg till att en kund gillar en produkt")
            {
                AddCustomerLikesProduct();
            }
            else if (input == "Visa alla produkter en kund är intresserad av")
            {
                ReadAllProductsCustomerLikes();
            }
        }

        private void AddCustomerLikesProduct()
        {
            MainWindow.SystemMessage("Hämta en kund");

            var customer = Find();


            var productDb = new ProductDbManager(((DbManager)DbManager).ConnectionString);

            var products = productDb.Read(null);

            if (products != null)
            {
                foreach (var x in products)
                {
                    MainWindow.Add(x.Print());
                }

                var input = MainWindow.GetInputWithQuestion("Vilken produkt gillar kunden:");

                var product = products.Find(prod => prod.Name == input);

                if (product != null)
                {
                    string sql = "INSERT INTO CustomerLikesProduct (CustomerID, ProductID)" +
                                 "VALUES (@CustomerId, @ProductId) ";

                    ((DbManager)DbManager).Query(sql, (command) =>
                    {
                        command.Parameters.Add(new SqlParameter("CustomerId", customer.CustomerId));
                        command.Parameters.Add(new SqlParameter("ProductId", product.Id));

                        command.ExecuteNonQuery();

                    });
                }
            }


        }

        private void ReadAllProductsCustomerLikes()
        {
            MainWindow.SystemMessage("Hämta en kund");

            var customer = Find();

            MainWindow.SystemMessage($"Hämtar produkter kund {customer.FullName} gillar...");

            var products = ((CustomerDbManager)DbManager).GetAllProducts(customer);

            MainWindow.SystemMessage($"Kund {customer} gillar:");

            MainWindow.Add(customer.Print());

            MainWindow.AddSeparator();

            if (products.Count == 0)
            {
                MainWindow.SystemMessage("Inga produkter hittades");
            }
            else
            {
                foreach (var product in products)
                {
                    MainWindow.Add(product.Print());
                }
            }
        }

        public override void Create()
        {
            MainWindow.SystemMessage("Skapa en kund!");

            var firstName = MainWindow.GetInputWithQuestion("Skriv in kundens förnamn:");
            var lastName = MainWindow.GetInputWithQuestion("Skriv in kundens efternamn:");
            var email = MainWindow.GetInputWithQuestion("Skriv in kundens email, lämna tomt om saknas:");
            var phoneNumber = MainWindow.GetInputWithQuestion("Skriv in kundens telefonnummer, lämna tomt om saknas:");

            var customer = new Customer(firstName, lastName, email, phoneNumber);

            DbManager.Create(customer);

            MainWindow.SystemMessage($"Ny kund skapad {customer}");
        }

        public override void ReadAll()
        {
            MainWindow.SystemMessage("Hämtar alla kunder:");

            var list = DbManager.Read(null);

            foreach (var customer in list)
            {
                MainWindow.Add(customer.Print());
            }
        }

        public override void Update()
        {
            MainWindow.SystemMessage("Ändra en kund");

            var customer = Find();

            if (customer != null)
            {
                MainWindow.SystemMessage("Hittat kund:");
                MainWindow.Add(customer.Print());

                var newcustomer = ChangeCustomer(customer);

                DbManager.Update(newcustomer);


                MainWindow.SystemMessage("Ändrat värden:");

                if (newcustomer.FirstName != null)
                    MainWindow.SystemMessage($"Förnamn: {customer.FirstName} {newcustomer.FirstName}");
                if (newcustomer.LastName != null)
                    MainWindow.SystemMessage($"Efternamn: {customer.LastName} {newcustomer.LastName}");
                if (newcustomer.Email != null)
                    MainWindow.SystemMessage($"Epost: {customer.Email} {newcustomer.Email}");
                if (newcustomer.PhoneNumber != null)
                    MainWindow.SystemMessage($"Telefonnummer: {customer.PhoneNumber} {newcustomer.PhoneNumber}");
            }
            else
            {
                MainWindow.ErrorMessage("No product found");
            }
        }

        public override void Delete()
        {
            MainWindow.SystemMessage("Tabort en kund");

            var customer = Find();
            DbManager.Delete(customer);

            MainWindow.SystemMessage("Tog bort kund:");
            MainWindow.Add(customer.Print());
        }

        private void AddPhoneNr()
        {
            MainWindow.SystemMessage("Lägg till telefonnummer på befintligt kund");

            var customer = new Customer()
            {
                CustomerId = Find().CustomerId,
                PhoneNumber = MainWindow.GetInputWithQuestion("Skriv in ett telefonnummer:")
            };

            DbManager.Update(customer);
        }

        private void DeletePhoneNr()
        {
            MainWindow.SystemMessage("Tabort ett telefonnummer på en kund");

            var customer = Find();

            if (customer.PhoneNumbers.Count == 1)
            {
                customer = new Customer()
                {
                    CustomerId = customer.CustomerId,
                    PhoneNumber = customer.PhoneNumber
                };
                ((CustomerDbManager)DbManager).DeletePhoneNr(customer);
            }
        }

        public override ICustomer Find()
        {
            var customer = Fill("Vad vill du söka på, välj sök när du är klar", "Sök");

            var list = DbManager.Read(customer);

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
                    var input = MainWindow.GetInputWithQuestion(new Question("Vilken kund vill du välja", temp_List.ToArray()));

                    customer = list.Find((item) => item.ToString() == input);
                    return customer;
                }
                catch (FormatException e)
                {
                    MainWindow.ErrorMessage("Not a valid input");
                }
            }
        }

        private ICustomer Fill(string question, string exit)
        {
            Customer customer = new Customer();

            Question quest = new Question(question, "Förnamn", "Efternamn", "Email", "Telefonnummer", exit);

            var input = "";
            do
            {
                input = MainWindow.GetInputWithQuestion(quest);
                if (input == "Förnamn")
                {
                    customer.FirstName = MainWindow.GetInputWithQuestion("Skriv in ett namn:");
                }
                else if (input == "Efternamn")
                {
                    customer.LastName = MainWindow.GetInputWithQuestion("Skriv in ett efternamn:");
                }
                else if (input == "Email")
                {
                    customer.Email = MainWindow.GetInputWithQuestion("Skriv in en epost:");
                }
                else if (input == "Telefonnummer")
                {
                    customer.AddPhoneNumber = MainWindow.GetInputWithQuestion("Skriv in ett telefonnummer:");
                }
            } while (input != exit);

            return customer;
        }

        private ICustomer ChangeCustomer(ICustomer customer)
        {
            MainWindow.Add(customer.Print());

            var ret = Fill("Vad vill du ändra, spara ändringarna med Ändra", "Ändra");
            ret.CustomerId = customer.CustomerId;
            return ret;
        }
    }
}