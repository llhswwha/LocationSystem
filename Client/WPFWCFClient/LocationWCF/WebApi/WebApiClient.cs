namespace LocationClient.WebApi
{
    public class WebApiClient
    {
        public string BaseUri { get; set; }

        public string Accept { get; set; }

        public WebApiClient()
        {
            BaseUri = "http://<host>:<port>/";
        }

        public WebApiClient(string host, string port)
        {
            BaseUri = string.Format("http://{0}:{1}/", host, port);
        }

        public WebApiClient(string baseUri)
        {
            BaseUri = baseUri;
        }

        public string GetString(string path="")
        {
            return WebApiHelper.GetString(string.Format("{0}{1}", BaseUri, path),Accept);
        }
    }
}
