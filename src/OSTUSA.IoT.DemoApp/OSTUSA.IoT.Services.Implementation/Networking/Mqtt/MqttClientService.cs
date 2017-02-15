using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSTUSA.IoT.Core.Networking.Sockets.Events;
using OSTUSA.IoT.Core.Networking.Mqtt.Events;
using OSTUSA.IoT.Services.Networking.Mqtt.Messages;
using System.Diagnostics;
using OSTUSA.IoT.Services.Networking.Sockets;

namespace OSTUSA.IoT.Services.Networking.Mqtt
{
    public abstract class MqttClientService : IMqttClientService
    {
        private readonly IWebSocketConnection _webSocketConnection;

        private IMqttClient _client = null;

        protected MqttClientService(IWebSocketConnection webSocketConnection)
        {
            _webSocketConnection = webSocketConnection;
            _webSocketConnection.OnClosed += WebSocketConnection_OnClosed;
        }

        public event EventHandler<PublishReceivedEventArgs> PublishReceived = delegate { };
        public event EventHandler Closed = delegate { };

        public ushort Subscribe(string[] topics, byte[] qosLevels)
        {
            Debug.WriteLine("Subscribing to {0}", string.Join(", ", topics));
            return _client.Subscribe(topics, qosLevels);
        }

        public async Task<ushort> SubscribeAsync(string[] topics, byte[] qosLevels)
        {
            var tcs = new TaskCompletionSource<ushort>();
            ushort messageId = 0;
            MqttMsgSubscribedEventHandler h = null;
            h = (sender, e) =>
            {
                if (e.MessageId == messageId)
                {
                    _client.MqttMsgSubscribed -= h;
                    tcs.SetResult(messageId);
                }
            };

            _client.MqttMsgSubscribed += h;
            messageId = Subscribe(topics, qosLevels);

            return await tcs.Task;
        }

        public ushort Unsubscribe(string[] topics)
        {
            Debug.WriteLine("Unsubscribing from {0}", string.Join(", ", topics));
            return _client.Unsubscribe(topics);
        }

        public virtual byte Connect(string clientId, string url, int port)
        {
            _client = new MqttClient(url, port, _webSocketConnection);
            _client.MqttMsgPublishReceived += MqttClient_PublishReceived;

            return _client.Connect(clientId, null, null, false, MqttMsgConnect.QOS_LEVEL_AT_MOST_ONCE, false, null, null, true, MqttMsgConnect.KEEP_ALIVE_PERIOD_DEFAULT);
        }

        public ushort Publish(string topic, byte[] message, byte qosLevel, bool retain)
        {
            Debug.WriteLine("Publishing to {0} with length {1}", topic, message);
            return _client.Publish(topic, message, qosLevel, retain);
        }

        public ushort Publish(string topic, byte[] message)
        {
            return this.Publish(topic, message, MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);
        }

        public void Disconnect()
        {
            _client?.Disconnect();
        }

        private void WebSocketConnection_OnClosed(object sender, EventArgs e)
        {
            _client.Disconnect();
            Closed(this, EventArgs.Empty);
        }

        private void MqttClient_PublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Message, 0, e.Message.Length);
            PublishReceived(this, new PublishReceivedEventArgs()
            {
                Message = message,
                Topic = e.Topic
            });
        }
    }
}
