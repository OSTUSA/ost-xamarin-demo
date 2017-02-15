using System;
using Square.SocketRocket;
using Foundation;
using OSTUSA.XamarinDemo.Core.Networking.Sockets.Events;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OSTUSA.XamarinDemo.Services.Networking.Sockets
{
    public class WebSocketConnection : IWebSocketConnection
    {
        public bool IsOpen { get; private set; }

        public event EventHandler OnClosed;
        public event EventHandler OnDispose;
        public event EventHandler<WebSocketExceptionEventArgs> OnError;
        public event EventHandler<WebSocketDataEventArgs> OnData;
        public event EventHandler OnOpened;

        static WebSocketConnection()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback += (o, certificate, chain, errors) => true;
        }

        private WebSocket _client = null;
        private object _lock = new object();
        private Queue<byte> _inputQueue;

        public WebSocketConnection()
        {
            _inputQueue = new Queue<byte>();
        }

        public void Open(string url, string protocol = null)
        {
            try
            {
                if (_client != null)
                    Close();

                SetUpClient(url, protocol);

                NSObject.InvokeInBackground(() => OpenInternal(url, protocol));
            }
            catch (Exception ex)
            {
                OnError?.Invoke(this, new WebSocketExceptionEventArgs(ex));
            }
        }

        private object _connectionLock = new object();
        private void OpenInternal(string url, string protocol)
        {
            lock (_connectionLock)
            {
                if (_client != null)
                {
                    _inputQueue.Clear();
                    _client.Open();

                    return;
                }
            }

            SetUpClient(url, protocol);
            OpenInternal(url, protocol);
        }

        private void SetUpClient(string url, string protocol)
        {
            if (string.IsNullOrEmpty(protocol))
                _client = new WebSocket(new NSUrl(url));
            else
                _client = new WebSocket(new NSUrl(url), new NSObject[] { new NSString(protocol) });

            _client.ReceivedMessage += _client_ReceivedMessage;
            _client.WebSocketClosed += _client_WebSocketClosed;
            _client.WebSocketFailed += _client_WebSocketFailed;
            _client.WebSocketOpened += _client_WebSocketOpened;
        }

        public void Close()
        {
            lock (_connectionLock)
            {
                try
                {
                    if (_client != null)
                    {
                        _client.ReceivedMessage -= _client_ReceivedMessage;
                        _client.WebSocketFailed -= _client_WebSocketFailed;
                        _client.WebSocketOpened -= _client_WebSocketOpened;
                        _client.WebSocketClosed -= _client_WebSocketClosed;

                        if (_client.ReadyState == ReadyState.Open)
                        {
                            _client.Close();
                        }
                        IsOpen = false;

                        _client.Dispose();
                        _client = null;

                        System.Diagnostics.Debug.WriteLine($"Web Socket Closed in Close()\n{Environment.StackTrace}");
                        OnClosed?.Invoke(this, EventArgs.Empty);
                    }
                }
                catch (Exception ex)
                {
                    OnError?.Invoke(this, new WebSocketExceptionEventArgs(ex));
                }
            }
        }

        public void Send(byte[] data)
        {
            try
            {
                Task.Factory.StartNew(async () =>
                {
                    while (_client == null || _client.ReadyState != ReadyState.Open)
                    {
                        await Task.Delay(TimeSpan.FromMilliseconds(50));
                    }

                    _client.Send(NSData.FromArray(data));
                });
            }
            catch (Exception ex)
            {
                OnError?.Invoke(this, new WebSocketExceptionEventArgs(ex));
            }
        }

        public void Dispose()
        {
            Close();
            OnDispose?.Invoke(this, EventArgs.Empty);
        }

        public int Receive(byte[] buffer, ref bool isRunning)
        {
            int idx = 0;
            while (idx < buffer.Length && isRunning)
            {
                lock (_lock)
                {
                    if (_inputQueue.Any() && isRunning)
                        buffer[idx++] = _inputQueue.Dequeue();
                }
            }
            return buffer.Length;
        }


        // Handlers


        private void _client_WebSocketOpened(object sender, EventArgs e)
        {
            IsOpen = true;
            OnOpened?.Invoke(this, EventArgs.Empty);
        }

        private void _client_WebSocketFailed(object sender, WebSocketFailedEventArgs e)
        {
            if (e.Error != null)
            {
                OnError?.Invoke(this, new WebSocketExceptionEventArgs(new Exception(e.Error.ToString())));
            }
            else
            {
                OnError?.Invoke(this, new WebSocketExceptionEventArgs(new Exception("Unknown WebSocket error!")));
            }

            Close();
        }

        private void _client_WebSocketClosed(object sender, WebSocketClosedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Web Socket Closed in _client_WebSocketClosed() with Reason \"{e.Reason}\" ({e.Code})\n{Environment.StackTrace}");

            IsOpen = false;
            OnClosed?.Invoke(this, EventArgs.Empty);
        }

        private void _client_ReceivedMessage(object sender, WebSocketReceivedMessageEventArgs e)
        {
            if (e != null && e.Message != null)
            {
                var nsData = (NSData)e.Message;
                byte[] dataBytes = new byte[nsData.Length];

                System.Runtime.InteropServices.Marshal.Copy(nsData.Bytes, dataBytes, 0, Convert.ToInt32(nsData.Length));
                OnData?.Invoke(this, new WebSocketDataEventArgs(dataBytes));

                lock (_lock)
                {
                    foreach (var b in dataBytes)
                    {
                        _inputQueue.Enqueue(b);
                    }
                }
            }
        }
    }
}

