using AutoServiceClient.ru.kso.autoservice.database.collection;
using AutoServiceClient.ru.kso.autoservice.database.datatype;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.IO;

namespace AutoServiceClient.ru.kso.autoservice.database.connector
{
    public sealed class DBConnector
    {
        private DBConnector()
        {

        }

        public static ObservableCollection<Service> GetServices()
        {
            ServiceCollection serviceCollection = ServiceCollection.GetInstance();
            using (SqlConnection connection = new SqlConnection(DBConnection.CONNECTION))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = string.Format(DBQuery.GET_SERVICES_QUERY, ServiceCollection.start, ServiceCollection.COUNT);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    AddService(reader, serviceCollection);
                    while (reader.NextResult())
                    {
                        reader.Read();
                        AddService(reader, serviceCollection);
                    }
                }
            }
            return serviceCollection.Services;
        }

        private static void AddService(SqlDataReader reader, ServiceCollection serviceCollection)
        {
            int id = reader.GetInt32(DBAttributes.ID_ATTR);
            string title = reader.GetString(DBAttributes.TITLE_ATTR);
            int cost = reader.GetInt32(DBAttributes.COST_ATTR);
            float duration = reader.GetInt32(DBAttributes.DURATION_IN_SECONDS_ATTR) / 3600;
            Stream fileStream = reader.GetStream(DBAttributes.PHOTO_ATTR); 
            Service service = new Service(id, title, cost, duration);
            service.SetImageFromStream(fileStream);
            serviceCollection.Add(service);
        }
    }
}
