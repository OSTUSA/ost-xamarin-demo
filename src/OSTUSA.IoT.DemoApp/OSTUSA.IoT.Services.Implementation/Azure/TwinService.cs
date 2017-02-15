using System;
using OSTUSA.IoT.Services.Networking.Mqtt;
using OSTUSA.IoT.Services.Azure.Configuration;
using OSTUSA.IoT.Services.Networking;
using OSTUSA.IoT.Services.Networking.Mqtt.Messages;
using OSTUSA.IoT.Services.Networking.Sockets;

namespace OSTUSA.IoT.Services.Azure
{
    public class TwinService : ITwinService
    {
        private readonly IMqttClient _mqttClient;

        private const string ApiVersion = "2016-11-14";
        private const string Hostname = "iot-sample-hub.azure-devices.net";
        private const string DeviceId = "myraspberrypi";

        public TwinService(IWebSocketConnection webSocketConnection)
        {
            var uri = $"wss://{Hostname}:443/$iothub/websocket";
            _mqttClient = new MqttClient(uri, 443, webSocketConnection);
        }

        public void Open()
        {
            var clientId = DeviceId;
            var username = $"{Hostname}/{DeviceId}/DeviceClientType=azure-iot-device%2F1.1.6&api-version=${ApiVersion}";
            string password = null;
            
            System.Diagnostics.Debug.WriteLine("Attempting to open MQTT connection");
            _mqttClient.Connect(clientId, username, password, false, MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, false, null, null, false, MqttMsgConnect.KEEP_ALIVE_PERIOD_DEFAULT);
        }
    }
}
