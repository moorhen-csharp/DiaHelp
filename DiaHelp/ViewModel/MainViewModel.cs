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
            SugarPage = new Command(SugarGo);
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
        // Свойство CurrentValue для анимации
        public double CurrentValue
        {
            get => _currentValue;
            set
            {
                if (_currentValue != value)
                {
                    _currentValue = value;
                    OnPropertyChanged(nameof(CurrentValue));
                    Result = Math.Round(value, 2).ToString(); // Обновляем результат
                }
            }
        }

        // Свойство для отображения результата
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

        

        // Команда для расчета
        private async void CalculateInsulin()
        {
            try
            {
          
                if (double.TryParse(CurrentGlucose.ToString(), out double currentGlucose) &&
                    double.TryParse(TargetGlucose.ToString(), out double targetGlucose) &&
                    double.TryParse(ISF.ToString(), out double isf))
                {
                    if (isf == 0)
                    {
                        throw new ArgumentException("Корректирующий коэффициент (ISF) не может быть равен нулю.");
                    }

                    double correctionInsulin = (currentGlucose - targetGlucose) / isf;

                    if (correctionInsulin < 0)
                    {
                        correctionInsulin = 0;
                    }

                    // Анимация результата
                    await AnimateResult(correctionInsulin);
                }
            }
            catch (Exception ex)
            {
                Result = $"Ошибка: {ex.Message}";
            }
        }

        private async Task AnimateResult(double finalValue)
        {
            double step = finalValue / 50; // Шаг увеличения
            for (double i = 0; i <= finalValue; i += step)
            {
                CurrentValue = i; // Обновляем текущее значение
                await Task.Delay(30); // Задержка для создания эффекта анимации
            }
            CurrentValue = finalValue; // Устанавливаем окончательное значение
        }

        private async void SugarGo()
        {
            Application.Current.MainPage = _windowService.GetAndCreateContentPage<SugarNoteViewModel>().View;
        }

        
        public ICommand SugarPage { get; }
        public Command CalculateCommand { get; }
    }
}
