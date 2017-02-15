using System;
namespace OSTUSA.IoT.Core.Networking.Mqtt.Events
{
    public class PublishReceivedEventArgs : EventArgs
    {
        public string Topic { get; set; }
        public string Message { get; set; }
    }
}

