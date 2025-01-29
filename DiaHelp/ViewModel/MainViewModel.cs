using CommunityToolkit.Mvvm.ComponentModel;
using DiaHelp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiaHelp.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string _welcomeMessage;

        public MainViewModel()
        {
            var username = Preferences.Get("Username", string.Empty);
            WelcomeMessage = $"Добро пожаловать, {username}!";
        }
    }
}
