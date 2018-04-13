using System;
using System.Linq;

namespace SQL_CRM
{
    public class ProductGui : AdministrateGui<IProduct>
    {

        public ProductGui(ConsoleWindowFrame mainWindow)
            : base(mainWindow,
                new ProductDbManager(System.Configuration.ConfigurationManager.ConnectionStrings["Kundregister"].ConnectionString))
        {
        }

        public override void Administrate()
        {
            var mainQuestion = new Question("Skriv in ett val", "Skapa en produkt,Skapa", "Visa alla produkter,Visa", "Ändra en produkt,Ändra", "Tabort en produkt,Tabort", "Tillbaka");

            var input = MainWindow.GetInputWithQuestion(mainQuestion);

            if (input == "Skapa en produkt")
            {
                Create();
            }
            else if (input == "Visa alla produkter")
            {
                ReadAll();
            }
            else if (input == "Ändra en produkt")
            {
                Update();
            }
            else if (input == "Tabort en produkt")
            {
                Delete();
            }
        }

        public override void Create()
        {
            MainWindow.SystemMessage("Skapa en produkt!");

            var name = MainWindow.GetInputWithQuestion("Skriv in produktens namn:");

            var product = new Product()
            {
                Name = name
            };

            DbManager.Create(product);

            MainWindow.SystemMessage($"Ny produkt skapad {product}");
        }

        public override void ReadAll()
        {
            MainWindow.SystemMessage("Hämtar alla produkter:");

            var list = DbManager.Read(null);

            foreach (var product in list)
            {
                Program.Print(product);
            }
        }

        public override void Update()
        {
            MainWindow.SystemMessage("Ändra en produkt");

            var product = Find();

            if (product != null)
            {
                MainWindow.SystemMessage("Hittat kund:");
                Program.Print(product);

                var newproduct = new Product()
                {
                    Id = product.Id,
                    Name = MainWindow.GetInputWithQuestion("Vilket namn har produkten")
                };

                DbManager.Update(product);


                MainWindow.SystemMessage("Ändrat värden:");
                MainWindow.SystemMessage($"Namn: {product.Name} -> {newproduct.Name}");
            }
            else
            {
                MainWindow.ErrorMessage("No product found");
            }
        }

        public override void Delete()
        {
            MainWindow.SystemMessage("Tabort en produkt");

            var product = Find();
            DbManager.Delete(product);

            MainWindow.SystemMessage("Tog bort produkt:");
            Program.Print(product);
        }

        public override IProduct Find()
        {
            var productname = MainWindow.GetInputWithQuestion("Vilken produkt söker du");

            var list = DbManager.Read(new Product()
            {
                Name = productname
            });

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
                    var input = MainWindow.GetInputWithQuestion(new Question("Vilken rodukt vill du välja", temp_List.ToArray()));

                    var product = list.Find((item) => item.ToString() == input);
                    return product;
                }
                catch (FormatException e)
                {
                    MainWindow.ErrorMessage("Not a valid input");
                }
            }
        }
        
        public void ReadAllCustomersThatLikesProduct()
        {
            MainWindow.SystemMessage("Hämta en produkt");

            var product = Find();

            MainWindow.SystemMessage($"Hämtar kunder som gillar {product.Name}...");

            var customers = ((ProductDbManager)DbManager).GetAllCustomer(product);

            MainWindow.SystemMessage($"Produktkunderna gillar:");

            Program.Print(product);

            MainWindow.AddSeparator();

            if (customers.Count == 0)
            {
                MainWindow.SystemMessage("Inga kunder hittades");
            }
            else
            {
                foreach (var customer in customers)
                {
                    Program.Print(customer);
                }
            }
        }
    }
}