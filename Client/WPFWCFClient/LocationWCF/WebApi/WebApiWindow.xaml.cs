using System.Windows;

namespace LocationClient.WebApi
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WebApiWindow : Window
    {
        public WebApiWindow()
        {
            InitializeComponent();
        }

        public void SetUrl(string url)
        {
            TbUri.Text = url;
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            string uri = TbUri.Text;
            string accept = CbContentType.Text;
            WebApiClient client = new WebApiClient(uri);
            client.Accept = accept;
            string content = client.GetString();
            //content = content.Replace(">", ">\n");
            TbContent.Text = content;

            //XmlDocument doc = new XmlDocument();
            //doc.LoadXml(content);
            //TbContent.Text=doc.InnerXml;
            //string filePath = AppDomain.CurrentDomain.BaseDirectory + "a.tmp";
            //File.WriteAllText(filePath, content);
            //WbContent.NavigateToString(content);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Test();
        }

        private void Test()
        {
            WebApiClient client = new WebApiClient("localhost", "8080");
            string str1 = client.GetString("");
            string str2 = client.GetString("issue");
            string str3 = client.GetString("issue/1");

        }
    }
}
