using DiaHelp.Interface;
using DiaHelp.ViewModel;

namespace DiaHelp
{
    public partial class App : Application
    {
        public App(IWindowService windowService)
        {
            InitializeComponent();
            MainPage = windowService.GetAndCreateContentPage<LoginViewModel>().View;
        }

        
    }
}