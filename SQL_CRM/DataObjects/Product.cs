namespace SQL_CRM
{
    public class Product : IProduct
    {
        public string Name { get; set; }
        public int? Id { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}