using AutoServiceClient.ru.kso.autoservice.constants;
using AutoServiceClient.ru.kso.autoservice.database.collection;
using AutoServiceClient.ru.kso.autoservice.page.servicelistpage;
using AutoServiceClient.ru.kso.autoservice.sort;
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
        private readonly ResourceLoader _resourceLoader;
        private bool _isReverseSort;

        public RootPage()
        {
            this.InitializeComponent();
            _resourceLoader = ResourceLoader.GetForCurrentView();
            _comboSortItems = new List<(ComboBoxItem, IServiceSorting)>();
            _comboFilterItems = new List<(ComboBoxItem comboBoxItem, IServiceSorting sorting)>();
            _serviceCollection = ServiceCollection.GetInstance();
            _serviceCollection.FetchNextPage();
            _isReverseSort = false;
            CreateComboBoxItems();
            SetPageData();
        }

        private void SetPageData()
        {
            double pageCount = _serviceCollection.PageCount;
            string pageTextData = string.Format(_resourceLoader.GetString(ResourceKey.CURRENT_PAGE_KEY), _serviceCollection.CurrentPage, pageCount);
            _pageTextBlock.Text = pageTextData;
            string tupleTextData = string.Format(_resourceLoader.GetString(ResourceKey.CURRENT_TUPLES_KEY), _serviceCollection.ShowedServices, _serviceCollection.Size);
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

        private void BtnPreviousPageClick(object sender, RoutedEventArgs e)
        {
            _serviceCollection.FetchPreviousPage();
            SetPageData();
        }

        private void BtnNextPageClick(object sender, RoutedEventArgs e)
        {
            _serviceCollection.FetchNextPage();
            SetPageData();
        }

        private void ContentFrameLoaded(object sender, RoutedEventArgs e)
        {
            _contentFrame.Navigate(typeof(ServiceListPage));
        }

        private void BtnReverseClick(object sender, RoutedEventArgs e)
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
            if (!_isReverseSort)
            {
                sorting.Reverse();
                _isReverseSort = !_isReverseSort;
                return;
            }
            sorting.Sort();
            _isReverseSort = !_isReverseSort;
        }
    }
}
