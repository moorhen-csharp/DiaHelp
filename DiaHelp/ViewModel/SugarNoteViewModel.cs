﻿using DiaHelp.Interface;
using DiaHelp.Model;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace DiaHelp.ViewModel
{
    public class SugarNoteViewModel : BaseViewModel
    {
        private IWindowService _windowService;
        private readonly IDatabaseService _databaseService;
        private readonly ILogger<SugarNoteViewModel> _logger;
        private decimal _sugarLevel { get; set; }
        public string MeasurementTime { get; set; }
        public string _mealType { get; set; }
        public ObservableCollection<SugarModel> SugarNotes { get; set; }

        public SugarNoteViewModel(IDatabaseService databaseService, ILogger<SugarNoteViewModel> logger, IWindowService windowService)
        {
            _windowService = windowService;
            _databaseService = databaseService;
            _logger = logger;
            SugarNotes = new ObservableCollection<SugarModel>();
            AddSugarNoteCommand = new RelayCommand(AddSugarNote);
            MainPage = new RelayCommand(MainGo);
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

        public async void AddSugarNote(object parameter)
        {
            var sugarNote = new SugarModel
            {
                SugarLevel = SugarLevel,
                MeasurementTime = MealType,
                Date = DateTime.Now 
            };

            try
            {
                if (_databaseService.AddSugarNote(sugarNote)) 
                {
                    SugarNotes.Insert(0, sugarNote);   
                }
                else
                {
                    _logger.LogError("Не удалось добавить запись о сахаре.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при добавлении записи.");
            }

            SugarLevel = 0;
            MealType = string.Empty;
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

        private void MainGo(object parametr)
        {
            Application.Current.MainPage = _windowService.GetAndCreateContentPage<MainViewModel>().View;
        }

        public ICommand MainPage { get; }
        public ICommand AddSugarNoteCommand { get; }
    }
}