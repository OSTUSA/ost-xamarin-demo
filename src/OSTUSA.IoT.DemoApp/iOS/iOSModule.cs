using System;
using OSTUSA.IoT.DemoApp.Bootstrapping.Modules;
using Xamarin.Forms;
using Autofac;
using OSTUSA.IoT.Services.Azure;
using OSTUSA.IoT.Services.Networking.Sockets;

[assembly: Dependency(typeof(OSTUSA.IoT.DemoApp.iOS.iOSModule))]
namespace OSTUSA.IoT.DemoApp.iOS
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
