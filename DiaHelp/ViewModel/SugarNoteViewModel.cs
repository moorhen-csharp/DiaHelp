using DiaHelp.Interface;
using DiaHelp.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace DiaHelp.ViewModel
{
    public class SugarNoteViewModel : BaseViewModel
    {
        private IWindowService _windowService;
        private readonly IDatabaseService _databaseService;
        private decimal _sugarLevel { get; set; }
        public string MeasurementTime { get; set; }
        public string _mealType { get; set; }
        public ObservableCollection<SugarModel> SugarNotes { get; set; }

        public SugarNoteViewModel(IDatabaseService databaseService, IWindowService windowService)
        {
            _windowService = windowService;
            _databaseService = databaseService;
            SugarNotes = new ObservableCollection<SugarModel>();
            AddSugarNoteCommand = new RelayCommand(AddSugarNote);
            MainPage = new RelayCommand(MainGo);
            Clear = new RelayCommand(ClearNote);
            SugarAverage = new RelayCommand(Average);
            LoadSugarNotes();
        }

        public decimal SugarLevel
        {
            get => _sugarLevel;
            set
            {
                _sugarLevel = value;
                OnPropertyChanged(nameof(SugarLevel));
            }
        }

        public string MealType
        {
            get => _mealType;
            set
            {
                _mealType = value;
                OnPropertyChanged(nameof(MealType));
            }
        }

        public async void ClearNote(object parametr)
        {
            if (_databaseService.ClearAllSugarNotes())
            {
                SugarNotes.Clear();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Не удалось очистить записи", "OK");
            }
        }

        public async void AddSugarNote(object parameter)
        {
            var sugarNote = new SugarModel
            {
                SugarLevel = SugarLevel,
                MeasurementTime = MealType,
                Date = DateTime.Now
            };

            if (SugarLevel <= 0)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Уровень сахара не может быть меньше или равен 0.", "ОК");
            }

            else if (string.IsNullOrEmpty(MealType))
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Пожалуйста введите тип измерения.", "ОК");
            }

            else if (_databaseService.AddSugarNote(sugarNote))
            {
                SugarNotes.Insert(0, sugarNote);
                SugarLevel = 0;
                MealType = string.Empty;
            }
        }

        public void Average(object parametr)
        {
            Application.Current.MainPage = _windowService.GetAndCreateContentPage<SugarAverageLevelViewModel>().View;
        }

        private void LoadSugarNotes()
        {
            var notes = _databaseService.GetAllSugarNotes();
            SugarNotes.Clear();
            foreach (var note in notes)
            {
                SugarNotes.Add(note);
            }
        }

        private void MainGo(object parametr) => Application.Current.MainPage = _windowService.GetAndCreateContentPage<MainViewModel>().View;

        public ICommand Clear { get; }
        public ICommand MainPage { get; }
        public ICommand AddSugarNoteCommand { get; }
        public ICommand SugarAverage { get; }
    }
}