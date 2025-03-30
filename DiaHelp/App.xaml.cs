using DiaHelp.Interface;
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

        protected override async void OnAppLinkRequestReceived(Uri uri)
        {
            Debug.WriteLine("Received URL: " + uri.ToString());

            if (uri.AbsolutePath == "/oauth2redirect")
            {
                Debug.WriteLine("Redirect URL detected: " + uri.ToString());

                var loginViewModel = _serviceProvider.GetService<LoginViewModel>();
                if (loginViewModel != null)
                {
                    await loginViewModel.HandleRedirectAsync(uri);
                }
                else
                {
                    Debug.WriteLine("LoginViewModel is null");
                }
            }
            else
            {
                Debug.WriteLine("Non-matching URL: " + uri.ToString());
            }

            base.OnAppLinkRequestReceived(uri);
        }


    }
}
