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

        public static int GetServiceCount()
        {
            int count = 0;
            using (SqlConnection connection = new SqlConnection(DBConnection.CONNECTION))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = DBQuery.GET_SERVICES_COUNT_QUERY;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    count = reader.GetInt32(DBAttributes.COUNT_ATTR);
                    
                }
            }
            return count;
        }

        public static ObservableCollection<Service> GetServicesNext(double start, double count)
        {
            ObservableCollection<Service> services = new ObservableCollection<Service>();
            using (SqlConnection connection = new SqlConnection(DBConnection.CONNECTION))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = string.Format(DBQuery.GET_SERVICES_NEXT_QUERY, start, count);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    AddService(reader, services);
                    while (reader.NextResult())
                    {
                        reader.Read();
                        AddService(reader, services);
                    }
                }
            }
            return services;
        }

        public static ObservableCollection<Service> GetServicesPrevious(double start, double count)
        {
            ObservableCollection<Service> services = new ObservableCollection<Service>();
            using (SqlConnection connection = new SqlConnection(DBConnection.CONNECTION))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = string.Format(DBQuery.GET_SERVICES_PREVIOUS_QUERY, start, count);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    AddService(reader, services);
                    while (reader.NextResult())
                    {
                        reader.Read();
                        AddService(reader, services);
                    }
                }
            }
            return services;
        }

        private static void AddService(SqlDataReader reader, ObservableCollection<Service> services)
        {
            int id = reader.GetInt32(DBAttributes.ID_ATTR);
            string title = reader.GetString(DBAttributes.TITLE_ATTR);
            int cost = reader.GetInt32(DBAttributes.COST_ATTR);
            float duration = reader.GetInt32(DBAttributes.DURATION_IN_SECONDS_ATTR) / 3600;
            Stream fileStream = reader.GetStream(DBAttributes.PHOTO_ATTR); 
            Service service = new Service(id, title, cost, duration);
            service.SetImageFromStream(fileStream);
            service.Discount = (float) reader.GetDouble(DBAttributes.DISCOUNT_ATTR);
            services.Add(service);
        }
    }
}
