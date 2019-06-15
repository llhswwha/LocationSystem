using BLL;
using LocationServices.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LocationServer.Windows
{
    /// <summary>
    /// GetKKSMonitorDataWindow.xaml 的交互逻辑
    /// </summary>
    public partial class GetKKSMonitorDataWindow : Window
    {
        public GetKKSMonitorDataWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void MenuGetAllMonitorData_Click(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(() =>
            {
                try
                {
                    Bll bll = new Bll();
                    List<DbModel.Location.AreaAndDev.KKSCode> lst = bll.KKSCodes.ToList();
                    LocationService ls = new LocationService();
                    foreach (DbModel.Location.AreaAndDev.KKSCode item in lst)
                    {
                        var monitor = ls.GetDevMonitorInfoByKKS(item.Code, true);
                    }

                    MessageBox.Show("获取数据完成！");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("获取监控点数据失败：" + ex.Message);
                }
            });
            thread.IsBackground = true;
            thread.Start();

            return;
        }
    }
}
