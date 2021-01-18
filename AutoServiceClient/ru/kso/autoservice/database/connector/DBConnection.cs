namespace AutoServiceClient.ru.kso.autoservice.database.connector
{
    public sealed class DBConnection
    {
        public const string DB = "AutoServiceDB";
        public const string USER = "sa";
        public const string PASSWORD = "flotridaz58rus";
        public const string SERVER = "DESKTOP-HBEEL2G\\SQLEXPRESS";
        public const string CONNECTION = @"Data Source = " + SERVER + "; Initial Catalog = " + DB + "; User ID = " + USER + "; Password = " + PASSWORD;

        private DBConnection()
        {

        }
    }
}
