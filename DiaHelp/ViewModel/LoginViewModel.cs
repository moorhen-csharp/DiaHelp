using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DiaHelp.Base;
using DiaHelp.Interface;
using DiaHelp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DiaHelp.ViewModel
{
    public partial class LoginViewModel : BaseViewModel
    {
        private readonly IDatabaseService _databaseService;
        private string _loginText;
        private string _passwordText;

        public LoginViewModel(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }
        public string LoginText
        {
            get => _loginText;
            set
            {
                _loginText = value;
                OnPropertyChanged(nameof(LoginText));
            }
        }

        public string PasswordText
        {
            get => _passwordText;
            set
            {
                _passwordText = value;
                OnPropertyChanged(nameof(PasswordText));
            }
        }

        private async Task Login()
        {
            if (string.IsNullOrEmpty(LoginText) || string.IsNullOrEmpty(PasswordText))
            {
                await Shell.Current.DisplayAlert("Ошибка", "Заполните все поля", "OK");
                return;
            }

            var user = _databaseService.GetUser(LoginText);

            if (user != null && BCrypt.Net.BCrypt.Verify(PasswordText, user.Password))
            {
                Preferences.Set("IsLoggedIn", true);
                await Shell.Current.GoToAsync("//MainPage");
            }
            else
            {
                await Shell.Current.DisplayAlert("Ошибка", "Неверные учетные данные", "OK");
            }
        }

        [RelayCommand]
        private async Task NavigateToRegistration()
        {
            await Shell.Current.GoToAsync(nameof(RegistrationViewModel));
        }
    }
}
