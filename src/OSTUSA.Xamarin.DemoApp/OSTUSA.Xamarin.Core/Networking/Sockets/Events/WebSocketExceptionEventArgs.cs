using System;

namespace OSTUSA.XamarinDemo.Core.Networking.Sockets.Events
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

