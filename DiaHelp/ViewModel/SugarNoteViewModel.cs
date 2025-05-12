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
        private string _selectedPeriod = "3 Месяца";
        private double _average;
        public List<string> Periods { get; }

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

            Periods = ["3 Месяца", "6 Месяцев", "1 Год"];
            CalculateCommand = new Command(async () => await CalculateAverage());


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

        public string SelectedPeriod
        {
            get => _selectedPeriod;
            set
            {
                _selectedPeriod = value;
                OnPropertyChanged();
                CalculateAverage().GetAwaiter();
            }
        }

        public double Average
        {
            get => _average;
            set
            {
                _average = value;
                OnPropertyChanged();
            }
        }

        private async Task CalculateAverage()
        {
            if (string.IsNullOrEmpty(SelectedPeriod)) return;

            var endDate = DateTime.Now;
            var startDate = SelectedPeriod switch
            {
                "3 Месяца" => endDate.AddMonths(-3),
                "6 Месяцев" => endDate.AddMonths(-6),
                "1 Год" => endDate.AddYears(-1),
                _ => endDate
            };

            var notes = await _databaseService.GetSugarNotesByPeriod(startDate, endDate);
            var validNotes = notes.Where(n => n.SugarLevel != -1).ToList();
            Average = validNotes.Any() ? validNotes.Average(n => n.SugarLevel) : 0;
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

                CalculateAverage();
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
        public ICommand CalculateCommand { get; }
    }
}