﻿using DiaHelp.Interface;
using DiaHelp.ViewModel;
using System.Diagnostics;

namespace DiaHelp
{
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App(IWindowService windowService)
        {
            InitializeComponent();


            bool isUserLoggedIn = Preferences.Get("IsUserLoggedIn", false);

            if (isUserLoggedIn)
            {
                // если вошел
                MainPage = windowService.GetAndCreateContentPage<MainViewModel>().View;
            }
            else
            {
                // если не вошел
                MainPage = windowService.GetAndCreateContentPage<LoginViewModel>().View;
            }
        }

        


    }
}
