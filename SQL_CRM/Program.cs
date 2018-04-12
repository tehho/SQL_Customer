using System;
using System.Deployment.Application;
using System.Linq;

namespace SQL_CRM
{
    public class Program
    {
        static readonly ConsoleWindowFrame _mainWindow = new ConsoleWindowFrame();

        private static CustomerDbManager _customerDbManager;
        private static ProductDbManager _productDbManager;

        public static void Main(string[] args)
        {
            _customerDbManager =
                new CustomerDbManager(System.Configuration.ConfigurationManager.ConnectionStrings["Kundregister"].ConnectionString);

            _productDbManager = 
                new ProductDbManager(System.Configuration.ConfigurationManager.ConnectionStrings["Kundregister"].ConnectionString);


            _mainWindow.Width = 115;
            _mainWindow.Height = 25;

            _mainWindow.StartRender();

            var customerGui = new CustomerGui(_mainWindow);
            var productGui = new ProductGui(_mainWindow);

            var running = true;

            var mainQuestion = new Question("Vad vill du göra", "Administrera en kund,Kund", "Administrera en produkt,Produkt", "Rensa skärmen,CLS", "Avsluta,Quit,Exit");

            while (running)
            {
                try
                {
                    var input = _mainWindow.GetInputWithQuestion(mainQuestion);

                    if (input == "Administrera en kund")
                    {
                        customerGui.Administrate();
                    }
                    else if (input == "Administrera en produkt")
                    {
                        productGui.Administrate();
                    }
                    else if (input == "Rensa skärmen")
                    {
                        _mainWindow.Clear();
                    }
                    else if (input == "Avsluta")
                    {
                        running = false;
                    }
                    _mainWindow.AddSeparator();
                }
                catch (Exception e)
                {
                    _mainWindow.ErrorMessage(e.Message);
                }
            }

            _mainWindow.PressAnyKeyToContinue();

            _mainWindow.Abort();
        }
        
        public static void Print(ICustomer customer)
        {
            _mainWindow.Add(new WebMessage("Customer", customer.ToString(), ConsoleColor.Green));
        }

        public static void Print(IProduct product)
        {
            _mainWindow.Add(new WebMessage("Product", product.ToString(), ConsoleColor.Yellow));
        }

        public static void ErrorMessage(string message)
        {
            _mainWindow.ErrorMessage(message);
        }

    }
}

