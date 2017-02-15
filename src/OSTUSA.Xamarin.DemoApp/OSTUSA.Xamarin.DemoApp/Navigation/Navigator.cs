using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSTUSA.XamarinDemo.Core.ViewModels;
using Xamarin.Forms;

namespace OSTUSA.XamarinDemo.DemoApp.Navigation
{
    public class Navigator : INavigator
    {
        private readonly Lazy<INavigation> _mainNavigation;
        private readonly IViewFactory<Page> _pageFactory;

        private Stack<INavigation> _navStack;

        public Navigator(Lazy<INavigation> mainNavigation, IViewFactory<Page> pageFactory)
        {
            _mainNavigation = mainNavigation;
            _pageFactory = pageFactory;

            _navStack = new Stack<INavigation>();
        }

        private INavigation Navigation
        {
            get { return _navStack.Any() ? _navStack.Peek() : _mainNavigation.Value; }
        }

        public IPageModel Peek()
        {
            if (!(Navigation?.NavigationStack?.Any()).GetValueOrDefault())
                return null;

            var view = Navigation.NavigationStack.Last();
            return view.BindingContext as IPageModel;
        }

        public async Task<IPageModel> PopAsync(bool animated = true)
        {
            var view = await Navigation.PopAsync(animated);
            return view.BindingContext as IPageModel;
        }

        public async Task<IPageModel> PopModalAsync(bool animated = true)
        {
            // pop the nav from the stack
            _navStack.Pop();

            // pop the modal
            await Navigation.PopModalAsync(animated);
            var page = Navigation.NavigationStack.Last();

            var pageModel = page.BindingContext as IPageModel;
            return pageModel;
        }

        public async Task<IPageModel> PopToRootAsync(bool animated = true)
        {
            await Navigation.PopToRootAsync(animated);

            var page = Navigation.NavigationStack.Last();

            var pageModel = page.BindingContext as IPageModel;
            return pageModel;
        }

        public TPageModel Insert<TPageModel>(TPageModel viewModel)
            where TPageModel : class, IPageModel
        {
            var view = _pageFactory.Resolve<TPageModel>(viewModel);

            Initialize(view, viewModel);

            Navigation.InsertPageBefore(view, Navigation.NavigationStack.Last());

            return viewModel;
        }

        public TPageModel InsertAtIndex<TPageModel>(int index)
            where TPageModel : class, IPageModel
        {
            if (index >= Navigation.NavigationStack.Count)
                throw new IndexOutOfRangeException();

            TPageModel model;
            var view = _pageFactory.Resolve<TPageModel>(out model);
            Initialize(view, model);

            Navigation.InsertPageBefore(view, Navigation.NavigationStack[index]);

            return model;
        }

        public async Task<TPageModel> PushAsync<TPageModel>(Action<TPageModel> setStateAction = null, bool animated = true)
            where TPageModel : class, IPageModel
        {
            TPageModel viewModel;
            var view = _pageFactory.Resolve<TPageModel>(out viewModel, setStateAction);

            Initialize(view, viewModel);

            await Navigation.PushAsync(view, animated);
            return viewModel;
        }

        public async Task<TPageModel> PushAsync<TPageModel>(TPageModel viewModel, bool animated = true)
            where TPageModel : class, IPageModel
        {
            var view = _pageFactory.Resolve(viewModel);

            Initialize(view, viewModel);

            await Navigation.PushAsync(view, animated);
            return viewModel;
        }

        public async Task<TPageModel> PushModalAsync<TPageModel>(Action<TPageModel> setStateAction = null, bool animated = true)
            where TPageModel : class, IPageModel
        {
            TPageModel viewModel;
            var view = _pageFactory.Resolve<TPageModel>(out viewModel, setStateAction);

            Initialize(view, viewModel);

            var nav = new NavigationPage(view);
            await Navigation.PushModalAsync(nav, animated);
            _navStack.Push(nav.Navigation);

            return viewModel;
        }

        public async Task<TPageModel> PushModalAsync<TPageModel>(TPageModel viewModel, bool animated = true)
            where TPageModel : class, IPageModel
        {
            var view = _pageFactory.Resolve(viewModel);

            Initialize(view, viewModel);

            var nav = new NavigationPage(view);
            await Navigation.PushModalAsync(nav, animated);
            _navStack.Push(nav.Navigation);

            return viewModel;
        }

        public TPageModel RemoveFromStack<TPageModel>(TPageModel viewModel)
            where TPageModel : class, IPageModel
        {
            var page = Navigation.NavigationStack.FirstOrDefault(x => x.BindingContext == viewModel);
            if (page != null)
                Navigation.RemovePage(page);

            return viewModel;
        }

        private void Initialize(Page page, IPageModel pageModel)
        {
            page.Appearing += Page_Appearing;
            page.Appearing += pageModel.OnAppearing;

            page.Disappearing += Page_Disappearing;
            page.Disappearing += pageModel.OnDisappearing;
        }

        void Page_Appearing(object sender, EventArgs e)
        {

        }

        void Page_Disappearing(object sender, EventArgs e)
        {
            var page = sender as Page;
            if (page == null)
                return;

            Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromMilliseconds(100));

                if (CanDisposePage(page))
                {
                    // un-bind page lifecycle events
                    page.Appearing -= Page_Appearing;
                    page.Disappearing -= Page_Disappearing;

                    var viewModel = page.BindingContext as IPageModel;
                    if (viewModel != null)
                    {
                        page.Appearing -= viewModel.OnAppearing;
                        page.Disappearing -= viewModel.OnDisappearing;

                        viewModel.Dispose();
                    }
                }
            });
        }

        private bool CanDisposePage(Page page)
        {
            foreach (var nav in _navStack.Reverse())
            {
                if (nav.NavigationStack.Contains(page) || nav.ModalStack.Contains(page))
                    return false;
            }

            var mainNav = _mainNavigation.Value;
            return !(mainNav.NavigationStack.Contains(page) || mainNav.ModalStack.Contains(page));
        }
    }
}
