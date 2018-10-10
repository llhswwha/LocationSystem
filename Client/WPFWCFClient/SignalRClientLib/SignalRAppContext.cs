namespace SignalRClientLib
{
    public class SignalRAppContext
    {
        public static string ServerUrl= "http://localhost:4955/realtime";

        public static void SetUrl(string ip, string port)
        {
            ServerUrl = string.Format("http://{0}:{1}/realtime", ip, port);
        }
    }
}
