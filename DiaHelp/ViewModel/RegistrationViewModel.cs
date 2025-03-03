using DiaHelp.Interface;
using DiaHelp.Model;
using DiaHelp.Services;
using System.Text.RegularExpressions;
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
            RegisterCmnd = new RelayCommand(Register);
            LoginCmnd = new RelayCommand(NavigateToLogin);
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

        private async void Register(object parameter)
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Введите email", "OK");
                return;
            }

            if (!IsValidEmail(Email))
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Некорректный формат email", "OK");
                return;
            }

            if (Password != ConfirmPassword)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Пароли не совпадают", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Введите пароль и подтвердите его", "OK");
                return;
            }

            var newUser = new UserModel
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
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Не удалось зарегистрироваться, возможно аккаунт уже существует", "OK");
            }
        }

        private bool IsValidEmail(string email)
        {
            var regex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, regex);
        }

        private async void NavigateToLogin(object parameter) => Application.Current.MainPage = _windowService.GetAndCreateContentPage<LoginViewModel>().View;


        public ICommand RegisterCmnd { get; }
        public ICommand LoginCmnd { get; }
    }
}