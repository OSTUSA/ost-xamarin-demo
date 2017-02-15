using System;
namespace OSTUSA.XamarinDemo.Core.Networking.Mqtt.Events
{
    public class PublishReceivedEventArgs : EventArgs
    {
        public string Topic { get; set; }
        public string Message { get; set; }
    }
}

