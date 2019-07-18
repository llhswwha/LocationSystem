using WebApiCommunication.ExtremeVision;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WebApiLib.Clients;
using System.Configuration;

namespace ExtremeVisionSimulator
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadHeadInfo_Click(object sender, RoutedEventArgs e)
        {
            string fileName = "HeadInfo.json";
            ReadInfoFile(fileName);
        }

        private void ReadInfoFile(string fileName)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "Data\\" + fileName;
            if (!File.Exists(path))
            {
                MessageBox.Show("未找到文件:" + path);
                return;
            }

            string content = File.ReadAllText(path);
            TbContent.Text = content;

            try
            {
                CameraAlarmInfo info = JsonConvert.DeserializeObject<CameraAlarmInfo>(content);
                GetImage(info.pic_data);

                info.time = GetDataTime(info.time_stamp);
                info.pic_data = "";
                grid1.SelectedObject = info;
                
                info.ParseData();
                string json = JsonConvert.SerializeObject(info);
                CameraAlarmInfo info2 = JsonConvert.DeserializeObject<CameraAlarmInfo>(json);
            }
            catch (Exception ex)
            {
                MessageBox.Show("解析失败:"+ex);
            }
        }

        public DateTime GetDataTime(long time_stamp)
        {
            DateTime dtStart = new DateTime(1970, 1, 1);
            long lTime = ((long)time_stamp * 10000000);
            TimeSpan toNow = new TimeSpan(lTime);
            DateTime AlarmTime = dtStart.Add(toNow);
            return AlarmTime;
        }

        public void GetImage(string base64)
        {
            base64 = base64.Replace("data:image/png;base64,", "").Replace("data:image/jgp;base64,", "").Replace("data:image/jpg;base64,", "").Replace("data:image/jpeg;base64,", "");//将base64头部信息替换
            byte[] bytes = Convert.FromBase64String(base64);
            //string imagebase64 = base64.Substring(base64.IndexOf(",") + 1);

            MemoryStream memStream = new MemoryStream(bytes);
            System.Drawing.Image mImage = System.Drawing.Image.FromStream(memStream);
            string path = AppDomain.CurrentDomain.BaseDirectory + "1.jpg";
            mImage.Save(path);

            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = new MemoryStream(bytes);
            bi.EndInit();
            Image1.Source = bi;
        }

        private void LoadFlameInfo_Click(object sender, RoutedEventArgs e)
        {
            string fileName = "FlameInfo.json";
            ReadInfoFile(fileName);
        }


        private void MenuSendToListener_Click(object sender, RoutedEventArgs e)
        {
            string host = ConfigurationManager.AppSettings["TargetHost"];
            string port = ConfigurationManager.AppSettings["Port2"];
            string content = TbContent.Text;
            WebApiClient client2 = new WebApiClient(host, port);
            string result2 = client2.PostEntity<string>("listener/ExtremeVision/callback/", content, true);
            MessageBox.Show("result2:" + result2);
        }

        private void MenuSendToWebApiPost_Click(object sender, RoutedEventArgs e)
        {
            string host = ConfigurationManager.AppSettings["TargetHost"];
            string port = ConfigurationManager.AppSettings["Port1"];
            WebApiClient client = new WebApiClient(host, port);
            string content = TbContent.Text;
            string result = client.PostEntity<string>("api/ExtremeVision/callback/", content, true);
            MessageBox.Show("result1:" + result);
        }

        private void LoadTestInfo_Click(object sender, RoutedEventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "Data\\Test";
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] files = dir.GetFiles();

            string host = ConfigurationManager.AppSettings["TargetHost"];
            string port = ConfigurationManager.AppSettings["Port2"];

            foreach (FileInfo file in files)
            {
                string content = File.ReadAllText(file.FullName);
                //CameraAlarmInfo info = JsonConvert.DeserializeObject<CameraAlarmInfo>(content);
                //info.ParseData();
                WebApiClient client2 = new WebApiClient(host, port);
                string result2 = client2.PostEntity<string>("listener/ExtremeVision/callback/", content, true);
                //MessageBox.Show("result2:" + result2);
            }
        }
    }
}
