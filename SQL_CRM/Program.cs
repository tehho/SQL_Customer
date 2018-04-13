using System;
using SQL_CRM.ConsoleClasses;
using SQL_CRM.CRUD;
using SQL_CRM.DataObjects;
using SQL_CRM.GUI;

namespace SQL_CRM
{
    public class Program
    {
        private static readonly ConsoleWindowFrame MainWindow = new ConsoleWindowFrame();

        private static CustomerDbManager _customerDbManager;
        private static ProductDbManager _productDbManager;

        public static void Main(string[] args)
        {
            MainWindow.Width = 115;
            MainWindow.Height = 25;

            MainWindow.StartRender();

            string connectionString;
            var input = MainWindow.GetInputWithQuestion("Vill du koppla upp mot Azure?").ToLower();
            if (input == "ja" ||
                input == "azure")
                connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Kundregister"]
                    .ConnectionString;
            else
                connectionString = "Server = localdb/mssqllocaldb; Database=KundregisterAndreasOVictor; Trusted Connection = true; UID = CustomerWriter; PWD = 1234test_!";
            

            _customerDbManager =
                new CustomerDbManager(connectionString);

            _productDbManager =
                new ProductDbManager(connectionString);


            var customerGui = new CustomerGui(MainWindow);
            var productGui = new ProductGui(MainWindow);

            var running = true;

            var mainQuestion = new Question("Vad vill du göra", "Administrera en kund,Kund", "Administrera en produkt,Produkt", "Rensa skärmen,CLS", "Avsluta,Quit,Exit");

            while (running)
            {
                try
                {
                    input = MainWindow.GetInputWithQuestion(mainQuestion);

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
                        MainWindow.Clear();
                    }
                    else if (input == "Avsluta")
                    {
                        running = false;
                    }
                    MainWindow.AddSeparator();
                }
                catch (Exception e)
                {
                    MainWindow.ErrorMessage(e.Message);
                }
            }

            MainWindow.PressAnyKeyToContinue();

            MainWindow.Abort();
        }
        
        public static void Print(ICustomer customer)
        {
            MainWindow.Add(new WebMessage("Customer", customer.ToString(), ConsoleColor.Green));
        }

        public static void Print(IProduct product)
        {
            MainWindow.Add(new WebMessage("Product", product.ToString(), ConsoleColor.Yellow));
        }

        public static void ErrorMessage(string message)
        {
            MainWindow.ErrorMessage(message);
        }

    }
}

