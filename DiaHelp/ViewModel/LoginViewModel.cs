﻿using DiaHelp.Interface;
using DiaHelp.Model;
using System.Windows.Input;

namespace DiaHelp.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IDatabaseService _databaseService;
        private readonly IWindowService _windowService;
        private string _username;
        private string _password;
        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }

        public LoginViewModel(IDatabaseService databaseService, IWindowService windowService)
        {
            _databaseService = databaseService;
            _windowService = windowService;
            LoginCommand = new RelayCommand(async _ => await Login());
            RegisterCommand = new RelayCommand(async _ => await NavigateToRegister());
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


        private async Task Login()
        {
            var user = _databaseService.GetUser(Username);
            if (user != null && BCrypt.Net.BCrypt.Verify(Password, user.Password))
            {
                Application.Current.MainPage = _windowService.GetAndCreateContentPage<MainViewModel>().View;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Неверные данные", "OK");
            }
        }

        private async Task NavigateToRegister() => Application.Current.MainPage = _windowService.GetAndCreateContentPage<RegistrationViewModel>().View;
    }
}