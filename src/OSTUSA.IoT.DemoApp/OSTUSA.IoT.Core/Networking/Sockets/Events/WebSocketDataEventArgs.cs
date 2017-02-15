using System;

namespace OSTUSA.IoT.Core.Networking.Sockets.Events
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

