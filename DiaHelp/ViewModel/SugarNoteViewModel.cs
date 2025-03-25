using DiaHelp.Interface;
using DiaHelp.Model;
using Microsoft.Maui.Graphics;
using System.Collections.ObjectModel;
using System.Windows.Input;
using static Microsoft.Diagnostics.Tracing.Analysis.GC.TraceGC;

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
            MainPage = new RelayCommand(MainGo);
            EntryData = new RelayCommand(EntryPage);
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

        private void UpdateChart()
        {
            ChartDrawable = new SugarChartDrawable(SugarNotes);
            OnPropertyChanged(nameof(ChartDrawable));
        }

        private void LoadSugarNotes()
        {
            var notes = _databaseService.GetAllSugarNotes();
            SugarNotes.Clear();
            foreach (var note in notes)
            {
                SugarNotes.Add(note);
            }
            UpdateChart(); // Обновляем график при загрузке данных
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
                UpdateChart();
            }
            
        }

        private void MainGo(object parameter) => Application.Current.MainPage = _windowService.GetAndCreateContentPage<MainViewModel>().View;

        private void ClearNote(object parameter)
        {
            if (_databaseService.ClearAllSugarNotes())
            {
                SugarNotes.Clear();
                UpdateChart(); 
            }
        }

        private void Average(object parameter) => Application.Current.MainPage = _windowService.GetAndCreateContentPage<SugarAverageLevelViewModel>().View;

        public void EntryPage(object parametr) => Application.Current.MainPage = _windowService.GetAndCreateContentPage<SugarEntryViewModel>().View;

        public IDrawable ChartDrawable { get; set; }

        public ICommand Clear { get; }
        public ICommand MainPage { get; }
        public ICommand EntryData { get; }
    }
}