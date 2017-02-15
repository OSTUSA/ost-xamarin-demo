using System;

namespace OSTUSA.IoT.Core.Networking.Sockets.Events
{
    public class WebSocketExceptionEventArgs : EventArgs
    {
        public Exception Exception { get; private set; }

        public WebSocketExceptionEventArgs(Exception exception)
        {
            Exception = exception;
        }
    }
}

