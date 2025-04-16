using DiaHelp.Interface;
using System.Windows.Input;

namespace DiaHelp.ViewModel
{
    class DiabetesSchoolViewModel : BaseViewModel
    {
        private readonly IWindowService _windowService;

        public DiabetesSchoolViewModel(IWindowService windowService)
        {
            _windowService = windowService;
            DiabetesLessonSyringePenCommand = new RelayCommand(LessonSyringePen);
            MainPage = new RelayCommand(MainGo);
        }

        public void LessonSyringePen(object parametr) => Application.Current.MainPage = _windowService.GetAndCreateContentPage<LessonSyringePenViewModel>().View;
        public void MainGo(object paremetr) => Application.Current.MainPage = _windowService.GetAndCreateContentPage<MainViewModel>().View;



        public ICommand DiabetesLessonSyringePenCommand { get; }
        public ICommand MainPage { get; }
    }
}
