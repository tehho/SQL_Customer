using SQL_CRM.ConsoleClasses;

namespace SQL_CRM.DataObjects
{
    public interface IProduct : IPrintable
    {
        string Name { get; set; }
        int? Id { get; set; }

        string ToString();
    }
}