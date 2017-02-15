using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSTUSA.XamarinDemo.Core.Networking.Mqtt.Events;

namespace OSTUSA.XamarinDemo.Services.Networking.Mqtt
{
    public interface IMqttClientService
    {
        byte Connect(string clientId, string url, int port);
        void Disconnect();

        ushort Subscribe(string[] topics, byte[] qosLevels);

        Task<ushort> SubscribeAsync(string[] topics, byte[] qosLevels);

        ushort Unsubscribe(string[] topics);

        ushort Publish(string topic, byte[] message);

        ushort Publish(string topic, byte[] message, byte qosLevel, bool retain);

        event EventHandler<PublishReceivedEventArgs> PublishReceived;
        event EventHandler Closed;
    }
}
