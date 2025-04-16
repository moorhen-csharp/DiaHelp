using DiaHelp.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DiaHelp.ViewModel
{
    class LessonSyringePenViewModel : BaseViewModel
    {
        private IWindowService _windowService;

        public LessonSyringePenViewModel(IWindowService windowService) 
        {
            _windowService = windowService;
            DiabetesLessonOneCommand = new RelayCommand(LessonOne);
            MainPage = new RelayCommand(MainGo);
        }

        public void LessonOne(object parametr) => Application.Current.MainPage = _windowService.GetAndCreateContentPage<DiabetesLessonOneViewModel>().View;
        public void MainGo(object paremetr) => Application.Current.MainPage = _windowService.GetAndCreateContentPage<MainViewModel>().View;


        public ICommand DiabetesLessonOneCommand { get; }
        public ICommand MainPage { get; }
    }
}
