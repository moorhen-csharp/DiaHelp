﻿using DiaHelp.Infrastructure;
using DiaHelp.Interface;
using DiaHelp.Services;
using DiaHelp.View;
using DiaHelp.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DiaHelp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Настройка базы данных с использованием SQLite
            builder.Services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseSqlite($"FileName={Path.Combine(FileSystem.AppDataDirectory, "DiaHelpTest.db")}");
            });

            // Регистрация сервисов
            builder.Services.AddSingleton<IDatabaseService, DatabaseService>();
            builder.Services.AddTransient<IWindowService, WindowService>();
            builder.Services.AddSingleton<AuthService>();

            // Регистрация ViewModels
            builder.Services.AddTransient<MainViewModel>();
            builder.Services.AddTransient<RegistrationViewModel>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<SugarNoteViewModel>();
            builder.Services.AddTransient<ProfileViewModel>();
            builder.Services.AddTransient<SugarAverageLevelViewModel>();
            builder.Services.AddTransient<SugarEntryViewModel>();
            builder.Services.AddTransient<BreadUnitViewModel>();

            // Регистрация страниц
            builder.Services.AddTransient<MainView>();
            builder.Services.AddTransient<RegistrationView>();
            builder.Services.AddTransient<LoginView>();
            builder.Services.AddTransient<SugarNoteView>();
            builder.Services.AddTransient<ProfileView>();
            builder.Services.AddTransient<SugarAverageLevelView>();
            builder.Services.AddTransient<SugarEntryView>();
            builder.Services.AddTransient<BreadUnitView>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
