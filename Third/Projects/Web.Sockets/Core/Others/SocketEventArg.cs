namespace Web.Sockets.Core
{
    public class SocketEventArg
    {
        public System.Net.Sockets.Socket Socket;
        public string Message;
        public SocketEventArg(System.Net.Sockets.Socket socket, string message)
        {
            this.Socket = socket;
            this.Message = message;
        }
    }
}
