using System;
using Autofac;
using OSTUSA.XamarinDemo.Services.Azure;
using OSTUSA.XamarinDemo.Services.Azure.Configuration;
namespace OSTUSA.XamarinDemo.IoC.Modules
{
    public class AzureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<MessageService>()
                .As<IMessageService>();

            builder.RegisterType<TwinConfig>()
                   .WithParameter("hostname", "iot-sample-hub.azure-devices.net")
                   .WithParameter("deviceId", "myraspberrypi");

            builder.RegisterType<TwinService>()
                   .As<ITwinService>();
        }
    }
}
