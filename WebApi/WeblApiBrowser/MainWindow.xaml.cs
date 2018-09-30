using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MarkedNet;
using System.IO;
using System.Net.Http;
using WebApiLib;
using WebApiLib.ApiDocs;
using WebApiLib.Clients;

namespace RestFulApiBrowser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            //LoadMdPaths();

            LoadApiDoc();
        }

        private ApiDocument _apiDoc;
        private void LoadApiDoc()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "/Data/BaseData.html";
            _apiDoc=new ApiDocument();
            _apiDoc.LoadDoc(path);
            LbUrls.ItemsSource = _apiDoc.Paths;
            LbUrls.DisplayMemberPath = "Name";
        }

        private void LoadMdPaths()
        {
            Marked marked = new Marked();
            string path = AppDomain.CurrentDomain.BaseDirectory + "api.md";
            string txt = File.ReadAllText(path);
            string[] lines = File.ReadAllLines(path);
            List<string> urls = new List<string>();
            foreach (string line in lines)
            {
                if (line.Trim().StartsWith("http:"))
                {
                    urls.Add(line.Trim());
                }
            }
            string result = marked.Parse(txt);
            LbUrls.ItemsSource = urls;
        }

        private void LbUrls_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            

            if (LbUrls.SelectedItem is ApiPath)
            {
                _apiDoc.BaseUri = TbBaseUri.Text;
                ApiPath apiPath = LbUrls.SelectedItem as ApiPath;
                //string resMsg = WebApiHelper.GetString(path.GetUrl());
                //TbResult.Text = string.Format("path:\n{0}\nResult:\n{1}\n", path, resMsg);

                string url = apiPath.GetUrl();
                if (string.IsNullOrEmpty(url))
                {
                    TbResult.Text = string.Format("path:\n{0}\nStatus:{1}\nResult:\n{2}\n", apiPath, "NoUri", "");
                    apiPath.SetResultState("NoUri");
                }
                else
                {
                    var client = new HttpClient();
                    HttpResponseMessage resMsg = client.GetAsync(url).Result;
                    var result = resMsg.Content.ReadAsStringAsync().Result;
                    //string resMsg=WebApiHelper.GetString(url);
                    TbResult.Text = string.Format("path:\n{0}\nStatus:{1}\nResult:\n{2}\n", apiPath, resMsg.StatusCode, result);
                    apiPath.SetResultState(resMsg.StatusCode.ToString());
                }
            }
            else
            {
                string url = LbUrls.SelectedItem as string;
                TbResult.Text = url;
            }
        }

        private void MenuItemTestAllPath_OnClick(object sender, RoutedEventArgs e)
        {

            if (_apiDoc != null)
            {
                _apiDoc.BaseUri = TbBaseUri.Text;
                ApiDocClient client = new ApiDocClient(_apiDoc);
                TbResult.Text = client.TestApis();

                LbUrls.DisplayMemberPath = "Result";
            }
        }
    }
}
