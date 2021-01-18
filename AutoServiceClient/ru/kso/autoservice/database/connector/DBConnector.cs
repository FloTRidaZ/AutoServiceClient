using AutoServiceClient.ru.kso.autoservice.database.datatype;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoServiceClient.ru.kso.autoservice.database.connector
{
    public sealed class DBConnector
    {
        private DBConnector()
        {

        }

        public static T GetServices<T>(T collection) where T : ICollection<Service>
        {
            using (SqlConnection connection = new SqlConnection(DBConnection.CONNECTION))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = DBQuery.GET_SERVICES_QUERY;
                using(SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Service service = new Service();
                        collection.Add(service);
                    }
                }
            }
            return collection;
        }
    }
}
