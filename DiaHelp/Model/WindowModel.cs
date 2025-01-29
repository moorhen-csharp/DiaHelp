
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiaHelp.Model
{
    public class WindowModel<TView, TViewModel>
         where TView : VisualElement
         where TViewModel : BaseViewModel, new()
    {
        public required TView View { get; set; }
        public required TViewModel ViewModel { get; set; }
    }
}
