using BenchmarkDotNet.Attributes;
using DiaHelp.Interface;
using System.Windows.Input;

namespace DiaHelp.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private double _currentGlucose;
        private double _targetGlucose;
        private double _isf;
        private double _currentValue;
        private string _result;
        private IWindowService _windowService;

        public MainViewModel(IWindowService windowService)
        {
            _windowService = windowService;
            CalculateCommand = new Command(CalculateInsulin);
            SugarPageCommand = new RelayCommand(SugarGo);
            ProfileCommand = new RelayCommand(Profile);
            BreadUnitCommand = new RelayCommand(BreadUnitGo);
        }
        public double CurrentGlucose
        {
            get => _currentGlucose;
            set
            {
                _currentGlucose = value;
                OnPropertyChanged();
            }
        }

        public double TargetGlucose
        {
            get => _targetGlucose;
            set
            {
                _targetGlucose = value;
                OnPropertyChanged();
            }
        }

        public double ISF
        {
            get => _isf;
            set
            {
                _isf = value;
                OnPropertyChanged();
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

        public string Result
        {
            get => _result;
            set
            {
                if (_result != value)
                {
                    _result = value;
                    OnPropertyChanged(nameof(Result));
                }
            }
        }

        private void Profile(object parameter) => Application.Current.MainPage = _windowService.GetAndCreateContentPage<ProfileViewModel>().View;

        private async void CalculateInsulin(object parameter)
        {
            if (double.TryParse(CurrentGlucose.ToString(), out double currentGlucose) && double.TryParse(TargetGlucose.ToString(), out double targetGlucose) && double.TryParse(ISF.ToString(), out double isf))
            {
                if (currentGlucose <= 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", "Пожалуйста, введите текущий уровень глюкозы.", "ОК");
                }
                else if (targetGlucose <= 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", "Пожалуйста, введите целевой уровень глюкозы.", "ОК");
                }
                else if (isf <= 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", "Пожалуйста, введите коэффициент инсулина.", "ОК");
                }

                else if (currentGlucose > 0 && targetGlucose > 0 && isf > 0)
                {
                    double correctionInsulin = (currentGlucose - targetGlucose) / isf;

                    if (correctionInsulin < 0)
                    {
                        correctionInsulin = 0;
                    }

                    await AnimateResult(correctionInsulin);
                }
            }
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

        private void SugarGo(object parameter) => Application.Current.MainPage = _windowService.GetAndCreateContentPage<SugarNoteViewModel>().View;

        private void BreadUnitGo(object parametr) => Application.Current.MainPage = _windowService.GetAndCreateContentPage<BreadUnitViewModel>().View;

        public ICommand SugarPageCommand { get; }
        public ICommand CalculateCommand { get; }
        public ICommand ProfileCommand { get; }
        public ICommand BreadUnitCommand { get; }  
    }
}
