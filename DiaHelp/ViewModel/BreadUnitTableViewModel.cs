using DiaHelp.Interface;
using DiaHelp.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace DiaHelp.ViewModel
{
    public class BreadUnitTableViewModel : BaseViewModel
    {
        private IWindowService _windowService;
        private ObservableCollection<ProductItem> _products;

        public BreadUnitTableViewModel(IWindowService windowService)
        {
            _windowService = windowService;
            MainPageCommand = new RelayCommand(MainGo);
            Products = new ObservableCollection<ProductItem>();
            BackCommand = new RelayCommand(BUGo);
            LoadDataAsync();
        }
        public ObservableCollection<ProductItem> Products
        {
            get => _products;
            private set => SetProperty(ref _products, value);
        }

        private async Task LoadDataAsync()
        {
            var productsToAdd = await Task.Run(() =>
            {
                return new[]
                {
                new ProductItem { ProductName = "белый, серый хлеб (кроме сдобного)", ProductWeight = "1 кусок толщиной 1 см", QuantityPerXE = 20 },
                new ProductItem { ProductName = "чёрный хлеб", ProductWeight = "1 кусок толщиной 1 см", QuantityPerXE = 25 },
                new ProductItem { ProductName = "хлеб с отрубями", ProductWeight = "1 кусок толщиной 1,3 см", QuantityPerXE = 30 },
                new ProductItem { ProductName = "хлеб бородинский", ProductWeight = "1 кусок толщиной 0,6 см", QuantityPerXE = 15 },
                new ProductItem { ProductName = "сухари", ProductWeight = "горсть", QuantityPerXE = 15 },
                new ProductItem { ProductName = "крекеры (сухое печенье)", ProductWeight = "-", QuantityPerXE = 15 },
                new ProductItem { ProductName = "панировочные сухари", ProductWeight = "-", QuantityPerXE = 15 },
                new ProductItem { ProductName = "сдобная булка", ProductWeight = "-", QuantityPerXE = 20 },
                new ProductItem { ProductName = "блин (большой)", ProductWeight = "1 шт.", QuantityPerXE = 30 },
                new ProductItem { ProductName = "вареники с творогом замороженные", ProductWeight = "4 шт.", QuantityPerXE = 50 },
                new ProductItem { ProductName = "пельмени замороженные", ProductWeight = "4 шт", QuantityPerXE = 50 },
                new ProductItem { ProductName = "ватрушка", ProductWeight = "-", QuantityPerXE = 50 },
                new ProductItem { ProductName = "вафли (мелкие)", ProductWeight = "1,5 шт", QuantityPerXE = 17 },
                new ProductItem { ProductName = "мука", ProductWeight = "1 ст. ложка с горкой", QuantityPerXE = 15 },
                new ProductItem { ProductName = "пряник", ProductWeight = "0,5 шт.", QuantityPerXE = 40 },
                new ProductItem { ProductName = "оладьи (средние)", ProductWeight = "1 шт", QuantityPerXE = 30 },
                new ProductItem { ProductName = "макаронные изделия (в сыром виде)", ProductWeight = "1-2 ст. ложки (в зависимости от формы)", QuantityPerXE = 15 },
                new ProductItem { ProductName = "макаронные изделия (в вареном виде)", ProductWeight = "2-4 ст. ложки (в зависимости от формы)", QuantityPerXE = 50 },
                new ProductItem { ProductName = "крупа (любая, в сыром виде)", ProductWeight = "1 ст. ложка", QuantityPerXE = 15 },
                new ProductItem { ProductName = "каша (любая)", ProductWeight = "2 ст. ложки с горкой", QuantityPerXE = 50 },
                new ProductItem { ProductName = "кукуруза (средняя)", ProductWeight = "0,5 початка", QuantityPerXE = 100 },
                new ProductItem { ProductName = "кукуруза (консервированная)", ProductWeight = "3 ст. ложки", QuantityPerXE = 60 },
                new ProductItem { ProductName = "кукурузные хлопья", ProductWeight = "4 ст. ложки", QuantityPerXE = 15 },
                new ProductItem { ProductName = "попкорн", ProductWeight = "10 ст. ложек", QuantityPerXE = 15 },
                new ProductItem { ProductName = "овсяные хлопья", ProductWeight = "2 ст. ложки", QuantityPerXE = 10 },
                new ProductItem { ProductName = "пшеничные отруби", ProductWeight = "12 ст. ложек", QuantityPerXE = 50 },
                };
            });

            Products = new ObservableCollection<ProductItem>(productsToAdd);
        }

        public void BUGo(object parametr) => Application.Current.MainPage = _windowService.GetAndCreateContentPage<BreadUnitViewModel>().View;

        private void MainGo(object parametr) => Application.Current.MainPage = _windowService.GetAndCreateContentPage<MainViewModel>().View;

        public ICommand MainPageCommand { get; }
        public ICommand BackCommand { get; }
    }
}
