using AutoServiceClient.ru.kso.autoservice.database.connector;
using AutoServiceClient.ru.kso.autoservice.database.datatype;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AutoServiceClient.ru.kso.autoservice.database.collection
{
    public sealed class ServiceCollection
    {
        private static ServiceCollection _instance;
        private const double COUNT = 40;
        public double Start { get; private set; }
        public double CurrentPage { get; private set; }
        public double PageCount { get; private set; }
        public double Size { get; private set; }
        public ObservableCollection<Service> Services { get; set; }

        private ServiceCollection()
        {
            Services = new ObservableCollection<Service>();
            Start = 1;
            CurrentPage = 0;
            Size = DBConnector.GetServiceCount();
            PageCount = Size / COUNT;
        }

        public void Add(Service service)
        {
            Services.Add(service);
        }

        public void FetchNextPage()
        {
            if (Size < Start)
            {
                return;
            }
            double currentCount = COUNT;
            CurrentPage++;
            if (CurrentPage > PageCount)
            {
                currentCount = COUNT * (PageCount % 1);
            }
            Services.Clear();
            AddNewServices(DBConnector.GetServicesNext(Start, currentCount));
            Start += currentCount;
        }

        public void FetchPreviousPage()
        {
            if (CurrentPage == 1)
            {
                return;
            }
            Start--;
            if (CurrentPage > PageCount)
            {
                Start -= COUNT * (PageCount % 1);
            } else
            {
                Start -= COUNT;
            }
            CurrentPage--;
            Services.Clear();
            AddNewServicesReverse(DBConnector.GetServicesPrevious(Start, COUNT));
            Start++;
        }

        private void AddNewServicesReverse(ObservableCollection<Service> newServices)
        {
            for (int index = newServices.Count - 1; index >= 0; index--)
            {
                Services.Add(newServices[index]);
            }
        }

        private void AddNewServices(ObservableCollection<Service> newServices)
        {
            foreach (Service service in newServices)
            {
                Services.Add(service);
            }
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
