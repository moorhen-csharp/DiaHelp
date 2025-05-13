using DiaHelp.Interface;
using System.Windows.Input;
using DiaHelp.Infrastructure;

namespace DiaHelp.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private double _currentGlucose;
        private double _targetGlucose = 5;
        private double _currentValue;
        private string _result;
        private readonly IDatabaseService _databaseService;
        private IWindowService _windowService;

        public MainViewModel(IWindowService windowService, IDatabaseService databaseService)
        {
            _databaseService = databaseService;
            _windowService = windowService;
            CalculateCommand = new Command(CalculateInsulin);
            SugarPageCommand = new RelayCommand(SugarGo);
            ProfileCommand = new RelayCommand(Profile);
            BreadUnitCommand = new RelayCommand(BreadUnitGo);
            DiabetesSchoolPageCommand = new RelayCommand(DiabetesSchoolGo);
            AiChatCommand = new RelayCommand(AiChatGo);
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

        private async void CalculateInsulin(object parameter)
        {
            var username = Preferences.Get("CurrentUsername", string.Empty);

            if (string.IsNullOrEmpty(username))
                return;

            var user = _databaseService.GetUser(username);

            if (double.TryParse(CurrentGlucose.ToString(), out double currentGlucose) && double.TryParse(TargetGlucose.ToString(), out double targetGlucose))
            {
                if (currentGlucose <= 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", "Пожалуйста, введите текущий уровень глюкозы.", "ОК");
                }
                else if (targetGlucose <= 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", "Пожалуйста, введите целевой уровень глюкозы.", "ОК");
                }

                else if (currentGlucose > 0 && targetGlucose > 0 && user.CoeffInsulin > 0)
                {
                    double correctionInsulin = (currentGlucose - targetGlucose) / user.CoeffInsulin;

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

        public async void AiChatGo(object parametr)
        {
            var page = _windowService.GetAndCreateContentPage<AiChatViewModel>().View;
            await PageTransitionHelper.AnimateTransition(page, true);
        }

        private async void SugarGo(object parameter)
        {
            var page = _windowService.GetAndCreateContentPage<SugarNoteViewModel>().View;
            await PageTransitionHelper.AnimateTransition(page, true);
        }

        private async void BreadUnitGo(object parametr)
        {
            var page = _windowService.GetAndCreateContentPage<BreadUnitViewModel>().View;
            await PageTransitionHelper.AnimateTransition(page, true);
        }

        private async void Profile(object parameter)
        {
            var page = _windowService.GetAndCreateContentPage<ProfileViewModel>().View;
            await PageTransitionHelper.AnimateTransition(page, true);
        }

        private async void DiabetesSchoolGo(object parametr)
        {
            var page = _windowService.GetAndCreateContentPage<DiabetesSchoolViewModel>().View;
            await PageTransitionHelper.AnimateTransition(page, true);
        }

        public ICommand SugarPageCommand { get; }
        public ICommand CalculateCommand { get; }
        public ICommand ProfileCommand { get; }
        public ICommand BreadUnitCommand { get; }
        public ICommand DiabetesSchoolPageCommand { get; }
        public ICommand AiChatCommand { get; }
    }
}
