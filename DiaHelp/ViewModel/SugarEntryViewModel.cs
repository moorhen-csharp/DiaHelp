using CommunityToolkit.Mvvm.Input;
using DiaHelp.Interface;
using DiaHelp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DiaHelp.ViewModel
{
    public class SugarEntryViewModel : BaseViewModel
    {
        private double _sugarLevel;
        private int _insulinDose;
        private string _selectedSugarType;
        private IWindowService _windowService;
        private readonly IDatabaseService _databaseService;
        public ObservableCollection<SugarModel> SugarNotes { get; set; }

        public SugarEntryViewModel(IDatabaseService databaseService, IWindowService windowService) 
        {
            _databaseService = databaseService;
            _windowService = windowService;
            SugarNotePage = new RelayCommand(SugarPage);
            SaveDataCommand = new RelayCommand(SaveSugarNote);
            SelectSugarTypeCommand = new RelayCommand<string>(SelectSugarType);
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

        public int InsulinDose
        {
            get => _insulinDose;
            set
            {
                _insulinDose = value;
                OnPropertyChanged(nameof(InsulinDose));
            }
        }

        private void SelectSugarType(string sugarType)
        {
            SelectedSugarType = sugarType;
        }

        private async void SaveSugarNote(object parametr)
        {
            var sugarNote = new SugarModel
            {
                SugarLevel = SugarLevel,
                MeasurementTime = SelectedSugarType,
                InsulinDose = InsulinDose,
                Date = DateTime.Now
            };

            if (SugarLevel <= 0)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Уровень сахара не может быть меньше или равен 0.", "ОК");
                return;
            }

            if (string.IsNullOrEmpty(SelectedSugarType))
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Пожалуйста, выберите тип измерения.", "ОК");
                return;
            }

            if (_databaseService.AddSugarNote(sugarNote))
            {
                SugarLevel = 0;
                SelectedSugarType = null;
                InsulinDose = 0;

                Application.Current.MainPage = _windowService.GetAndCreateContentPage<SugarNoteViewModel>().View;
            }
        }

        public void SugarPage(object parametr)
        {
            Application.Current.MainPage = _windowService.GetAndCreateContentPage<SugarNoteViewModel>().View;
        }

        public ICommand SugarNotePage { get; }
        public ICommand SaveDataCommand { get; }
        public ICommand SelectSugarTypeCommand { get; }
    }
}
