using DiaHelp.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm;
using System.Windows.Input;

namespace DiaHelp.ViewModel
{
    public class SugarAverageLevelViewModel : BaseViewModel
    {
        private readonly IDatabaseService _db;
        private readonly IWindowService _windowService;
        private string _selectedPeriod = "3 Месяца";
        private decimal _average;
        public List<string> Periods { get; }

        public SugarAverageLevelViewModel(IDatabaseService databaseService, IWindowService windowService)
        {
            _windowService = windowService;
            _db = databaseService;
            Periods = ["3 Месяца", "6 Месяцев", "1 Год"];
            CalculateCommand = new Command(async () => await CalculateAverage());
            MainPageCommand = new RelayCommand(GoToMain);
            _windowService = windowService;
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

        public decimal Average
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

            var notes = await _db.GetSugarNotesByPeriod(startDate, endDate);
            Average = notes.Any() ? notes.Average(n => n.SugarLevel) : 0;
        }

        public void GoToMain(object parametr) => Application.Current.MainPage = _windowService.GetAndCreateContentPage<SugarNoteViewModel>().View;

        public ICommand CalculateCommand { get; }
        public ICommand MainPageCommand { get; }
    }
}
