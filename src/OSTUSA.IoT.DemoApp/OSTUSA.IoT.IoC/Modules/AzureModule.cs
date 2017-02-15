using System;
using Autofac;
using OSTUSA.IoT.Services.Azure;
using OSTUSA.IoT.Services.Azure.Configuration;
namespace OSTUSA.IoT.IoC.Modules
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
