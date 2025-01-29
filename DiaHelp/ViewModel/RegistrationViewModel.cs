using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DiaHelp.Base;
using DiaHelp.Interface;
using DiaHelp.Model;

public partial class RegistrationViewModel : BaseViewModel
{
    private readonly IDatabaseService _databaseService;


    private string _email;


    private string _password;


    private string _confirmPassword;

    public RegistrationViewModel(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged(nameof(Password));
        }
    }
    public string ConfirmPassword
    {
        get => _confirmPassword;
        set
        {
            _confirmPassword = value;
            OnPropertyChanged(nameof(ConfirmPassword));
        }
    }
    [RelayCommand]
    private async Task Register()
    {
        if (Password != ConfirmPassword)
        {
            await Shell.Current.DisplayAlert("Ошибка", "Пароли не совпадают", "OK");
            return;
        }

        var newUser = new User
        {
            //Email = Email,
            Password = BCrypt.Net.BCrypt.HashPassword(Password),
            //Username = Email,
            RegistrationDate = DateTime.UtcNow
        };

        if (_databaseService.AddUser(newUser))
        {
            await Shell.Current.DisplayAlert("Успех", "Регистрация завершена", "OK");
            await Shell.Current.GoToAsync(".."); // Возврат на предыдущую страницу (Login)
        }
        else
        {
            await Shell.Current.DisplayAlert("Ошибка", "Регистрация не удалась", "OK");
        }
    }
}
