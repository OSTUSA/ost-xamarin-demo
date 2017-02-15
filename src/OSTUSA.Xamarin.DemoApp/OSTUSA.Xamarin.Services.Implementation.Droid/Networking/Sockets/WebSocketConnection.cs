using System;
using System.Threading.Tasks;
using Square.OkHttp3;
using Square.OkHttp3.WS;
using Java.Util.Concurrent;
using OSTUSA.XamarinDemo.Core.Networking.Sockets.Events;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace OSTUSA.XamarinDemo.Services.Networking.Sockets
{
    public class WebSocketConnection : IWebSocketConnection
    {
        private readonly OkHttpClient _client;
        private IWebSocket _socket;

        private object _queueLock = new object();
        private object _sendLock = new object();
        private Queue<byte> _inputQueue;

        public WebSocketConnection()
        {
            _client = new OkHttpClient.Builder()
                .ReadTimeout(0, TimeUnit.Milliseconds)
                .WriteTimeout(0, TimeUnit.Milliseconds)
                .ConnectTimeout(10, TimeUnit.Seconds)
                .Build();
            
            _inputQueue = new Queue<byte>();
        }

        #region IWebSocketConnection implementation

        public event EventHandler OnClosed;
        public event EventHandler OnDispose;
        public event EventHandler<WebSocketExceptionEventArgs> OnError;
        public event EventHandler<WebSocketDataEventArgs> OnData;
        public event EventHandler OnOpened;


        public void Open(string url, string protocol = null)
        {
            var builder = new Request.Builder()
                .Url(url);

            if (!string.IsNullOrEmpty(protocol)) 
            {
                builder.AddHeader("Sec-WebSocket-Protocol", protocol);
            }

            var request = builder.Build();

            var call = WebSocketCall.Create(_client, request);
            var listener = call.Enqueue();
            listener.Open += Listener_Open;
            listener.Close += Listener_Close;
            listener.Failure += Listener_Failure;
            listener.Message += Listener_Message;
        }

        void Listener_Message(object sender, MessageEventArgs e)
        {
            var data = e.Message.Bytes();

            lock(_queueLock)
            {
                foreach (var b in data)
                {
                    _inputQueue.Enqueue(b);
                }
            }
            OnData?.Invoke(this, new WebSocketDataEventArgs(data));
        }

        void Listener_Failure(object sender, FailureEventArgs e)
        {
            OnError?.Invoke(this, new WebSocketExceptionEventArgs(e.Exception));

            Close();
        }

        void Listener_Close(object sender, CloseEventArgs e)
        {
            _socket?.Dispose();
            _socket = null;

            System.Diagnostics.Debug.WriteLine($"Web Socket Closed in Listener_Close() with Reason \"{e.Reason}\" ({e.Code})\n{Environment.StackTrace}");
            OnClosed?.Invoke(this, EventArgs.Empty);
        }

        void Listener_Open(object sender, OpenEventArgs e)
        {
            _socket = e.WebSocket;

            OnOpened?.Invoke(this, EventArgs.Empty);
        }

        public void Close()
        {
            try
            {
                _socket?.Close(0, "Closing");
            }
            catch (Java.Net.SocketException ex)
            {
                if (_socket != null)
                    Listener_Close(this, new CloseEventArgs(0, "Cannot close socket that is already closed."));
            }
        }

        public void Send(byte[] data)
        {
            var body = RequestBody.Create(WebSocket.Binary, data);

            // send the data in the background
            Task.Factory.StartNew(() => {
                while (!IsOpen)
                {
                    Thread.Sleep(50);
                }

                lock(_sendLock)
                {
                    _socket.SendMessage(body);
                }
            });
        }

        public bool IsOpen
        {
            get
            {
                return _socket != null;
            }
        }

        public int Receive(byte[] buffer, ref bool isRunning)
        {
            int idx = 0;
            while (idx < buffer.Length && isRunning)
            {
                lock (_queueLock)
                {
                    if (_inputQueue.Any() && isRunning)
                        buffer[idx++] = _inputQueue.Dequeue();
                }
            }
            return buffer.Length;
        }

        #endregion

        #region IDisposable implementation

        public void Dispose()
        {
            if (_socket != null)
                _socket.Dispose();
            _socket = null;

            OnDispose?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}

