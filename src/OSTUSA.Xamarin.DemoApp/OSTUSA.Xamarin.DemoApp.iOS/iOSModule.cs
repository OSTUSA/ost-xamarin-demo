using System;
using OSTUSA.XamarinDemo.DemoApp.Bootstrapping.Modules;
using Xamarin.Forms;
using Autofac;
using OSTUSA.XamarinDemo.Services.Azure;
using OSTUSA.XamarinDemo.Services.Networking.Sockets;

[assembly: Dependency(typeof(OSTUSA.XamarinDemo.DemoApp.iOS.iOSModule))]
namespace OSTUSA.XamarinDemo.DemoApp.iOS
{
    public class iOSModule : PlatformModule
    {
        protected override void Load(Autofac.ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<WebSocketConnection>()
                   .As<IWebSocketConnection>();
        }
    }
}
