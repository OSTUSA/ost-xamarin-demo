using System;
using OSTUSA.IoT.Core.ViewModels;
using Xamarin.Forms;

namespace OSTUSA.IoT.DemoApp.Navigation
{
    public interface IViewFactory<T>
        where T : VisualElement
    {
        void Register<TViewModel, TView>()
            where TViewModel : class, IViewModel
            where TView : T;

        T Resolve<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel;

        T Resolve<TViewModel>(out TViewModel viewModel, Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel;

        T Resolve<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel;
    }
}
