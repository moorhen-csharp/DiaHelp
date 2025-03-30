﻿using DiaHelp.Interface;
using DiaHelp.Services;
using System.Diagnostics;
using System.Web;
using System.Windows.Input;

namespace DiaHelp.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IDatabaseService _databaseService;
        private readonly IWindowService _windowService;
        private readonly AuthService _authService;
        private string _username;
        private string _password;

        public LoginViewModel(IDatabaseService databaseService, IWindowService windowService, AuthService authService)
        {
            _databaseService = databaseService;
            _windowService = windowService;
            _authService = authService;

            LoginCommand = new RelayCommand(Login);
            RegisterCommand = new RelayCommand(NavigateToRegister);
            LoginYandexCommand = new RelayCommand(LoginYandex);
            
        }

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private async void Login(object parameter)
        {
            var user = _databaseService.GetUser(Username);
            if (user != null && BCrypt.Net.BCrypt.Verify(Password, user.Password))
            {
                Preferences.Set("IsUserLoggedIn", true);
                Application.Current.MainPage = _windowService.GetAndCreateContentPage<MainViewModel>().View;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Неверные данные", "OK");
            }
        }

        public async void LoginYandex(object parametr)
        {
            try
            {
                var url = _authService.GetLoginUrl();
                Debug.WriteLine("Open this URL: " + url);  // Логирование URL
                await Launcher.OpenAsync(url);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", ex.Message, "OK");
            }
            
        }

        public async Task HandleRedirectAsync(Uri uri)
        {
            Debug.WriteLine("Handling redirect... URL: " + uri.ToString());

            var query = HttpUtility.ParseQueryString(uri.Query);
            var code = query["code"];

            if (string.IsNullOrEmpty(code))
            {
                Debug.WriteLine("Error: Authorization code is missing.");
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Не удалось получить код авторизации", "OK");
                return;
            }

            Debug.WriteLine("Received code: " + code);

            // Теперь делаем запрос на получение токена
            var tokenResponse = await _authService.GetTokenAsync(code);
            if (tokenResponse != null && !string.IsNullOrEmpty(tokenResponse.AccessToken))
            {
                Debug.WriteLine("Token received: " + tokenResponse.AccessToken);
                Preferences.Set("OAuthToken", tokenResponse.AccessToken);
                Preferences.Set("IsUserLoggedIn", true);

                // Переход на главную страницу
                Application.Current.MainPage = _windowService.GetAndCreateContentPage<MainViewModel>().View;
                Debug.WriteLine("Redirecting to main page.");
            }
            else
            {
                Debug.WriteLine("Error: Token response is null or invalid.");
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Не удалось получить токен", "OK");
            }
        }





        private async void NavigateToRegister(object parametr) => Application.Current.MainPage = _windowService.GetAndCreateContentPage<RegistrationViewModel>().View;

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }
        public ICommand LoginYandexCommand { get; }
    }
}
