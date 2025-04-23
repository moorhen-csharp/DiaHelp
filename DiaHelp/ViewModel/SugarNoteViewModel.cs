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
        private bool _isListEmpty = true;
        private bool _borderVsbl = true;
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
            Clear = new RelayCommand(ClearNoteData);

            LoadSugarNotesAsync();
        }

        public bool IsListEmpty
        {
            get => _isListEmpty;
            set
            {
                _isListEmpty = value;
                OnPropertyChanged(nameof(IsListEmpty));
                BorderVsbl = !_isListEmpty;
            }
        }

        public bool BorderVsbl
        {
            get => !_isListEmpty;
            set
            {
                if (value != !_isListEmpty)
                {
                    OnPropertyChanged(nameof(BorderVsbl));
                }
            }
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

        private async Task LoadSugarNotesAsync()
        {
            try
            {
                var notes = await _databaseService.GetAllSugarNotesAsync();

                SugarNotes.Clear();

                foreach (var note in notes)
                {
                    SugarNotes.Add(note);
                }

                IsListEmpty = SugarNotes.Count == 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке заметок: {ex.Message}");
            }
        }

        public void ClearNoteData(object parametr)
        {
            _databaseService.ClearAllSugarNotes();
            LoadSugarNotesAsync();
        }
        private void MainGo(object parameter) => Application.Current.MainPage = _windowService.GetAndCreateContentPage<MainViewModel>().View;

        public void EntryPage(object parametr) => Application.Current.MainPage = _windowService.GetAndCreateContentPage<SugarEntryViewModel>().View;

        public ICommand MainPage { get; }
        public ICommand Clear { get; }
        public ICommand EntryData { get; }
    }
}
