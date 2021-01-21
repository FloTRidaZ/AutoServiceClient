namespace AutoServiceClient.ru.kso.autoservice.database.connector
{
    public sealed class DBQuery
    {
        public const string GET_SERVICES_QUERY = "EXEC select_from_service {0}, {1}";
        public const string GET_CLIENTS_QUERY = "";

        private DBQuery()
        {

        }
    }
}
