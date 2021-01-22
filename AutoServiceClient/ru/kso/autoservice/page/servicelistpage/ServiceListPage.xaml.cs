using AutoServiceClient.ru.kso.autoservice.constants;
using AutoServiceClient.ru.kso.autoservice.database.collection;
using AutoServiceClient.ru.kso.autoservice.database.datatype;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.Resources;
using Windows.UI.Text;
using Windows.UI.Xaml.Controls;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace AutoServiceClient.ru.kso.autoservice.page.servicelistpage
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class ServiceListPage : Page
    {
        private readonly ObservableCollection<Service> _services;
        private readonly ServiceCollection _serviceCollection;
        private readonly ResourceLoader _resourceLoader;

        public ServiceListPage()
        {
            this.InitializeComponent();
            _serviceCollection = ServiceCollection.GetInstance();
            _services = _serviceCollection.Services;
            _resourceLoader = ResourceLoader.GetForCurrentView();
        }

        private void ServicesGridViewItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void ServicesGridViewContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            if (args.Phase != 0)
            {
                return;
            }
            args.RegisterUpdateCallback(ShowCost);
        }

        private void ShowCost(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            if (args.Phase != 1)
            {
                return;
            }
            StackPanel root = args.ItemContainer.ContentTemplateRoot as StackPanel;
            TextBlock costTextBlock = root.Children[2] as TextBlock;
            Service service = args.Item as Service;
            costTextBlock.Text = string.Format(_resourceLoader.GetString(ResourceKey.PRICE_KEY), service.Cost);
            costTextBlock.Opacity = 1;
            args.RegisterUpdateCallback(ShowDuration);
        }

        private void ShowDuration(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            if (args.Phase != 2)
            {
                return;
            }
            StackPanel root = args.ItemContainer.ContentTemplateRoot as StackPanel;
            TextBlock durationTextBlock = root.Children[3] as TextBlock;
            Service service = args.Item as Service;
            durationTextBlock.Text = string.Format(_resourceLoader.GetString(ResourceKey.DURATION_KEY), service.Duration);
            durationTextBlock.Opacity = 1;
            args.RegisterUpdateCallback(ShowDiscount);
        }

        private void ShowDiscount(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            if (args.Phase != 3)
            {
                return;
            }
            StackPanel root = args.ItemContainer.ContentTemplateRoot as StackPanel;
            TextBlock costTextBlock = root.Children[2] as TextBlock;
            TextBlock discountTextBlock = root.Children[4] as TextBlock;
            Service service = args.Item as Service;
            string discount;
            if (service.Discount == 0)
            {
                discount = _resourceLoader.GetString(ResourceKey.NULL_DISCOUNT_KEY);
            } else
            {
                discount = string.Format(_resourceLoader.GetString(ResourceKey.DISCOUNT_KEY), service.Discount * 100);
            }
            discountTextBlock.Text = discount;
            discountTextBlock.Opacity = 1;
            costTextBlock.TextDecorations = TextDecorations.Strikethrough;
        }
    }
}
