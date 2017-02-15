using System;
using Autofac;
using OSTUSA.XamarinDemo.DemoApp.PageModels;

namespace OSTUSA.XamarinDemo.DemoApp.Bootstrapping.Modules
{
    public class ViewModelModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            LoadViewModels(builder);
            LoadPageModels(builder);
        }

        private void LoadViewModels(ContainerBuilder builder)
        {
            
        }

        private void LoadPageModels(ContainerBuilder builder)
        {
            builder.RegisterType<MainPageModel>();
        }
    }
}
