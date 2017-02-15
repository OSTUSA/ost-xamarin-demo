using System;
using Autofac;
using OSTUSA.IoT.DemoApp.Bootstrapping.Modules;
using OSTUSA.IoT.Services.Networking.Sockets;
using Xamarin.Forms;

[assembly: Dependency(typeof(OSTUSA.IoT.DemoApp.Droid.DroidModule))]
namespace OSTUSA.IoT.DemoApp.Droid
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
