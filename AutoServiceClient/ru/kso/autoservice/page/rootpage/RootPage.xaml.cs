using AutoServiceClient.ru.kso.autoservice.page.managerpage;
using AutoServiceClient.ru.kso.autoservice.page.orderpage;
using AutoServiceClient.ru.kso.autoservice.page.servicelistpage;
using AutoServiceClient.ru.kso.autoservice.sort;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace AutoServiceClient.ru.kso.autoservice.page.rootpage
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class RootPage : Page
    {
        private readonly List<(ComboBoxItem comboBoxItem, IServiceSorting sorting)> _comboSortItems;
        private readonly List<(ComboBoxItem comboBoxItem, Page page)> _comboUserTypeItems;
        private readonly List<(ComboBoxItem comboBoxItem, IServiceSorting sorting)> _comboFilterItems;

        public RootPage()
        {
            this.InitializeComponent();
            _comboSortItems = new List<(ComboBoxItem, IServiceSorting)>();
            _comboUserTypeItems = new List<(ComboBoxItem comboBoxItem, Page page)>();
            _comboFilterItems = new List<(ComboBoxItem comboBoxItem, IServiceSorting sorting)>();
            CreateComboBoxItems();
        }

        private void CreateComboBoxItems()
        {
            CreateSortItems();
            CreateUserItems();
            CreateFilterItems();
        }

        private void CreateFilterItems()
        {
            ComboBoxItem itemAllFilter = new ComboBoxItem
            {
                Content = "Все"
            };
            ComboBoxItem itemLowFilter = new ComboBoxItem
            {
                Content = "От 0 до 5%"
            };
            ComboBoxItem itemSmallFilter = new ComboBoxItem
            {
                Content = "От 5% до 15%"
            };
            ComboBoxItem itemMediumFilter = new ComboBoxItem
            {
                Content = "От 15% до 30%"
            };
            ComboBoxItem itemHightFilter = new ComboBoxItem
            {
                Content = "От 30% до 70%"
            };
            ComboBoxItem itemFullyFilter = new ComboBoxItem
            {
                Content = "От 70% до 100%"
            };
            _comboFilterItems.Add((itemAllFilter, new RowFilter()));
            _comboFilterItems.Add((itemLowFilter, new LowDiscountFilter()));
            _comboFilterItems.Add((itemSmallFilter, new SmallDiscountFilter()));
            _comboFilterItems.Add((itemMediumFilter, new MediumDiscountFilter()));
            _comboFilterItems.Add((itemHightFilter, new HightDiscountFilter()));
            _comboFilterItems.Add((itemFullyFilter, new FullyDiscountFilter()));
        }

        private void CreateSortItems()
        {
            ComboBoxItem itemPrice = new ComboBoxItem
            {
                Content = "По цене",
            };
            _comboSortItems.Add((itemPrice, new ServicePriceSorting()));
        }

        private void CreateUserItems()
        {
            List<ComboBoxItem> items = new List<ComboBoxItem>();
            ComboBoxItem itemClient = new ComboBoxItem
            {
                Content = "Клиент"
            };
            ComboBoxItem itemManager = new ComboBoxItem
            {
                Content = "Управляющий"
            };
            _comboUserTypeItems.Add((itemClient, new ServiceListPage()));
            _comboUserTypeItems.Add((itemManager, new ManagerPage()));
        }

        private void ContentFrameLoaded(object sender, RoutedEventArgs e)
        {
            _contentFrame.Content = new ServiceListPage();
        }

        private void SortingComboBoxLoaded(object sender, RoutedEventArgs e)
        {
            List<ComboBoxItem> items = new List<ComboBoxItem>();
            foreach ((ComboBoxItem comboBoxItem, _) in _comboSortItems)
            {
                items.Add(comboBoxItem);
            }
            _sortingComboBox.ItemsSource = items;
            _sortingComboBox.SelectedItem = items[0];
        }

        private void SortingComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Sort(_sortingComboBox, _comboSortItems);
        }

        private void UserTypeComboBoxLoaded(object sender, RoutedEventArgs e)
        {
            List<ComboBoxItem> items = new List<ComboBoxItem>();
            foreach ((ComboBoxItem comboBoxItem, _) in _comboUserTypeItems)
            {
                items.Add(comboBoxItem);
            }
            _userTypeComboBox.ItemsSource = items;
            _userTypeComboBox.SelectedItem = items[0];
        }

        private void UserTypeComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem current = _userTypeComboBox.SelectedItem as ComboBoxItem;
            Page page = _comboUserTypeItems.Find(item => item.comboBoxItem.Equals(current)).page;
            if (page == null)
            {
                return;
            }
            _contentFrame.Content = page;
        }

        private void SearchLineTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {

        }

        private void FilterComboBoxLoaded(object sender, RoutedEventArgs e)
        {
            List<ComboBoxItem> items = new List<ComboBoxItem>();
            foreach ((ComboBoxItem comboBoxItem, _) in _comboFilterItems)
            {
                items.Add(comboBoxItem);
            }
            _filterComboBox.ItemsSource = items;
            _filterComboBox.SelectedItem = items[0];
        }

        private void FilterComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Sort(_filterComboBox, _comboFilterItems);
        }

        private void Sort(ComboBox comboBox, List<(ComboBoxItem comboBoxItem, IServiceSorting sorting)> comboItems)
        {
            ComboBoxItem current = comboBox.SelectedItem as ComboBoxItem;
            IServiceSorting sorting = comboItems.Find(item => item.comboBoxItem.Equals(current)).sorting;
            if (sorting == null)
            {
                return;
            }
            sorting.Sort(null);
        }

        private void BtnNextPageClick(object sender, RoutedEventArgs e)
        {

        }

        private void BtnInverseClick(object sender, RoutedEventArgs e)
        {
            ComboBoxItem current = _sortingComboBox.SelectedItem as ComboBoxItem;
            IServiceSorting sorting = _comboSortItems.Find(item => item.comboBoxItem.Equals(current)).sorting;
            if (sorting == null)
            {
                return;
            }
            sorting.Reverse(null);
        }

        private void BtnPreviousPageClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
