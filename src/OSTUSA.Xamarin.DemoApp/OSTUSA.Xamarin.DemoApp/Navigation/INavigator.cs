using System;
using System.Threading.Tasks;
using OSTUSA.XamarinDemo.Core.ViewModels;

namespace OSTUSA.XamarinDemo.DemoApp.Navigation
{
    public interface INavigator
    {
        IPageModel Peek();
        Task<IPageModel> PopAsync(bool animated = true);
        Task<IPageModel> PopModalAsync(bool animated = true);
        Task<IPageModel> PopToRootAsync(bool animated = true);

        TPageModel Insert<TPageModel>(TPageModel viewModel)
            where TPageModel : class, IPageModel;

        TPageModel InsertAtIndex<TPageModel>(int index)
            where TPageModel : class, IPageModel;

        Task<TViewModel> PushAsync<TViewModel>(Action<TViewModel> setStateAction = null, bool animated = true)
            where TViewModel : class, IPageModel;
        Task<TViewModel> PushAsync<TViewModel>(TViewModel viewModel, bool animated = true)
            where TViewModel : class, IPageModel;
        Task<TViewModel> PushModalAsync<TViewModel>(Action<TViewModel> setStateAction = null, bool animated = true)
            where TViewModel : class, IPageModel;
        Task<TViewModel> PushModalAsync<TViewModel>(TViewModel viewModel, bool animated = true)
            where TViewModel : class, IPageModel;

        TViewModel RemoveFromStack<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IPageModel;
    }
}
