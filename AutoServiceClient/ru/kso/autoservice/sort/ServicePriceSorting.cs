using AutoServiceClient.ru.kso.autoservice.database.datatype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoServiceClient.ru.kso.autoservice.sort
{
    public class ServicePriceSorting : IServiceSorting
    {
        public void Sort(ICollection<Service> collection)
        {
            IEnumerator<Service> enumerator = collection.GetEnumerator();
        }
    }
}
