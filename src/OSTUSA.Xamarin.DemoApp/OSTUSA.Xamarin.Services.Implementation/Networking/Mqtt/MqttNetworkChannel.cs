/*
Copyright (c) 2013, 2014 Paolo Patierno

All rights reserved. This program and the accompanying materials
are made available under the terms of the Eclipse Public License v1.0
and Eclipse Distribution License v1.0 which accompany this distribution. 

The Eclipse Public License is available at 
   http://www.eclipse.org/legal/epl-v10.html
and the Eclipse Distribution License is available at 
   http://www.eclipse.org/org/documents/edl-v10.php.

Contributors:
   Paolo Patierno - initial API and implementation and/or initial documentation
*/

using System;
using System.Linq;
using OSTUSA.XamarinDemo.Core.Networking.Mqtt;
using OSTUSA.XamarinDemo.Core.Networking.Sockets.Events;
using OSTUSA.XamarinDemo.Services.Networking.Sockets;
using System.Threading.Tasks;

namespace OSTUSA.XamarinDemo.Services.Networking.Mqtt
{
    public class MqttNetworkChannel : IMqttNetworkChannel
    {
        private IWebSocketConnection _webSocketConnection;

        // stream socket for communication
        //        private StreamSocket socket;

        // remote host information
        private string _url;
        private int _remotePort;

        // using SSL
        private bool _secure;

        // SSL/TLS protocol version
        private MqttSslProtocols _sslProtocol;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="socket">Socket opened with the client</param>
        public MqttNetworkChannel(IWebSocketConnection webSocketConnection)
        {
            this._webSocketConnection = webSocketConnection;
            this._sslProtocol = MqttSslProtocols.None;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="remoteHostName">Remote Host name</param>
        /// <param name="remotePort">Remote port</param>
        /// <param name="secure">Using SSL</param>
        /// <param name="sslProtocol">SSL/TLS protocol version</param>
        public MqttNetworkChannel(string url, int remotePort, bool secure, MqttSslProtocols sslProtocol, IWebSocketConnection webSocketConnection)
        {
            _url = url;
            _remotePort = remotePort;
            _secure = secure;
            _sslProtocol = sslProtocol;

            _webSocketConnection = webSocketConnection;

            if (secure && (sslProtocol == MqttSslProtocols.None))
                throw new ArgumentException("For secure connection, an SSL/TLS protocol version is needed");
        }

        public bool DataAvailable
        {
            get { return true; }
        }

        public int Receive(byte[] buffer, ref bool isRunning)
        {
            if (isRunning)
                return _webSocketConnection.Receive(buffer, ref isRunning);

            return 0;
            ////  Note:  Receive should be fired from an input message from the socket
            ////  not sure what to implement, but will return buffer length for now

            //// read all data needed (until fill buffer)
            //int idx = 0;
            //while (idx < buffer.Length)
            //{
            //    // fixed scenario with socket closed gracefully by peer/broker and
            //    // Read return 0. Avoid infinite loop.

            //    // read is executed synchronously
            //    var result = _webSocketConnection.InputStream.ReadAsync(buffer, 0, buffer.Length).Result;
            //    if (result == 0)
            //        return 0;
            //    idx += result;
            //}
            //return buffer.Length;
        }

        public int Receive(byte[] buffer)
        {
            var isRunning = true;
            return Receive(buffer, ref isRunning);
        }

        //public int Receive(byte[] buffer, int timeout)
        //{
        //    return buffer.Length;
        //    //CancellationTokenSource cts = new CancellationTokenSource(timeout);

        //    //try
        //    //{
        //    //    // read all data needed (until fill buffer)
        //    //    int idx = 0;
        //    //    while (idx < buffer.Length)
        //    //    {
        //    //        // fixed scenario with socket closed gracefully by peer/broker and
        //    //        // Read return 0. Avoid infinite loop.
        //    //        // read is executed synchronously
        //    //        var result = _webSocketConnection.InputStream.ReadAsync(buffer, 0, buffer.Length, cts.Token).Result;
        //    //        if (result == 0)
        //    //            return 0;
        //    //        idx += result;
        //    //    }
        //    //    return buffer.Length;
        //    //}
        //    //catch (TaskCanceledException)
        //    //{
        //    //    return 0;
        //    //}
        //}

        public int Send(byte[] buffer)
        {
            this._webSocketConnection.Send(buffer);

            return buffer.Length;
        }

        public void Close()
        {
            this._webSocketConnection?.Close();
            this._webSocketConnection?.Dispose();  //  Prior version did a dispose
        }

        public void Connect()
        {
            if (this._webSocketConnection != null && this._webSocketConnection.IsOpen)
            {
                Close();
            }

            WebsocketConnectionInit();

            this._webSocketConnection.Open(_url, "mqtt");
            Task.Delay(1000).Wait(); // for some reason we need to wait here to avoid an error writing to the websocket stream

            //this.socket = new StreamSocket();

            //// connection is executed synchronously
            //this.socket.ConnectAsync(this.remoteHostName,
            //    this.remotePort.ToString(),
            //    MqttSslUtility.ToSslPlatformEnum(this.sslProtocol)).AsTask().Wait();
        }

        private void WebsocketConnectionInit()
        {
            this._webSocketConnection.OnClosed += _webSocketConnection_OnClosed;
            this._webSocketConnection.OnData += _webSocketConnection_OnData;
            this._webSocketConnection.OnOpened += _webSocketConnection_OnOpened;
            this._webSocketConnection.OnError += _webSocketConnection_OnError;
            this._webSocketConnection.OnDispose += _webSocketConnection_OnDispose;
        }

        private void WebsocketConnectionDispose()
        {
            if (_webSocketConnection != null)
            {
                this._webSocketConnection.OnClosed -= _webSocketConnection_OnClosed;
                this._webSocketConnection.OnData -= _webSocketConnection_OnData;
                this._webSocketConnection.OnOpened -= _webSocketConnection_OnOpened;
                this._webSocketConnection.OnError -= _webSocketConnection_OnError;
                this._webSocketConnection.OnDispose -= _webSocketConnection_OnDispose;

                _webSocketConnection?.Dispose();
                _webSocketConnection = null;
            }
        }

        private void _webSocketConnection_OnDispose(object sender, EventArgs e)
        {
            WebsocketConnectionDispose();
        }

        private void _webSocketConnection_OnError(object sender, WebSocketExceptionEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Web socket error: {0}", e.Exception.Message);
        }

        private void _webSocketConnection_OnOpened(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Web socket connected");
        }

        private void _webSocketConnection_OnData(object sender, WebSocketDataEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Web socket received data 0x{0}", string.Join("", e.Data.Select(x => x.ToString("X2"))));
        }

        private void _webSocketConnection_OnClosed(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Web socket closed");
            WebsocketConnectionDispose();
        }

        public void Accept()
        {
            // TODO : SSL support with StreamSocket / StreamSocketListener seems to be NOT supported
            return;
        }
    }

    /// <summary>
    /// MQTT SSL utility class
    /// </summary>
    //public static class MqttSslUtility
    //{
    //    public static SocketProtectionLevel ToSslPlatformEnum(MqttSslProtocols mqttSslProtocol)
    //    {
    //        switch (mqttSslProtocol)
    //        {
    //            case MqttSslProtocols.None:
    //                return SocketProtectionLevel.PlainSocket;
    //            case MqttSslProtocols.SSLv3:
    //                return SocketProtectionLevel.SslAllowNullEncryption;
    //            case MqttSslProtocols.TLSv1_0:
    //                return SocketProtectionLevel.Tls10;
    //            case MqttSslProtocols.TLSv1_1:
    //                return SocketProtectionLevel.Tls11;
    //            case MqttSslProtocols.TLSv1_2:
    //                return SocketProtectionLevel.Tls12;
    //            default:
    //                throw new ArgumentException("SSL/TLS protocol version not supported");
    //        }
    //    }
    //}
}
