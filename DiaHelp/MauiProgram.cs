using DiaHelp.Infrastructure;
using DiaHelp.Interface;
using DiaHelp.Services;
using DiaHelp.View;
using DiaHelp.ViewModel;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Platform;

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
            builder.Services.AddDbContext<ApplicationContext>();
            builder.Services.AddSingleton<IDatabaseService, DatabaseService>();

            // Регистрация ViewModels
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<RegistrationViewModel>();
            builder.Services.AddTransient<MainViewModel>();

            // Регистрация страниц
            builder.Services.AddTransient<LoginView>();
            builder.Services.AddTransient<RegistrationView>();
            
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
