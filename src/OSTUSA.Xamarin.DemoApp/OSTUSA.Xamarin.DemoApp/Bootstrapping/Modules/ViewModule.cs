using System;
using Autofac;
using OSTUSA.XamarinDemo.DemoApp.Navigation;
using OSTUSA.XamarinDemo.DemoApp.Pages;
using Xamarin.Forms;

namespace OSTUSA.XamarinDemo.DemoApp.Bootstrapping.Modules
{
    public class ViewModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<ViewFactory<Page>>()
                .As<IViewFactory<Page>>()
                .SingleInstance();

            builder.RegisterType<Navigator>()
                .As<INavigator>()
                .SingleInstance();
            
            builder.Register<INavigation>(context => Application.Current.MainPage.Navigation)
                .SingleInstance();

            builder.Register<Page>(context => Application.Current.MainPage);

            LoadViews(builder);
            LoadPages(builder);
        }

        private void LoadViews(ContainerBuilder builder)
        {
            
        }
        
        private void LoadPages(ContainerBuilder builder)
        {
            builder.RegisterType<MainPage>();
        }
    }
}
