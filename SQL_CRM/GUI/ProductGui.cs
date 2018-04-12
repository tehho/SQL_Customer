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

            var input = _mainWindow.GetInputWithQuestion(mainQuestion);

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
            _mainWindow.SystemMessage("Skapa en produkt!");

            var name = _mainWindow.GetInputWithQuestion("Skriv in produktens namn:");

            var product = new Product()
            {
                Name = name
            };

            _dbManager.Create(product);

            _mainWindow.SystemMessage($"Ny produkt skapad {product}");
        }

        public override void ReadAll()
        {
            _mainWindow.SystemMessage("Hämtar alla produkter:");

            var list = _dbManager.Read(null);

            foreach (var product in list)
            {
                Program.Print(product);
            }
        }

        public override void Update()
        {
            _mainWindow.SystemMessage("Ändra en produkt");

            var product = Find();

            if (product != null)
            {
                _mainWindow.SystemMessage("Hittat kund:");
                Program.Print(product);

                var newproduct = new Product()
                {
                    Id = product.Id,
                    Name = _mainWindow.GetInputWithQuestion("Vilket namn har produkten")
                };

                _dbManager.Update(product);


                _mainWindow.SystemMessage("Ändrat värden:");
                _mainWindow.SystemMessage($"Namn: {product.Name} -> {newproduct.Name}");
            }
            else
            {
                _mainWindow.ErrorMessage("No product found");
            }
        }

        public override void Delete()
        {
            _mainWindow.SystemMessage("Tabort en produkt");

            var product = Find();
            _dbManager.Delete(product);

            _mainWindow.SystemMessage("Tog bort produkt:");
            Program.Print(product);
        }

        public override IProduct Find()
        {
            var productname = _mainWindow.GetInputWithQuestion("Vilken produkt söker du");

            var list = _dbManager.Read(new Product()
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
                    var input = _mainWindow.GetInputWithQuestion(new Question("Vilken rodukt vill du välja", temp_List.ToArray()));

                    var product = list.Find((item) => item.ToString() == input);
                    return product;
                }
                catch (FormatException e)
                {
                    _mainWindow.ErrorMessage("Not a valid input");
                }
            }
        }
        
        public void ReadAllCustomersThatLikesProduct()
        {
            _mainWindow.SystemMessage("Hämta en produkt");

            var product = Find();

            _mainWindow.SystemMessage($"Hämtar kunder som gillar {product.Name}...");

            var customers = ((ProductDbManager)_dbManager).GetAllCustomer(product);

            _mainWindow.SystemMessage($"Produktkunderna gillar:");

            Program.Print(product);

            _mainWindow.AddSeparator();

            if (customers.Count == 0)
            {
                _mainWindow.SystemMessage("Inga kunder hittades");
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