using AutoServiceClient.ru.kso.autoservice.database.datatype;
using System.Collections.ObjectModel;

namespace AutoServiceClient.ru.kso.autoservice.sort
{
    public sealed class RowFilter : AServiceFilter
    {
        public override void Filter()
        {
            ObservableCollection<Service> filterableCollection = serviceCollection.FilterableServices;
            ObservableCollection<Service> rowCollection = serviceCollection.RowServices;
            filterableCollection.Clear();
            foreach (Service service in rowCollection)
            {
                filterableCollection.Add(service);
            }
        }
    }
}
