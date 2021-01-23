using AutoServiceClient.ru.kso.autoservice.constants;
using AutoServiceClient.ru.kso.autoservice.database.collection;
using AutoServiceClient.ru.kso.autoservice.database.datatype;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.Resources;
using Windows.UI.Text;
using Windows.UI.Xaml.Controls;

namespace AutoServiceClient.ru.kso.autoservice.page.servicelistpage
{
    public sealed partial class ServiceListPage : Page
    {
        private readonly ObservableCollection<Service> _services;
        private readonly ServiceCollection _serviceCollection;
        private readonly ResourceLoader _resourceLoader;

        public ServiceListPage()
        {
            this.InitializeComponent();
            _serviceCollection = ServiceCollection.GetInstance();
            _services = _serviceCollection.FilterableServices;
            _resourceLoader = ResourceLoader.GetForCurrentView();
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
            RelativePanel costPanel = root.Children[2] as RelativePanel;
            TextBlock costTextBlock = costPanel.Children[0] as TextBlock;
            costTextBlock.TextDecorations = TextDecorations.None;
            Service service = args.Item as Service;
            costTextBlock.Text = string.Format(_resourceLoader.GetString(ResourceKey.PRICE_KEY), service.Cost);
            costPanel.Opacity = 1;
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
            RelativePanel costPanel = root.Children[2] as RelativePanel;
            TextBlock discountTextBlock = root.Children[4] as TextBlock;
            discountTextBlock.Opacity = 1;
            Service service = args.Item as Service;
            TextBlock newCostTextBlock = costPanel.Children[1] as TextBlock;
            TextBlock costTextBlock = costPanel.Children[0] as TextBlock;
            if (service.Discount == 0)
            {
                discountTextBlock.Text = _resourceLoader.GetString(ResourceKey.NULL_DISCOUNT_KEY);
                costTextBlock.TextDecorations = TextDecorations.None;
                newCostTextBlock.Text = "";
                return;
            }
            discountTextBlock.Text = string.Format(_resourceLoader.GetString(ResourceKey.DISCOUNT_KEY), service.Discount * 100);
            newCostTextBlock.Text = string.Format(_resourceLoader.GetString(ResourceKey.DISCOUNT_PRICE_KEY), service.DiscountCost);
            costTextBlock.TextDecorations = TextDecorations.Strikethrough;
        }
    }
}
