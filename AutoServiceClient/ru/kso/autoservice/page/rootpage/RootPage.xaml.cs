using AutoServiceClient.ru.kso.autoservice.database.collection;
using AutoServiceClient.ru.kso.autoservice.page.managerpage;
using AutoServiceClient.ru.kso.autoservice.page.servicelistpage;
using AutoServiceClient.ru.kso.autoservice.sort;
using System;
using System.Collections.Generic;
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
        private readonly List<(ComboBoxItem comboBoxItem, Type page)> _comboUserTypeItems;
        private readonly List<(ComboBoxItem comboBoxItem, IServiceSorting sorting)> _comboFilterItems;
        private readonly ServiceCollection _serviceCollection;
        private readonly double _pageCount;

        public RootPage()
        {
            this.InitializeComponent();
            _comboSortItems = new List<(ComboBoxItem, IServiceSorting)>();
            _comboUserTypeItems = new List<(ComboBoxItem comboBoxItem, Type page)>();
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
            string pageTextData = string.Format("Страница {0} из {1}", _serviceCollection.CurrentPage, _pageCount);
            _pageTextBlock.Text = pageTextData;
            string tupleTextData = string.Format("Показано {0} из {1} услуг", _serviceCollection.Start - 1, _serviceCollection.Size);
            _tuplesTextBlock.Text = tupleTextData;
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
            ComboBoxItem itemClient = new ComboBoxItem
            {
                Content = "Клиент"
            };
            ComboBoxItem itemManager = new ComboBoxItem
            {
                Content = "Управляющий"
            };
            _comboUserTypeItems.Add((itemClient, typeof(ServiceListPage)));
            _comboUserTypeItems.Add((itemManager, typeof(ManagerPage)));
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
            if (!(_userTypeComboBox.SelectedItem is ComboBoxItem current))
            {
                return;
            }
            Type page = _comboUserTypeItems.Find(item => item.comboBoxItem.Equals(current)).page;
            if (page == null)
            {
                return;
            }
            _contentFrame.Navigate(page);
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
            string pageTextData = string.Format("Страница {0} из {1}", _serviceCollection.CurrentPage, _pageCount);
            _pageTextBlock.Text = pageTextData;
            string tupleTextData = string.Format("Показано {0} из {1} услуг", _serviceCollection.Start - 1, _serviceCollection.Size);
            _tuplesTextBlock.Text = tupleTextData;
            Sort(_sortingComboBox, _comboSortItems);
        }

        private void BtnNextPageClick(object sender, RoutedEventArgs e)
        {
            _serviceCollection.FetchNextPage();
            string pageTextData = string.Format("Страница {0} из {1}", _serviceCollection.CurrentPage, _pageCount);
            _pageTextBlock.Text = pageTextData;
            string tupleTextData = string.Format("Показано {0} из {1} услуг", _serviceCollection.Start - 1, _serviceCollection.Size);
            _tuplesTextBlock.Text = tupleTextData;
            Sort(_sortingComboBox, _comboSortItems);
        }


        private void BtnInverseLoaded(object sender, RoutedEventArgs e)
        {
            _btnInverse.Click += Reverse;
        }
    }
}
