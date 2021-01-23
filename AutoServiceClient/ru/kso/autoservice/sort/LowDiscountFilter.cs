using AutoServiceClient.ru.kso.autoservice.database.collection;
using AutoServiceClient.ru.kso.autoservice.database.datatype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoServiceClient.ru.kso.autoservice.sort
{
    public sealed class LowDiscountFilter : IServiceSorting
    {
        private readonly ServiceCollection _collection;

        public LowDiscountFilter()
        {
            _collection = ServiceCollection.GetInstance();
        }

        public void Reverse()
        {
        }

        public void Sort()
        {

        }
    }
}
