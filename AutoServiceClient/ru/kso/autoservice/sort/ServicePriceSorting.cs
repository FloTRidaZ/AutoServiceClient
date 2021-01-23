using AutoServiceClient.ru.kso.autoservice.database.collection;
using AutoServiceClient.ru.kso.autoservice.database.datatype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoServiceClient.ru.kso.autoservice.sort
{
    public sealed class ServicePriceSorting : IServiceSorting
    {
        private readonly ServiceCollection _collection;

        public ServicePriceSorting()
        {
            _collection = ServiceCollection.GetInstance();
        }

        public void Sort()
        {
            for (int outIndex = _collection.FilterableServices.Count - 1; outIndex >= 1; outIndex--)
            {
                for (int index = 0; index < outIndex; index++)
                {
                    Service current = _collection.FilterableServices[index];
                    Service next = _collection.FilterableServices[index + 1];
                    if (current.DiscountCost > next.DiscountCost)
                    {
                        _collection.FilterableServices[index] = next;
                        _collection.FilterableServices[index + 1] = current;
                    }
                }
            }
        }

        public void Reverse()
        {
            for (int outIndex = _collection.FilterableServices.Count - 1; outIndex >= 1; outIndex--)
            {
                for (int index = 0; index < outIndex; index++)
                {
                    Service current = _collection.FilterableServices[index];
                    Service next = _collection.FilterableServices[index + 1];
                    if (current.DiscountCost < next.DiscountCost)
                    {
                        _collection.FilterableServices[index] = next;
                        _collection.FilterableServices[index + 1] = current;
                    }
                }
            }
        }

    }
}
