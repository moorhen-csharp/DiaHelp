using DiaHelp.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DiaHelp.ViewModel
{
    class DiabetesLessonOneViewModel : BaseViewModel
    {
        private IWindowService _windowService;

        public DiabetesLessonOneViewModel(IWindowService windowService) 
        {
            _windowService = windowService;
            BackPage = new RelayCommand(LessonSyringePen);
            MainPage = new RelayCommand(MainGo);
        }

        public void LessonSyringePen(object parametr) => Application.Current.MainPage = _windowService.GetAndCreateContentPage<LessonSyringePenViewModel>().View;
        public void MainGo(object paremetr) => Application.Current.MainPage = _windowService.GetAndCreateContentPage<MainViewModel>().View;


        public ICommand BackPage { get; }
        public ICommand MainPage { get; }
    }
}
