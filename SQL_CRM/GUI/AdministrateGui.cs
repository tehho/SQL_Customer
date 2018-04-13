namespace SQL_CRM
{
    public abstract class AdministrateGui<T>
    {
        protected readonly ICrud<T> DbManager;
        protected readonly ConsoleWindowFrame MainWindow;
        
        protected AdministrateGui(ConsoleWindowFrame mainWindow, ICrud<T> dbManager)
        {
            MainWindow = mainWindow;
            DbManager = dbManager;
        }

        public abstract void Administrate();
        public abstract void Create();
        public abstract void ReadAll();
        public abstract void Update();
        public abstract void Delete();
        public abstract T Find();}
}