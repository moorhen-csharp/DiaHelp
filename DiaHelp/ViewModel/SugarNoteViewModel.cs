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
        public string _measurementTime { get; set; }
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

        public string MeasurementTime
        {
            get => _measurementTime;
            set
            {
                _measurementTime = value;
                OnPropertyChanged(nameof(MeasurementTime));
            }
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

        private void MainGo(object parameter) => Application.Current.MainPage = _windowService.GetAndCreateContentPage<MainViewModel>().View;

        public void EntryPage(object parametr) => Application.Current.MainPage = _windowService.GetAndCreateContentPage<SugarEntryViewModel>().View;

        public ICommand MainPage { get; }
        public ICommand EntryData { get; }
    }
}
