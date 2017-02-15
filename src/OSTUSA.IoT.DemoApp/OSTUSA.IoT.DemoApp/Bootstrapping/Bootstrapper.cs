using System;
using Autofac;
using OSTUSA.IoT.IoC.Modules;
using OSTUSA.IoT.DemoApp.Bootstrapping.Modules;
using Xamarin.Forms;
using OSTUSA.IoT.DemoApp.Navigation;
using OSTUSA.IoT.DemoApp.PageModels;
using OSTUSA.IoT.DemoApp.Pages;

namespace OSTUSA.IoT.DemoApp.Bootstrapping
{
    public class Bootstrapper
    {
        private readonly Application _app;

        public Bootstrapper(Application app)
        {
            _app = app;
        }

        public void Run()
        {
            var container = CreateContainer();

            MapViews(container);
            MapPages(container);

            StartApplication(container);
        }

        private IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<AzureModule>();
            builder.RegisterModule<ViewModule>();
            builder.RegisterModule<ViewModelModule>();

            var platformModule = DependencyService.Get<PlatformModule>();
            builder.RegisterModule(platformModule);

            return builder.Build();
        }

        private void MapViews(IContainer container)
        {
            
        }

        private void MapPages(IContainer container)
        {
            var pageFactory = container.Resolve<IViewFactory<Page>>();

            pageFactory.Register<MainPageModel, MainPage>();
        }

        private void StartApplication(IContainer container)
        {
            var pageFactory = container.Resolve<IViewFactory<Page>>();
            var remotePage = pageFactory.Resolve<MainPageModel>(x => x.OnAppearing(this, EventArgs.Empty));

            _app.MainPage = new NavigationPage(remotePage);
        }
    }
}
