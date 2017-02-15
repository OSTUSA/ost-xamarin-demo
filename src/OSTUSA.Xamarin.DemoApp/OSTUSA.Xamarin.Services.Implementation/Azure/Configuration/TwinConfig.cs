using System;
namespace OSTUSA.XamarinDemo.Services.Azure.Configuration
{
    public class TwinConfig
    {
        private const string ApiVersion = "2016-11-14";

        public string Hostname { get; private set; }
        private readonly string _deviceId;

        public TwinConfig(string hostname, string deviceId)
        {
            Hostname = hostname;
            _deviceId = deviceId;
        }

        public string ClientId => _deviceId;
        public string Username => $"{Hostname}/{_deviceId}/api-version={ApiVersion}";
    }
}
