using DiaHelp.Interface;
using DiaHelp.Model;
using DiaHelp.Services;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DiaHelp.ViewModel
{
    public partial class RegistrationViewModel : BaseViewModel
    {
        private readonly IDatabaseService _databaseService;
        private readonly IWindowService _windowService;
        private string _email;
        private string _password;
        private string _confirmPassword;

        public RegistrationViewModel(IDatabaseService databaseService, IWindowService windowService)
        {
            _windowService = windowService;

            _databaseService = databaseService;
            RegisterCmnd = new RelayCommand(async _ => await Register());
            LoginCmnd = new RelayCommand(async _ => await NavigateToLogin());
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        private async Task Register()
        {
            if (Password != ConfirmPassword)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Пароли не совпадают", "OK");
                return;
            }

            var newUser = new User
            {
                Email = Email,
                Password = Password,
                Username = Email,
                RegistrationDate = DateTime.UtcNow
            };

            if (_databaseService.AddUser(newUser))
            {
                await Application.Current.MainPage.DisplayAlert("Успех", "Регистрация завершена", "OK");
                Application.Current.MainPage = _windowService.GetAndCreateContentPage<LoginViewModel>().View;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Не удалось зарегистрироваться", "OK");
            }
        }

        private async Task NavigateToLogin() => Application.Current.MainPage = _windowService.GetAndCreateContentPage<LoginViewModel>().View;


        public ICommand RegisterCmnd { get; }
        public ICommand LoginCmnd { get; }
    }
}