using AutoServiceClient.ru.kso.autoservice.database.connector;
using AutoServiceClient.ru.kso.autoservice.database.datatype;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace AutoServiceClient.ru.kso.autoservice.page.servicelistpage
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class ServiceListPage : Page
    {
        private readonly ObservableCollection<Service> _services;

        public ServiceListPage()
        {
            this.InitializeComponent();
            _services = DBConnector.GetServices();
        }

        private void ServicesGridViewItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void CostTextBlockLoaded(object sender, RoutedEventArgs e)
        {
            TextBlock cost = sender as TextBlock;
            cost.Text = string.Format("Цена: {0} руб", cost.Text);
        }

        private void DurationTextBlockLoaded(object sender, RoutedEventArgs e)
        {
            TextBlock duration = sender as TextBlock;
            duration.Text = string.Format("Продолжительность: {0} ч.", duration.Text);
        }
    }
}
