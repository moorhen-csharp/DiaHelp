using CommunityToolkit.Mvvm.Input;
using DiaHelp.Interface;
using DiaHelp.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace DiaHelp.ViewModel
{
    public class FoodEntryViewModel : BaseViewModel
    {
        private IWindowService _windowService;
        private string _foodName;
        private string _category;
        private double _weight;
        private double _insulinFromFood;
        private string _selectedCategory;
        private string _selectedFoodType;
        private readonly IDatabaseService _databaseService;
        public ObservableCollection<FoodModel> FoodNotes { get; set; }
        public List<string> Categories { get; }

        public FoodEntryViewModel(IDatabaseService databaseService, IWindowService windowService)
        {
            _databaseService = databaseService;
            _windowService = windowService;
            SugarNotePage = new RelayCommand(SugarPage);
            SaveDataCommand = new RelayCommand(SaveFoodNote);
            SelectFoodTypeCommand = new RelayCommand<string>(SelectFoodType);
            Categories = ["Газировка", "Фастфуд", "Мучное", "Макаронные изделия"];
        }

        private void SelectFoodType(string foodType) => SelectedFoodType = foodType;

        public string SelectedFoodType
        {
            get => _selectedFoodType;
            set
            {
                _selectedFoodType = value;
                OnPropertyChanged(nameof(SelectedFoodType));
            }
        }

        public string FoodName
        {
            get => _foodName;
            set
            {
                _foodName = value;
                OnPropertyChanged(nameof(FoodName));
            }
        }
        public string Category
        {
            get => _category;
            set
            {
                _category = value;
                OnPropertyChanged(nameof(Category));
            }
        }
        public double Weight
        {
            get => _weight;
            set
            {
                _weight = value;
                OnPropertyChanged(nameof(Weight));
            }
        }
        public double InsulinFromFood
        {
            get => _insulinFromFood;
            set
            {
                _insulinFromFood = value;
                OnPropertyChanged(nameof(InsulinFromFood));
            }
        }

        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
            }
        }
        private async void SaveFoodNote(object parametr)
        {
            var foodNote = new FoodModel
            {
                FoodName = FoodName,
                FoodType = SelectedFoodType,
                Category = SelectedCategory,
                Weight = Weight,
                InsulinFromFood = InsulinFromFood,
                Date = DateTime.Now
            };

            if (_databaseService.AddFoodNote(foodNote))
            {
                Application.Current.MainPage = _windowService.GetAndCreateContentPage<SugarNoteViewModel>().View;
            }
        }

        public void SugarPage(object parametr) => Application.Current.MainPage = _windowService.GetAndCreateContentPage<SugarNoteViewModel>().View;

        public ICommand SugarNotePage { get; }
        public ICommand SaveDataCommand { get; }
        public ICommand SelectFoodTypeCommand {  get; }
    }
}