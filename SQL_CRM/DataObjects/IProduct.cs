namespace SQL_CRM
{
    public interface IProduct
    {
        string Name { get; set; }
        int? Id { get; set; }

        string ToString();
    }
}