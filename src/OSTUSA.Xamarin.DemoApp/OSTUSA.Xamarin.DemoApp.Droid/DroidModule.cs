using System;
using Autofac;
using OSTUSA.XamarinDemo.DemoApp.Bootstrapping.Modules;
using OSTUSA.XamarinDemo.Services.Networking.Sockets;
using Xamarin.Forms;

[assembly: Dependency(typeof(OSTUSA.XamarinDemo.DemoApp.Droid.DroidModule))]
namespace OSTUSA.XamarinDemo.DemoApp.Droid
{
    public class DroidModule : PlatformModule
    {
        protected override void Load(Autofac.ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<WebSocketConnection>()
                   .As<IWebSocketConnection>();
        }
    }
}
