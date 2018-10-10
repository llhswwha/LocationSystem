using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using WebApiLib.Clients;

namespace WebApiBrowser
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

        private void MenuRestuFulApi_Click(object sender, RoutedEventArgs e)
        {
            RestFulApiBrowser.RestFulApiWindow window = new RestFulApiBrowser.RestFulApiWindow();
            window.ShowDialog();
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
