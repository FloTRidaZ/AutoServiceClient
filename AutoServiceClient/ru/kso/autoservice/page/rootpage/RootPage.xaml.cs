using AutoServiceClient.ru.kso.autoservice.constants;
using AutoServiceClient.ru.kso.autoservice.database.collection;
using AutoServiceClient.ru.kso.autoservice.database.datatype;
using AutoServiceClient.ru.kso.autoservice.page.servicelistpage;
using AutoServiceClient.ru.kso.autoservice.sort;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AutoServiceClient.ru.kso.autoservice.page.rootpage
{
    public sealed partial class RootPage : Page
    {
        private readonly List<(ComboBoxItem comboBoxItem, AServiceSorting sorting)> _comboSortItems;
        private readonly List<(ComboBoxItem comboBoxItem, AServiceFilter filter)> _comboFilterItems;
        private readonly ServiceCollection _serviceCollection;
        private readonly ResourceLoader _resourceLoader;
        private bool _isReverseSort;

        public RootPage()
        {
            this.InitializeComponent();
            _resourceLoader = ResourceLoader.GetForCurrentView();
            _comboSortItems = new List<(ComboBoxItem, AServiceSorting)>();
            _comboFilterItems = new List<(ComboBoxItem comboBoxItem, AServiceFilter filter)>();
            _serviceCollection = ServiceCollection.GetInstance();
            _serviceCollection.FetchNextPage();
            _isReverseSort = false;
            CreateComboBoxItems();
            SetBottomPanelData();
        }

        private void SetBottomPanelData()
        {
            SetPageData();
            SetCurrentPageTuple();
            SetReviewData();
            SetTuplesData();
        }

        private void SetTuplesData()
        {
            string tuplesTextData = string.Format(_resourceLoader.GetString(ResourceKey.TUPLES_KEY), _serviceCollection.Size);
            _tuplesTextBlock.Text = tuplesTextData;
        }

        private void SetReviewData()
        {
            string reviewTextData = string.Format(_resourceLoader.GetString(ResourceKey.REVIEW_TUPLES_KEY), _serviceCollection.ShowedServices);
            _reviewTextBlock.Text = reviewTextData;
        }

        private void SetPageData()
        {
            string pageTextData = string.Format(_resourceLoader.GetString(ResourceKey.CURRENT_PAGE_KEY), _serviceCollection.CurrentPage, _serviceCollection.PageCount);
            _pageTextBlock.Text = pageTextData;
        }

        private void SetCurrentPageTuple()
        {
            string currentPageTupleTextData = string.Format(_resourceLoader.GetString(ResourceKey.CURRENT_TUPLES_KEY), _serviceCollection.FilterableServices.Count, _serviceCollection.RowServices.Count);
            _currentPageTuplesTextBlock.Text = currentPageTupleTextData;
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
            string searchData = _searchLine.Text;
            Sort(_sortingComboBox, _comboSortItems);
            Filter(_filterComboBox, _comboFilterItems);
            Search(searchData);
            SetBottomPanelData();
        }

        private void Search(string searchData)
        {
            searchData = searchData.ToLower();
            ObservableCollection<Service> filterableCollection = _serviceCollection.FilterableServices;
            ObservableCollection<Service> tempCollection = new ObservableCollection<Service>(filterableCollection);
            filterableCollection.Clear();
            foreach (Service service in tempCollection)
            {
                if (service.Title.ToLower().Contains(searchData))
                {
                    filterableCollection.Add(service);
                }
            }
            SetCurrentPageTuple();
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
            Filter(_filterComboBox, _comboFilterItems);
            string searchData = _searchLine.Text;
            if (!(searchData == null || searchData == ""))
            {
                Search(searchData);
            }
            SetCurrentPageTuple();
        }

        private void Filter(ComboBox comboBox, List<(ComboBoxItem comboBoxItem, AServiceFilter filter)> comboItems)
        {
            if(!(comboBox.SelectedItem is ComboBoxItem current))
            {
                return;
            }
            AServiceFilter filter = comboItems.Find(item => item.comboBoxItem.Equals(current)).filter;
            if (filter == null)
            {
                return;
            }
            filter.Filter();
        }

        private void Sort(ComboBox comboBox, List<(ComboBoxItem comboBoxItem, AServiceSorting sorting)> comboItems)
        {
            if (!(comboBox.SelectedItem is ComboBoxItem current))
            {
                return;
            }
            AServiceSorting sorting = comboItems.Find(item => item.comboBoxItem.Equals(current)).sorting;
            if (sorting == null)
            {
                return;
            }
            if (_isReverseSort)
            {
                sorting.Reverse();
                return;
            }
            sorting.Sort();
        }

        private void BtnPreviousPageClick(object sender, RoutedEventArgs e)
        {
            _serviceCollection.FetchPreviousPage();
            Sort(_sortingComboBox, _comboSortItems);
            Filter(_filterComboBox, _comboFilterItems);
            string searchData = _searchLine.Text;
            if (!(searchData == null || searchData == ""))
            {
                Search(searchData);
            }
            SetPageData();
            SetCurrentPageTuple();
            SetReviewData();
        }

        private void BtnNextPageClick(object sender, RoutedEventArgs e)
        {
            _serviceCollection.FetchNextPage();
            Sort(_sortingComboBox, _comboSortItems);
            Filter(_filterComboBox, _comboFilterItems);
            string searchData = _searchLine.Text;
            if (!(searchData == null || searchData == ""))
            {
                Search(searchData);
            }
            SetPageData();
            SetCurrentPageTuple();
            SetReviewData();
        }

        private void ContentFrameLoaded(object sender, RoutedEventArgs e)
        {
            _contentFrame.Navigate(typeof(ServiceListPage));
        }

        private void BtnReverseClick(object sender, RoutedEventArgs e)
        {
            _isReverseSort = !_isReverseSort;
            Sort(_sortingComboBox, _comboSortItems);
            Filter(_filterComboBox, _comboFilterItems);
            string searchData = _searchLine.Text;
            if (!(searchData == null || searchData == ""))
            {
                Search(searchData);
            }
        }
    }
}
