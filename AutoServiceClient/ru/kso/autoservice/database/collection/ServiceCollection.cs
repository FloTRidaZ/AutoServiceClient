using AutoServiceClient.ru.kso.autoservice.database.connector;
using AutoServiceClient.ru.kso.autoservice.database.datatype;
using System.Collections.ObjectModel;

namespace AutoServiceClient.ru.kso.autoservice.database.collection
{
    public sealed class ServiceCollection
    {
        private static ServiceCollection _instance;
        private const int COUNT = 40;
        private ObservableCollection<Service> _filterableServices;
        private ObservableCollection<Service> _rowServices;
        private int _start;
        private int _currentPage;
        private float _pageCount;
        private int _size;
        private int _showedServices;

        public int ShowedServices
        {
            get
            {
                return _showedServices;
            }

            private set
            {
                _showedServices = value;
            }
        }

        public int CurrentPage
        {
            get
            {
                return _currentPage;
            }

            private set
            {
                _currentPage = value;
            }
        }

        public float PageCount
        {
            get
            {
                return _pageCount;
            }
            
            private set
            {
                _pageCount = value;
            }
        }
        public int Size
        {
            get
            {
                return _size;
            }

            private set
            {
                _size = value;
            }
        }
        public ObservableCollection<Service> FilterableServices
        {
            get
            {
                return _filterableServices;
            }

            private set
            {
                _filterableServices = value;
            }
        }
        public ObservableCollection<Service> RowServices
        {
            get
            {
                return _rowServices;
            }

            private set
            {
                _rowServices = value;
            }
        }
        public bool IsLastPage
        {
            get
            {
                return CurrentPage == PageCount;
            }
        }

        public bool IsFirstPage
        {
            get
            {
                return CurrentPage == 1;
            }
        }

        private ServiceCollection()
        {
            FilterableServices = new ObservableCollection<Service>();
            RowServices = new ObservableCollection<Service>();
            _start = 1;
            CurrentPage = 0;
            ShowedServices = 0;
            Size = DBConnector.GetServiceCount();
            PageCount = (float) Size / (float) COUNT;
            float mod = PageCount % 1;
            if (mod != 0)
            {
                PageCount++;
                PageCount -= mod;
            }
        }

        public void FetchNextPage()
        {
            if (Size < _start)
            {
                return;
            }
            if (IsLastPage)
            {
                return;
            }
            CurrentPage++;
            FilterableServices.Clear();
            RowServices.Clear();
            AddNewServices(DBConnector.GetServicesNext(_start, COUNT));
            _start += COUNT;
            ShowedServices += FilterableServices.Count;
        }

        public void FetchPreviousPage()
        {
            if (IsFirstPage)
            {
                return;
            }
            _start--;
            _start -= COUNT;
            CurrentPage--;
            ShowedServices -= FilterableServices.Count;
            FilterableServices.Clear();
            RowServices.Clear();
            AddNewServicesReverse(DBConnector.GetServicesPrevious(_start, COUNT));
            _start++;
        }

        private void AddNewServicesReverse(ObservableCollection<Service> newServices)
        {
            for (int index = newServices.Count - 1; index >= 0; index--)
            {
                FilterableServices.Add(newServices[index]);
                RowServices.Add(newServices[index]);
            }
        }

        private void AddNewServices(ObservableCollection<Service> newServices)
        {
            foreach (Service service in newServices)
            {
                FilterableServices.Add(service);
                RowServices.Add(service);
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
