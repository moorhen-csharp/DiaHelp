using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DiaHelp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiaHelp.ViewModel
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly UserService _userService;

        [ObservableProperty]
        private string _username;

        [ObservableProperty]
        private string _password;

        public LoginViewModel(UserService userService)
        {
            _userService = userService;
        }

        [RelayCommand]
        private async Task Login()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                await Shell.Current.DisplayAlert("Ошибка", "Заполните все поля", "OK");
                return;
            }

            var user = await _userService.AuthenticateUser(Username, Password);
            if (user != null)
            {
                await Shell.Current.GoToAsync("//MainPage");
                Username = string.Empty;
                Password = string.Empty;
            }
            else
            {
                await Shell.Current.DisplayAlert("Ошибка", "Неверные учетные данные", "OK");
            }
        }

        [RelayCommand]
        private async Task NavigateToRegister()
        {
            await Shell.Current.GoToAsync("RegistrationView");
        }

        
    }
}
