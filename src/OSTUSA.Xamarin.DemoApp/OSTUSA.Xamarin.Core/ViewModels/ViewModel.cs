using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OSTUSA.XamarinDemo.Core.ViewModels
{
    public abstract class ViewModel
    {
        public virtual event PropertyChangedEventHandler PropertyChanged;

        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);

            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void Dispose()
        {
            PropertyChanged = null;
        }

        public void SetState<T>(Action<T> action)
            where T : class, IViewModel
        {
            action(this as T);
        }
    }
}
