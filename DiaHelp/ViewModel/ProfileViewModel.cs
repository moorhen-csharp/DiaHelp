using DiaHelp.Interface;
using System.Windows.Input;

namespace DiaHelp.ViewModel
{
    public class ProfileViewModel : BaseViewModel
    {
        private readonly IWindowService _windowService;

        public ProfileViewModel(IWindowService windowService)
        {
            _windowService = windowService;
            LogoutCommand = new RelayCommand(Logout);
            MainPageCommand = new RelayCommand(MainGo);

        }

        public void LoadData(object parametr)
        {
            //var user = _databaseService.GetAllSugarNotes();
            //SugarNotes.Clear();
            //foreach (var note in notes)
            //{
            //    SugarNotes.Add(note);
            //}
        }

        public void Logout(object parametr)
        {
            Preferences.Set("IsUserLoggedIn", false);
            Application.Current.MainPage = _windowService.GetAndCreateContentPage<LoginViewModel>().View;
        }

        private void MainGo(object parametr) => Application.Current.MainPage = _windowService.GetAndCreateContentPage<MainViewModel>().View;

        public ICommand MainPageCommand { get; }
        public ICommand LogoutCommand { get; }
    }
}
