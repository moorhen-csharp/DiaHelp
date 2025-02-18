using DiaHelp.Interface;
using DiaHelp.Services;
using DiaHelp.ViewModel;

namespace DiaHelp
{
    public partial class App : Application
    {
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