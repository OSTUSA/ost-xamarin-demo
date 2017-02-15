using System;
namespace OSTUSA.IoT.Core.ViewModels
{
    public interface IPageModel : IViewModel
    {
        void OnAppearing(object sender, EventArgs e);
        void OnDisappearing(object sender, EventArgs e);
    }
}
