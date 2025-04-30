using DiaHelp.Infrastructure;
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
                    fonts.AddFont("Comfortaa-VariableFont_wght.ttf", "Comfortaa");
                });

            builder.Services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseSqlite($"FileName={Path.Combine(FileSystem.AppDataDirectory, "DiaHelpTest.db")}");
            });

            //Services
            builder.Services.AddSingleton<IDatabaseService, DatabaseService>();
            builder.Services.AddTransient<IWindowService, WindowService>();
            builder.Services.AddSingleton<AuthService>();
            builder.Services.AddSingleton<IAiChatService, AiChatService>();

            //ViewModel
            builder.Services.AddTransient<MainViewModel>();
            builder.Services.AddTransient<RegistrationViewModel>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<SugarNoteViewModel>();
            builder.Services.AddTransient<ProfileViewModel>();
            builder.Services.AddTransient<SugarEntryViewModel>();
            builder.Services.AddTransient<BreadUnitViewModel>();
            builder.Services.AddTransient<DiabetesSchoolViewModel>();
            builder.Services.AddTransient<LessonSyringePenViewModel>();
            builder.Services.AddTransient<DiabetesLessonOneViewModel>();
            builder.Services.AddTransient<AiChatViewModel>();

            //View
            builder.Services.AddTransient<MainView>();
            builder.Services.AddTransient<RegistrationView>();
            builder.Services.AddTransient<LoginView>();
            builder.Services.AddTransient<SugarNoteView>();
            builder.Services.AddTransient<ProfileView>();
            builder.Services.AddTransient<SugarEntryView>();
            builder.Services.AddTransient<BreadUnitView>();
            builder.Services.AddTransient<DiabetesSchoolView>();
            builder.Services.AddTransient<LessonSyringePenView>();
            builder.Services.AddTransient<DiabetesLessonOneView>();
            builder.Services.AddTransient<AiChatView>();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
