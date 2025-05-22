using DiaHelp.Interface;
using DiaHelp.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace DiaHelp.ViewModel
{
    public class BreadUnitTableViewModel : BaseViewModel
    {
        private IWindowService _windowService;
        private ObservableCollection<ProductItemModel> _products;

        public BreadUnitTableViewModel(IWindowService windowService)
        {
            _windowService = windowService;
            MainPageCommand = new RelayCommand(MainGo);
            Products = new ObservableCollection<ProductItemModel>();
            BackCommand = new RelayCommand(BUGo);
            LoadDataAsync();
        }
        public ObservableCollection<ProductItemModel> Products
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
                new ProductItemModel { ProductName = "белый, серый хлеб (кроме сдобного)", ProductWeight = "1 кусок толщиной 1 см", QuantityPerXE = 20 },
                new ProductItemModel { ProductName = "чёрный хлеб", ProductWeight = "1 кусок толщиной 1 см", QuantityPerXE = 25 },
                new ProductItemModel { ProductName = "хлеб с отрубями", ProductWeight = "1 кусок толщиной 1,3 см", QuantityPerXE = 30 },
                new ProductItemModel { ProductName = "хлеб бородинский", ProductWeight = "1 кусок толщиной 0,6 см", QuantityPerXE = 15 },
                new ProductItemModel { ProductName = "сухари", ProductWeight = "горсть", QuantityPerXE = 15 },
                new ProductItemModel { ProductName = "крекеры (сухое печенье)", ProductWeight = "-", QuantityPerXE = 15 },
                new ProductItemModel { ProductName = "панировочные сухари", ProductWeight = "-", QuantityPerXE = 15 },
                new ProductItemModel { ProductName = "сдобная булка", ProductWeight = "-", QuantityPerXE = 20 },
                new ProductItemModel { ProductName = "блин (большой)", ProductWeight = "1 шт.", QuantityPerXE = 30 },
                new ProductItemModel { ProductName = "вареники с творогом замороженные", ProductWeight = "4 шт.", QuantityPerXE = 50 },
                new ProductItemModel { ProductName = "пельмени замороженные", ProductWeight = "4 шт", QuantityPerXE = 50 },
                new ProductItemModel { ProductName = "ватрушка", ProductWeight = "-", QuantityPerXE = 50 },
                new ProductItemModel { ProductName = "вафли (мелкие)", ProductWeight = "1,5 шт", QuantityPerXE = 17 },
                new ProductItemModel { ProductName = "мука", ProductWeight = "1 ст. ложка с горкой", QuantityPerXE = 15 },
                new ProductItemModel { ProductName = "пряник", ProductWeight = "0,5 шт.", QuantityPerXE = 40 },
                new ProductItemModel { ProductName = "оладьи (средние)", ProductWeight = "1 шт", QuantityPerXE = 30 },
                new ProductItemModel { ProductName = "макаронные изделия (в сыром виде)", ProductWeight = "1-2 ст. ложки (в зависимости от формы)", QuantityPerXE = 15 },
                new ProductItemModel { ProductName = "макаронные изделия (в вареном виде)", ProductWeight = "2-4 ст. ложки (в зависимости от формы)", QuantityPerXE = 50 },
                new ProductItemModel { ProductName = "крупа (любая, в сыром виде)", ProductWeight = "1 ст. ложка", QuantityPerXE = 15 },
                new ProductItemModel { ProductName = "каша (любая)", ProductWeight = "2 ст. ложки с горкой", QuantityPerXE = 50 },
                new ProductItemModel { ProductName = "кукуруза (средняя)", ProductWeight = "0,5 початка", QuantityPerXE = 100 },
                new ProductItemModel { ProductName = "кукуруза (консервированная)", ProductWeight = "3 ст. ложки", QuantityPerXE = 60 },
                new ProductItemModel { ProductName = "кукурузные хлопья", ProductWeight = "4 ст. ложки", QuantityPerXE = 15 },
                new ProductItemModel { ProductName = "попкорн", ProductWeight = "10 ст. ложек", QuantityPerXE = 15 },
                new ProductItemModel { ProductName = "овсяные хлопья", ProductWeight = "2 ст. ложки", QuantityPerXE = 10 },
                new ProductItemModel { ProductName = "пшеничные отруби", ProductWeight = "12 ст. ложек", QuantityPerXE = 50 },
                };
            });

            Products = new ObservableCollection<ProductItemModel>(productsToAdd);
        }

        public void BUGo(object parametr) => Application.Current.MainPage = _windowService.GetAndCreateContentPage<BreadUnitViewModel>().View;

        private void MainGo(object parametr) => Application.Current.MainPage = _windowService.GetAndCreateContentPage<MainViewModel>().View;

        public ICommand MainPageCommand { get; }
        public ICommand BackCommand { get; }
    }
}
