using System;
using System.ComponentModel;
namespace OSTUSA.XamarinDemo.Core.ViewModels
{
    public interface IViewModel : INotifyPropertyChanged, IDisposable
    {
        void SetState<T>(Action<T> action)
            where T : class, IViewModel;
    }
}
