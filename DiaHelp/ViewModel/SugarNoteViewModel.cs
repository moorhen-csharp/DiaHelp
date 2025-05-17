using DiaHelp.Interface;
using DiaHelp.Model;
using DiaHelp.View;
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
        private bool _isSugarListVisible = true;
        private decimal _sugarLevel { get; set; }
        public string _measurementTime { get; set; }

        public List<string> Periods { get; }
        public ObservableCollection<SugarModel> SugarNotes { get; set; }
        public ObservableCollection<FoodModel> FoodNotes { get; set; }

        public SugarNoteViewModel(IDatabaseService databaseService, IWindowService windowService)
        {
            _windowService = windowService;
            _databaseService = databaseService;

            SugarNotes = new ObservableCollection<SugarModel>();
            FoodNotes = new ObservableCollection<FoodModel>();

            MainPage = new RelayCommand(MainGo);
            EntryData = new RelayCommand(EntryPage);
            FoodEntryData = new RelayCommand(FoodEntryPage);
            Clear = new RelayCommand(ClearNoteData);
            ShowSugarList = new RelayCommand(OnShowSugarList);
            ShowFoodList = new RelayCommand(OnShowFoodList);

            Periods = ["1 День","3 Месяца", "6 Месяцев", "1 Год"];
            CalculateCommand = new Command(async () => await CalculateAverage());

            LoadSugarNotesAsync();
            LoadFoodNotesAsync();
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

        public bool IsSugarListVisible
        {
            get => _isSugarListVisible;
            set
            {
                _isSugarListVisible = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsFoodListVisible));
            }
        }

        public bool IsFoodListVisible
        {
            get => !_isSugarListVisible;
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
                "1 День" => endDate.AddDays(-1),
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

        private async Task LoadFoodNotesAsync()
        {
            try
            {
                var notes = await _databaseService.GetAllFoodNotesAsync();
                FoodNotes.Clear();
                foreach (var note in notes)
                {
                    FoodNotes.Add(note);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке заметок о еде: {ex.Message}");
            }
        }

        public void ClearNoteData(object parametr)
        {
            _databaseService.ClearAllSugarNotes();
            LoadSugarNotesAsync();
        }

        private void OnShowSugarList(object parameter)
        {
            IsSugarListVisible = true;
            IsListEmpty = SugarNotes.Count == 0;
        }

        private void OnShowFoodList(object parameter)
        {
            IsSugarListVisible = false;
            IsListEmpty = FoodNotes.Count == 0;
        }

        private void MainGo(object parameter) => Application.Current.MainPage = _windowService.GetAndCreateContentPage<MainViewModel>().View;

        public void EntryPage(object parametr) => Application.Current.MainPage = _windowService.GetAndCreateContentPage<SugarEntryViewModel>().View;

        public void FoodEntryPage(object parametr) => Application.Current.MainPage = _windowService.GetAndCreateContentPage<FoodEntryViewModel>().View;

        public ICommand MainPage { get; }
        public ICommand Clear { get; }
        public ICommand EntryData { get; }
        public ICommand CalculateCommand { get; }
        public ICommand ShowSugarList { get; }
        public ICommand ShowFoodList { get; }
        public ICommand FoodEntryData { get; }
    }
}