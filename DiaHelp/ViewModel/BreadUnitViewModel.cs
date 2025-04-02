using DiaHelp.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DiaHelp.ViewModel
{
    class BreadUnitViewModel : BaseViewModel
    {
        private double _carbohydrates;
        private double _productWeight;
        private string _result;
        private IWindowService _windowService;
        private double _currentValue;

        public BreadUnitViewModel(IWindowService windowService) 
        {
            _windowService = windowService;
            MainCommand = new RelayCommand(MainGo);
            CalculateCommand = new RelayCommand(CalcelateBU);
            InfoCommand = new RelayCommand(InfoDA);
        }

        public double Carbohydrates
        {
            get => _carbohydrates;
            set
            {
                _carbohydrates = value;
                OnPropertyChanged(nameof(Carbohydrates));
            }
        }

        public double ProductWeight
        {
            get => _productWeight;
            set
            {
                _productWeight = value;
                OnPropertyChanged(nameof(ProductWeight));
            }
        }

        public string Result
        {
            get => _result;
            set
            {
                _result = value;
                OnPropertyChanged(nameof(Result));
            }
        }

        public double CurrentValue
        {
            get => _currentValue;
            set
            {
                if (_currentValue != value)
                {
                    _currentValue = value;
                    OnPropertyChanged(nameof(CurrentValue));
                    Result = Math.Round(value, 2).ToString();
                }
            }
        }

        public async void CalcelateBU(object parametr)
        {
            double CalcBU = ((Carbohydrates * ProductWeight) / 100) / 12;

            Carbohydrates = 0;
            ProductWeight = 0;

            await AnimateResult(CalcBU);
        }

        private async Task AnimateResult(double finalValue)
        {
            double step = finalValue / 50;
            for (double i = 0; i <= finalValue; i += step)
            {
                CurrentValue = i;
                await Task.Delay(30);
            }
            CurrentValue = finalValue;
        }

        public void InfoDA(object parametr) => Application.Current.MainPage.DisplayAlert("Внимание", "Подсчет ХЕ производится с учетом того, что 1 ХЕ это 12г углеводов", "ОК");

        public void MainGo(object parametr) => Application.Current.MainPage = _windowService.GetAndCreateContentPage<MainViewModel>().View;

        public ICommand CalculateCommand { get; }
        public ICommand MainCommand { get; }
        public ICommand InfoCommand { get; }

    }
}
