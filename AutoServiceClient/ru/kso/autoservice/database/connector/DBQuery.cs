namespace AutoServiceClient.ru.kso.autoservice.database.connector
{
    public sealed class DBQuery
    {
        public const string GET_SERVICES_NEXT_QUERY = "EXEC select_from_service_next {0}, {1}";
        public const string GET_SERVICES_PREVIOUS_QUERY = "EXEC select_from_service_previous {0}, {1}";
        public const string GET_SERVICES_COUNT_QUERY = "EXEC select_service_count";
        public const string GET_CLIENTS_QUERY = "";

        private DBQuery()
        {

        }
    }
}
