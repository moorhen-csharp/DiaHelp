using DiaHelp.Interface;
using System.Diagnostics;
using System.Windows.Input;

namespace DiaHelp.ViewModel
{
    public class ProfileViewModel : BaseViewModel
    {
        private readonly IDatabaseService _databaseService;
        private readonly IWindowService _windowService;
        private string _email;
        private string _name;
        private string _lastName;
        private double _coeffInsulinil;
        private string _experience;
        private bool _entryEnbl = false;
        private bool _entryVsbl = false;
        private bool _labelVsbl = true;
        private string _entryExperience;
        private string _editBtntext = "Изменить";

        public ProfileViewModel(IWindowService windowService, IDatabaseService databaseService)
        {
            _windowService = windowService;
            LogoutCommand = new RelayCommand(Logout);
            MainPageCommand = new RelayCommand(MainGo);
            _databaseService = databaseService;
            EditCommand = new RelayCommand(EditProfile);
            LoadData();
        }
        #region Binding Properties
        public bool LabelVsbl
        {
            get => _labelVsbl;
            set => SetProperty(ref _labelVsbl, value);
        }
        public bool EntryVsbl
        {
            get => _entryVsbl;
            set => SetProperty(ref _entryVsbl, value);
        }
        public bool EntryEnbl
        {
            get => _entryEnbl;
            set => SetProperty(ref _entryEnbl, value);
        }
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }
        public double CoeffInsulin
        {
            get => _coeffInsulinil;
            set => SetProperty(ref _coeffInsulinil, value);
        }
        public string Experience
        {
            get => _experience;
            set => SetProperty(ref _experience, value);
        }
        public string EntryExperience
        {
            get => _entryExperience;
            set => SetProperty(ref _entryExperience, value);
        }
        public string EditBtntext
        {
            get => _editBtntext;
            set => SetProperty(ref _editBtntext, value);
        }
        #endregion

        #region Methods
        public void LoadData()
        {
            var username = Preferences.Get("CurrentUsername", string.Empty);

            if (string.IsNullOrEmpty(username))
                return;

            var user = _databaseService.GetUser(username);

            if (user != null)
            {
                Email = user.Email;
                Name = user.Name;
                LastName = user.LastName;
                CoeffInsulin = user.CoeffInsulin;
                Experience = user.Experience;
            }
        }

        public void EditProfile(object parametr)
        {
            if (EntryVsbl == false && EntryEnbl == false)
            {
                EntryEnbl = true;
                EntryVsbl = true;
                LabelVsbl = false;
                EditBtntext = "Сохранить";
            }

            else
            {
                EntryEnbl = false;
                EntryVsbl = false;
                LabelVsbl = true;
                EditBtntext = "Изменить";

                var user = _databaseService.GetUser(Preferences.Get("CurrentUsername", string.Empty));
                if (user != null)
                {
                    // Обновляем поле Experience
                    user.Experience = EntryExperience;

                    // Сохраняем изменения в базе данных
                    try
                    {
                        _databaseService.UpdateUser(user); // Используем метод UpdateUser
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Ошибка при обновлении данных: {ex.Message}");
                    }
                }
                LoadData();
            }
        }

        public void Logout(object parametr)
        {
            Preferences.Set("IsUserLoggedIn", false);
            Application.Current.MainPage = _windowService.GetAndCreateContentPage<LoginViewModel>().View;
        }

        private void MainGo(object parametr) => Application.Current.MainPage = _windowService.GetAndCreateContentPage<MainViewModel>().View;
        #endregion

        #region ICommand
        public ICommand MainPageCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand EditCommand { get; }
        #endregion
    }
}
