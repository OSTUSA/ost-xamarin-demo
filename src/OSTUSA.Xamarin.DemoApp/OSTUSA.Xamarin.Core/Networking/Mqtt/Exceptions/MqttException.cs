using System;
namespace OSTUSA.XamarinDemo.Core.Networking.Mqtt.Exceptions
{
    public class MqttException : Exception
    {
        public MqttException()
        {
        }

        public MqttException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
