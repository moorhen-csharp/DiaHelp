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
            UpdateChart(); 
        }

        private void MainGo(object parameter) => Application.Current.MainPage = _windowService.GetAndCreateContentPage<MainViewModel>().View;

        public void EntryPage(object parametr) => Application.Current.MainPage = _windowService.GetAndCreateContentPage<SugarEntryViewModel>().View;

        public IDrawable ChartDrawable { get; set; }

        public ICommand Clear { get; }
        public ICommand MainPage { get; }
        public ICommand EntryData { get; }
    }
}