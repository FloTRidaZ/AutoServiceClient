using AutoServiceClient.ru.kso.autoservice.database.collection;

namespace AutoServiceClient.ru.kso.autoservice.sort
{
    public abstract class AServiceSorting
    {
        protected readonly ServiceCollection serviceCollection;

        public AServiceSorting()
        {
            serviceCollection = ServiceCollection.GetInstance();
        }

        public abstract void Sort();

        public abstract void Reverse();
    }
}
