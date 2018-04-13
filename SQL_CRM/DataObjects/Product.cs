using System;

namespace SQL_CRM.DataObjects
{
    public class Product : IProduct
    {
        public string Name { get; set; }
        public int? Id { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public WebMessage Print()
        {
            return new WebMessage("Produkt", ToString(), ConsoleColor.Yellow);
        }
    }
}