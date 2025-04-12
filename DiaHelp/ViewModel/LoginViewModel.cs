using DiaHelp.Interface;
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
        private readonly string ClientId = "a13c6e23bebf4cc490e294a57e99cff6";
        private readonly string ClientSecret = "0f2c96fec93e40f6afbedca6feba7674";
        private readonly string RedirectUri = "diahelp:/oauth2redirect";

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
                Preferences.Set("CurrentUsername", user.Username);
                Application.Current.MainPage = _windowService.GetAndCreateContentPage<MainViewModel>().View;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Неверные данные", "OK");
            }
        }

        public async void LoginYandex(object parametr)
        {

            string authUrl = $"https://oauth.yandex.ru/authorize?response_type=code&client_id={ClientId}&redirect_uri={RedirectUri}";
            Debug.WriteLine("Ссылка для авторизации " + authUrl);
            await Launcher.OpenAsync(authUrl);


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

            var tokenResponse = await _authService.GetTokenAsync(code);
            if (tokenResponse != null && !string.IsNullOrEmpty(tokenResponse.AccessToken))
            {
                Debug.WriteLine("Token received: " + tokenResponse.AccessToken);
                Preferences.Set("OAuthToken", tokenResponse.AccessToken);
                Preferences.Set("IsUserLoggedIn", true);

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
