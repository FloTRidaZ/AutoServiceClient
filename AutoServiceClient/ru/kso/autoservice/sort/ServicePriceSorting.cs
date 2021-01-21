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
        ServiceCollection _collection;

        public ServicePriceSorting()
        {
            _collection = ServiceCollection.GetInstance();
        }

        public void Sort()
        {
            for (int outIndex = _collection.Services.Count - 1; outIndex >= 1; outIndex--)
            {
                for (int index = 0; index < outIndex; index++)
                {
                    Service current = _collection.Services[index];
                    Service next = _collection.Services[index + 1];
                    if (current.Cost > next.Cost)
                    {
                        _collection.Services[index] = next;
                        _collection.Services[index + 1] = current;
                    }
                }
            }
        }

        public void Reverse()
        {
            
        }

    }
}
