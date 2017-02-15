using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSTUSA.XamarinDemo.Core.Networking.Sockets.Events;
using System.IO;

namespace OSTUSA.XamarinDemo.Services.Networking.Sockets
{
    public interface IWebSocketConnection : IDisposable
    {
        bool IsOpen { get; }

        event EventHandler OnClosed;
        event EventHandler OnDispose;
        event EventHandler<WebSocketExceptionEventArgs> OnError;
        event EventHandler<WebSocketDataEventArgs> OnData;
        event EventHandler OnOpened;

        void Close();
        void Open(string url, string protocol = null);
        void Send(byte[] data);

        //Stream InputStream { get; }
        int Receive(byte[] buffer, ref bool isRunning);
    }
}
