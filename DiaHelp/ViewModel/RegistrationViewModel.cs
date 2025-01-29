using DiaHelp.Interface;
using DiaHelp.Model;
using DiaHelp.Services;
using System.Threading.Tasks;
using System.Windows.Input;

public partial class RegistrationViewModel : BaseViewModel
{
    private readonly IDatabaseService _databaseService;
    private string _email;
    private string _password;
    private string _confirmPassword;

    

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

    public RegistrationViewModel(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
        RegisterCmnd = new RelayCommand(async _ => await Register());
    }

    private async Task Register()
    {
        if (Password != ConfirmPassword)
        {
            await Shell.Current.DisplayAlert("Ошибка", "Пароли не совпадают", "OK");
            return;
        }

        var newUser = new User
        {
            Email = Email,
            Password = BCrypt.Net.BCrypt.HashPassword(Password),
            Username = Email,
            RegistrationDate = DateTime.UtcNow
        };

        if (_databaseService.AddUser(newUser))
        {
            await Shell.Current.DisplayAlert("Успех", "Регистрация завершена", "OK");
            await Shell.Current.GoToAsync(".."); // Возврат к странице входа
        }
        else
        {
            await Shell.Current.DisplayAlert("Ошибка", "Не удалось зарегистрироваться", "OK");
        }
    }
    public ICommand RegisterCmnd { get; }
}