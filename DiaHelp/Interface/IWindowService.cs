using DiaHelp.Model;

namespace DiaHelp.Interface
{
    public interface IWindowService
    {
        WindowModel<ContentPage, BaseViewModel> CreateContentView<TviewModel>() where TviewModel : BaseViewModel;
        WindowModel<ContentPage, BaseViewModel> GetView<TViewModel>() where TViewModel : BaseViewModel;
        WindowModel<ContentPage, BaseViewModel> GetAndCreateContentPage<TViewModel>() where TViewModel : BaseViewModel;
    }
}
