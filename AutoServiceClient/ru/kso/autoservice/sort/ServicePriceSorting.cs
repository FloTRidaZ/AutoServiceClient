using AutoServiceClient.ru.kso.autoservice.database.datatype;
using System.Collections.ObjectModel;

namespace AutoServiceClient.ru.kso.autoservice.sort
{
    public sealed class ServicePriceSorting : AServiceSorting
    {

        public override void Sort()
        {
            Sort(serviceCollection.RowServices);
            Sort(serviceCollection.FilterableServices);
        }

        private void Sort(ObservableCollection<Service> collection)
        {
            for (int outIndex = collection.Count - 1; outIndex >= 1; outIndex--)
            {
                for (int index = 0; index < outIndex; index++)
                {
                    Service current = collection[index];
                    Service next = collection[index + 1];
                    if (current.DiscountCost > next.DiscountCost)
                    {
                        collection[index] = next;
                        collection[index + 1] = current;
                    }
                }
            }
        }

        public override void Reverse()
        {
            Reverse(serviceCollection.FilterableServices);
            Reverse(serviceCollection.RowServices);
        }

        private static void Reverse(ObservableCollection<Service> collection)
        {
            for (int outIndex = collection.Count - 1; outIndex >= 1; outIndex--)
            {
                for (int index = 0; index < outIndex; index++)
                {
                    Service current = collection[index];
                    Service next = collection[index + 1];
                    if (current.DiscountCost < next.DiscountCost)
                    {
                        collection[index] = next;
                        collection[index + 1] = current;
                    }
                }
            }
        }
    }
}
