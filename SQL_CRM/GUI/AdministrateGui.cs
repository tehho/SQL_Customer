namespace SQL_CRM
{
    public abstract class AdministrateGui<T>
    {
        protected readonly ICrud<T> _dbManager;
        protected readonly ConsoleWindowFrame _mainWindow;

        protected AdministrateGui(ConsoleWindowFrame mainWindow, ICrud<T> dbManager)
        {
            _mainWindow = mainWindow;
            _dbManager = dbManager;
        }

        public abstract void Administrate();
        public abstract void Create();
        public abstract void ReadAll();
        public abstract void Update();
        public abstract void Delete();
        public abstract T Find();}
}