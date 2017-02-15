using System;

namespace OSTUSA.XamarinDemo.Core.Networking.Sockets.Events
{
    public class WebSocketDataEventArgs : EventArgs
    {
        public byte[] Data { get; private set; }
        
        public WebSocketDataEventArgs(byte[] data)
        {
            Data = data;
        }
    }
}

