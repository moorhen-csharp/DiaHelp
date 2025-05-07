using CommunityToolkit.Mvvm.Input;
using DiaHelp.Interface;
using DiaHelp.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace DiaHelp.ViewModel
{
    public class SugarEntryViewModel : BaseViewModel
    {
        private double _sugarLevel;
        private int _insulinDose;
        private string _selectedSugarType;
        private IWindowService _windowService;
        private string _selectedHealthType;
        private readonly IDatabaseService _databaseService;
        private bool _isNotMeasured;
        public ObservableCollection<SugarModel> SugarNotes { get; set; }

        public SugarEntryViewModel(IDatabaseService databaseService, IWindowService windowService)
        {
            _databaseService = databaseService;
            _windowService = windowService;
            SugarNotePage = new RelayCommand(SugarPage);
            SaveDataCommand = new RelayCommand(SaveSugarNote);
            SelectSugarTypeCommand = new RelayCommand<string>(SelectSugarType);
            SelectHealthTypeCommand = new RelayCommand<string>(SelectHealthType);
            NotMeasuredCommand = new RelayCommand(NotMeasured);
        }

        public bool IsNotMeasured
        {
            get => _isNotMeasured;
            set
            {
                _isNotMeasured = value;
                OnPropertyChanged(nameof(IsNotMeasured));
            }
        }

        public double SugarLevel
        {
            get => _sugarLevel;
            set
            {
                _sugarLevel = value;
                OnPropertyChanged(nameof(SugarLevel));
            }
        }

        public string SelectedSugarType
        {
            get => _selectedSugarType;
            set
            {
                _selectedSugarType = value;
                OnPropertyChanged(nameof(SelectedSugarType));
            }
        }
        public string SelectedHealthType
        {
            get => _selectedHealthType;
            set
            {
                _selectedHealthType = value;
                OnPropertyChanged(nameof(SelectedHealthType));
            }
        }

        public int InsulinDose
        {
            get => _insulinDose;
            set
            {
                _insulinDose = value;
                OnPropertyChanged(nameof(InsulinDose));
            }
        }

        private void SelectSugarType(string sugarType) => SelectedSugarType = sugarType;
        private void SelectHealthType(string healthType) => SelectedHealthType = healthType;

        private void NotMeasured(object parametr)
        {
            IsNotMeasured = true;
            SugarLevel = 0;
        }

        private async void SaveSugarNote(object parametr)
        {
            if (string.IsNullOrEmpty(SelectedSugarType))
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Пожалуйста, выберите тип измерения.", "ОК");
                return;
            }

            if (string.IsNullOrEmpty(SelectedHealthType))
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Пожалуйста, Введите ваше самочувствие.", "ОК");
                return;
            }

            if (!IsNotMeasured && SugarLevel <= 0)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Уровень сахара не может быть меньше или равен 0.", "ОК");
                return;
            }

            if (!IsNotMeasured)
            {
                if (SugarLevel <= 3 && SugarLevel >= 2.1)
                {
                    await Application.Current.MainPage.DisplayAlert("Внимание!", $"Ваш уровень сахара = {SugarLevel}, Срочно повысьте его.", "ОК");
                }

                if (SugarLevel <= 2)
                {
                    await Application.Current.MainPage.DisplayAlert("Внимание!", $"Ваш уровень сахара = {SugarLevel}, Срочно повысьте его. Рекомендуется вызвать скорую помощь", "ОК");
                }

                if (SugarLevel >= 20 && SugarLevel <= 29)
                {
                    await Application.Current.MainPage.DisplayAlert("Внимание!", $"Ваш уровень сахара = {SugarLevel}, Его нужно срочно понизить!", "ОК");
                }

                if (SugarLevel >= 30)
                {
                    await Application.Current.MainPage.DisplayAlert("Внимание!", $"Ваш уровень сахара = {SugarLevel}, Его нужно срочно понизить! Рекомендуется вызвать скорую помощь.", "ОК");
                }
            }

            var sugarNote = new SugarModel
            {
                SugarLevel = IsNotMeasured ? -1 : SugarLevel,
                MeasurementTime = SelectedSugarType,
                HealthType = SelectedHealthType,
                InsulinDose = InsulinDose,
                Date = DateTime.Now
            };

            if (_databaseService.AddSugarNote(sugarNote))
            {
                SugarLevel = 0;
                SelectedSugarType = null;
                SelectedHealthType = null;
                InsulinDose = 0;
                IsNotMeasured = false;

                Application.Current.MainPage = _windowService.GetAndCreateContentPage<SugarNoteViewModel>().View;
            }
        }

        public void SugarPage(object parametr) => Application.Current.MainPage = _windowService.GetAndCreateContentPage<SugarNoteViewModel>().View;


        public ICommand SugarNotePage { get; }
        public ICommand SaveDataCommand { get; }
        public ICommand SelectSugarTypeCommand { get; }
        public ICommand SelectHealthTypeCommand { get; }
        public ICommand NotMeasuredCommand { get; }
    }
}
