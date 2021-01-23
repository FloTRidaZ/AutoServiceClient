using AutoServiceClient.ru.kso.autoservice.database.collection;

namespace AutoServiceClient.ru.kso.autoservice.sort
{
    public abstract class AServiceFilter
    {
        protected readonly ServiceCollection serviceCollection;

        public AServiceFilter()
        {
            serviceCollection = ServiceCollection.GetInstance();
        }

        public abstract void Filter();
    }
}
