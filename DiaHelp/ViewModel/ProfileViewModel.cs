using DiaHelp.Interface;
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

        public ProfileViewModel(IWindowService windowService, IDatabaseService databaseService)
        {
            _windowService = windowService;
            LogoutCommand = new RelayCommand(Logout);
            MainPageCommand = new RelayCommand(MainGo);
            _databaseService = databaseService;
            LoadData();
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
            }
        }

        public void Logout(object parametr)
        {
            Preferences.Set("IsUserLoggedIn", false);
            Application.Current.MainPage = _windowService.GetAndCreateContentPage<LoginViewModel>().View;
        }

        private void MainGo(object parametr) => Application.Current.MainPage = _windowService.GetAndCreateContentPage<MainViewModel>().View;

        public ICommand MainPageCommand { get; }
        public ICommand LogoutCommand { get; }
    }
}
