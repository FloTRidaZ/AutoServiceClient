using AutoServiceClient.ru.kso.autoservice.constants;
using AutoServiceClient.ru.kso.autoservice.database.collection;
using AutoServiceClient.ru.kso.autoservice.page.managerpage;
using AutoServiceClient.ru.kso.autoservice.page.servicelistpage;
using AutoServiceClient.ru.kso.autoservice.sort;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace AutoServiceClient.ru.kso.autoservice.page.rootpage
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class RootPage : Page
    {
        private readonly List<(ComboBoxItem comboBoxItem, IServiceSorting sorting)> _comboSortItems;
        private readonly List<(ComboBoxItem comboBoxItem, IServiceSorting sorting)> _comboFilterItems;
        private readonly ServiceCollection _serviceCollection;
        private readonly double _pageCount;
        private readonly ResourceLoader _resourceLoader;

        public RootPage()
        {
            this.InitializeComponent();
            _resourceLoader = ResourceLoader.GetForCurrentView();
            _comboSortItems = new List<(ComboBoxItem, IServiceSorting)>();
            _comboFilterItems = new List<(ComboBoxItem comboBoxItem, IServiceSorting sorting)>();
            _serviceCollection = ServiceCollection.GetInstance();
            _serviceCollection.FetchNextPage();
            CreateComboBoxItems();
            _pageCount = _serviceCollection.PageCount;
            double mod = _pageCount % 1;
            if (mod != 0)
            {
                _pageCount++;
                _pageCount -= mod;
            }
            string pageTextData = string.Format(_resourceLoader.GetString(ResourceKey.CURRENT_PAGE_KEY), _serviceCollection.CurrentPage, _pageCount);
            _pageTextBlock.Text = pageTextData;
            string tupleTextData = string.Format(_resourceLoader.GetString(ResourceKey.CURRENT_TUPLES_KEY), _serviceCollection.Start - 1, _serviceCollection.Size);
            _tuplesTextBlock.Text = tupleTextData;
        }

        private void CreateComboBoxItems()
        {
            CreateSortItems();
            CreateFilterItems();
        }

        private void CreateFilterItems()
        {
            ComboBoxItem itemAllFilter = new ComboBoxItem
            {
                Content = _resourceLoader.GetString(ResourceKey.ALL_FILTER_KEY)
            };
            ComboBoxItem itemLowFilter = new ComboBoxItem
            {
                Content = _resourceLoader.GetString(ResourceKey.LOW_FILTER_KEY)
            };
            ComboBoxItem itemSmallFilter = new ComboBoxItem
            {
                Content = _resourceLoader.GetString(ResourceKey.SMALL_FILTER_KEY)
            };
            ComboBoxItem itemMediumFilter = new ComboBoxItem
            {
                Content = _resourceLoader.GetString(ResourceKey.MEDIUM_FILTER_KEY)
            };
            ComboBoxItem itemHightFilter = new ComboBoxItem
            {
                Content = _resourceLoader.GetString(ResourceKey.HIGHT_FILTER_KEY)
            };
            ComboBoxItem itemFullyFilter = new ComboBoxItem
            {
                Content = _resourceLoader.GetString(ResourceKey.FULL_FILTER_KEY)
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
                Content = _resourceLoader.GetString(ResourceKey.PRICE_FILTER_KEY)
            };
            _comboSortItems.Add((itemPrice, new ServicePriceSorting()));
        }

        private void SortingComboBoxLoaded(object sender, RoutedEventArgs e)
        {
            List<ComboBoxItem> items = new List<ComboBoxItem>();
            foreach ((ComboBoxItem comboBoxItem, _) in _comboSortItems)
            {
                items.Add(comboBoxItem);
            }
            _sortingComboBox.ItemsSource = items;
        }

        private void SortingComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Sort(_sortingComboBox, _comboSortItems);
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
        }

        private void FilterComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Sort(_filterComboBox, _comboFilterItems);
        }

        private void Sort(ComboBox comboBox, List<(ComboBoxItem comboBoxItem, IServiceSorting sorting)> comboItems)
        {
            if (!(comboBox.SelectedItem is ComboBoxItem current))
            {
                return;
            }
            IServiceSorting sorting = comboItems.Find(item => item.comboBoxItem.Equals(current)).sorting;
            if (sorting == null)
            {
                return;
            }
            sorting.Sort();
        }

        private void Reverse(object sender, RoutedEventArgs e)
        {
            if (!(_sortingComboBox.SelectedItem is ComboBoxItem current))
            {
                return;
            }
            IServiceSorting sorting = _comboSortItems.Find(item => item.comboBoxItem.Equals(current)).sorting;
            if (sorting == null)
            {
                return;
            }
            sorting.Reverse();
            _btnInverse.Click += Resort;
            _btnInverse.Click -= Reverse;
        }

        private void Resort(object sender, RoutedEventArgs e)
        {
            Sort(_sortingComboBox, _comboSortItems);
            _btnInverse.Click += Reverse;
            _btnInverse.Click -= Resort;
        }

        private void BtnPreviousPageClick(object sender, RoutedEventArgs e)
        {
            _serviceCollection.FetchPreviousPage();
            string pageTextData = string.Format(_resourceLoader.GetString(ResourceKey.CURRENT_PAGE_KEY), _serviceCollection.CurrentPage, _pageCount);
            _pageTextBlock.Text = pageTextData;
            string tupleTextData = string.Format(_resourceLoader.GetString(ResourceKey.CURRENT_TUPLES_KEY), _serviceCollection.Start - 1, _serviceCollection.Size);
            _tuplesTextBlock.Text = tupleTextData;
            Sort(_sortingComboBox, _comboSortItems);
        }

        private void BtnNextPageClick(object sender, RoutedEventArgs e)
        {
            _serviceCollection.FetchNextPage();
            string pageTextData = string.Format(_resourceLoader.GetString(ResourceKey.CURRENT_PAGE_KEY), _serviceCollection.CurrentPage, _pageCount);
            _pageTextBlock.Text = pageTextData;
            string tupleTextData = string.Format(_resourceLoader.GetString(ResourceKey.CURRENT_TUPLES_KEY), _serviceCollection.Start - 1, _serviceCollection.Size);
            _tuplesTextBlock.Text = tupleTextData;
            Sort(_sortingComboBox, _comboSortItems);
        }


        private void BtnInverseLoaded(object sender, RoutedEventArgs e)
        {
            _btnInverse.Click += Reverse;
        }

        private void ContentFrameLoaded(object sender, RoutedEventArgs e)
        {
            _contentFrame.Navigate(typeof(ServiceListPage));
        }
    }
}
