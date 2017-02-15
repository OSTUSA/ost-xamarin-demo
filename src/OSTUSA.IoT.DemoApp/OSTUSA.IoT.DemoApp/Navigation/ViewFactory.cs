using System;
using System.Collections.Generic;
using Autofac;
using OSTUSA.IoT.Core.ViewModels;
using Xamarin.Forms;

namespace OSTUSA.IoT.DemoApp.Navigation
{
    public class ViewFactory<T> : IViewFactory<T>
        where T : VisualElement
    {
        private readonly IDictionary<Type, Type> _map;
        private readonly IComponentContext _componentContext;

        public ViewFactory(IComponentContext componentContext)
        {
            _componentContext = componentContext;

            _map = new Dictionary<Type, Type>();
        }

        public void Register<TViewModel, TView>()
            where TViewModel : class, IViewModel
            where TView : T
        {
            _map[typeof(TViewModel)] = typeof(TView);
        }

        public T Resolve<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
        {
            TViewModel viewModel;
            return Resolve<TViewModel>(out viewModel, setStateAction);
        }

        public T Resolve<TViewModel>(out TViewModel viewModel, Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
        {
            viewModel = _componentContext.Resolve<TViewModel>();

            var viewType = _map[viewModel.GetType()];
            T view = _componentContext.Resolve(viewType) as T;

            if (setStateAction != null)
            {
                viewModel.SetState(setStateAction);
            }

            view.BindingContext = viewModel;

            return view;
        }

        public T Resolve<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel
        {
            try
            {
                var viewType = _map[viewModel.GetType()];

                T view = _componentContext.Resolve(viewType) as T;
                view.BindingContext = viewModel;

                return view;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
