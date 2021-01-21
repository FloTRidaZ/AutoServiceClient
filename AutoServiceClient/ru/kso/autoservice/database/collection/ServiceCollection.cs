using AutoServiceClient.ru.kso.autoservice.database.datatype;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoServiceClient.ru.kso.autoservice.database.collection
{
    public sealed class ServiceCollection
    {
        private static ServiceCollection _instance;
        public static int start = 1;
        public const int COUNT = 20;
        public ObservableCollection<Service> Services { get; private set; }
        public int Size { get; set; }

        private ServiceCollection()
        {
            Services = new ObservableCollection<Service>();
        }

        public void Add(Service service)
        {
            Services.Add(service);
        }

        public static void CreateInstance()
        {
            if (_instance != null)
            {
                return;
            }
            _instance = new ServiceCollection();
        }

        public static ServiceCollection GetInstance()
        {
            return _instance;
        }
    }
}
