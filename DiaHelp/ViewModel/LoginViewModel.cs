using DiaHelp.Interface;
using DiaHelp.View;
using System.Windows.Input;

public class LoginViewModel : BaseViewModel
{
    private readonly IDatabaseService _databaseService;
    private string _username;
    private string _password;

    public ICommand LoginCommand { get; }
    public ICommand RegisterCommand { get; }

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

    public LoginViewModel(IDatabaseService databaseService)
    {
        _databaseService = databaseService;

        LoginCommand = new RelayCommand(async _ => await Login());
        RegisterCommand = new RelayCommand(async _ => await NavigateToRegister());
    }

    public LoginViewModel()
    {
    }

    private async Task Login()
    {
        var user = _databaseService.GetUser(Username);
        if (user != null && BCrypt.Net.BCrypt.Verify(Password, user.Password))
        {
            await Shell.Current.GoToAsync("//MainPage");
        }
        else
        {
            await Shell.Current.DisplayAlert("Ошибка", "Неверные данные", "OK");
        }
    }

    private async Task NavigateToRegister()
    {
        await Shell.Current.GoToAsync(nameof(RegistrationView));
    }
}