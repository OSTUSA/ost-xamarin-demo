using System;
namespace OSTUSA.XamarinDemo.Core.ViewModels
{
    public abstract class PageModel : ViewModel, IPageModel
    {
        public virtual void OnAppearing(object sender, EventArgs e)
        {
            
        }

        public virtual void OnDisappearing(object sender, EventArgs e)
        {
            
        }
    }
}
