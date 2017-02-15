using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSTUSA.IoT.Core.Networking.Mqtt.Events;

namespace OSTUSA.IoT.Services.Networking.Mqtt
{
    public interface IMqttClient
    {
        byte Connect(string clientId,
            string username,
            string password,
            bool willRetain,
            byte willQosLevel,
            bool willFlag,
            string willTopic,
            string willMessage,
            bool cleanSession,
            ushort keepAlivePeriod);

        ushort Subscribe(string[] topics, byte[] qosLevels);

        ushort Unsubscribe(string[] topics);

        ushort Publish(string topic, byte[] message);

        ushort Publish(string topic, byte[] message, byte qosLevel, bool retain);

        void Disconnect();

        // event for PUBLISH message received
        event MqttMsgPublishEventHandler MqttMsgPublishReceived;
        // event for published message
        event MqttMsgPublishedEventHandler MqttMsgPublished;
        // event for subscribed topic
        event MqttMsgSubscribedEventHandler MqttMsgSubscribed;
        // event for unsubscribed topic
        event MqttMsgUnsubscribedEventHandler MqttMsgUnsubscribed;

        // event for peer/client disconnection
        event ConnectionClosedEventHandler ConnectionClosed;
    }

    /// <summary>
    /// Delagate that defines event handler for PUBLISH message received
    /// </summary>
    public delegate void MqttMsgPublishEventHandler(object sender, MqttMsgPublishEventArgs e);

    /// <summary>
    /// Delegate that defines event handler for published message
    /// </summary>
    public delegate void MqttMsgPublishedEventHandler(object sender, MqttMsgPublishedEventArgs e);

    /// <summary>
    /// Delagate that defines event handler for subscribed topic
    /// </summary>
    public delegate void MqttMsgSubscribedEventHandler(object sender, MqttMsgSubscribedEventArgs e);

    /// <summary>
    /// Delagate that defines event handler for unsubscribed topic
    /// </summary>
    public delegate void MqttMsgUnsubscribedEventHandler(object sender, MqttMsgUnsubscribedEventArgs e);

    /// <summary>
    /// Delegate that defines event handler for cliet/peer disconnection
    /// </summary>
    public delegate void ConnectionClosedEventHandler(object sender, EventArgs e);
}
