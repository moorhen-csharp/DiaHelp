
using DiaHelp.Interface;
using DiaHelp.Model;

namespace DiaHelp.Services
{
    public sealed class WindowService(IServiceProvider serviceProvider) : IWindowService
    {
        private readonly List<WindowModel<ContentPage, BaseViewModel>> _views = new();

        private WindowModel<TView, BaseViewModel> CreateVisualElement<TView, TViewModel>()
            where TView : VisualElement, new()
            where TViewModel : BaseViewModel
        {
            var type = typeof(TViewModel);
            var viewModel = serviceProvider.GetRequiredService<TViewModel>();

            var view = Activator.CreateInstance(Type.GetType($"{type.Namespace.Replace("ViewModel", "View")}.{type.Name.Replace("ViewModel", "View")}")) as TView;
            view.BindingContext = viewModel;

            var window = new WindowModel<TView, BaseViewModel> { View = view, ViewModel = viewModel };

            return window;
        }

        public WindowModel<ContentPage, BaseViewModel> CreateContentView<TViewModel>() where TViewModel : BaseViewModel
        {
            var view = CreateVisualElement<ContentPage, TViewModel>();
            _views.Add(view);
            return view;
        }

        public WindowModel<ContentPage, BaseViewModel> GetView<TViewModel>() where TViewModel : BaseViewModel =>
            _views.FirstOrDefault(p => p.ViewModel.GetType() == typeof(TViewModel)) ?? CreateContentView<TViewModel>();

        public WindowModel<ContentPage, BaseViewModel> GetAndCreateContentPage<TViewModel>() where TViewModel : BaseViewModel => CreateVisualElement<ContentPage, TViewModel>();

    }
}
