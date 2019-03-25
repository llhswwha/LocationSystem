using ArchorUDPTool.Models;
using DbModel.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ArchorUDPTool.Controls
{
    /// <summary>
    /// UDPArchorListBox.xaml 的交互逻辑
    /// </summary>
    public partial class UDPArchorListBox : UserControl
    {
        public UDPArchorListBox()
        {
            InitializeComponent();
        }

        public ArchorManager archorManager;

        private void CheckColumn_Checked(object sender, RoutedEventArgs e)
        {
            if (archorManager == null) return;
            archorManager.CheckAll();
        }

        private void CheckColumn_Unchecked(object sender, RoutedEventArgs e)
        {
            if (archorManager == null) return;
            archorManager.UnCheckAll();
        }

        public object SelectedItem
        {
            get
            {
                return DataGrid3.SelectedItem;
            }
        }

        public IList SelectedItems
        {
            get
            {
                return DataGrid3.SelectedItems;
            }
        }

        private UDPArchorList _archorList;

        public UDPArchorList ItemsSource
        {
            //get
            //{
            //    return DataGrid3.ItemsSource;
            //}
            set
            {
                _archorList = value;
                DataGrid3.ItemsSource = value;
                if (value == null)
                {
                    LbCount.Content = "";
                }
                else
                {
                    LbCount.Content = string.Format("{0}/{1}", value.GetConnectedCount(), value.Count);
                }
                
                subArchorList = value;

                Filter();
            }
        }

        public List<UDPArchor> GetCheckedArchors()
        {
            var list = new List<UDPArchor>();
            foreach (var item in DataGrid3.Items)
            {
                var archor = item as UDPArchor;
                if (archor == null) continue;
                if (archor.IsChecked)
                    list.Add(archor);
            }
            return list;
        }

        private void MenuRefreshOne_Click(object sender, RoutedEventArgs e)
        {
            var archor = DataGrid3.SelectedItem as UDPArchor;
            if (archor == null) return;
            archorManager.ScanArchor(archor);
        }

        private void MenuRefeshMuti_Click(object sender, RoutedEventArgs e)
        {
            var list = new List<UDPArchor>();
            foreach (var item in DataGrid3.SelectedItems)
            {
                var archor = item as UDPArchor;
                if (archor == null) continue;
                list.Add(archor);
            }
            archorManager.ScanArchor(list.ToArray());
        }

        private void MenuRefreshMultiChecked_Click(object sender, RoutedEventArgs e)
        {
            var list = GetCheckedArchors();
            archorManager.ScanArchor(list.ToArray());
        }

        private void MenuPingArchor_Click(object sender, RoutedEventArgs e)
        {
            var archor = DataGrid3.SelectedItem as UDPArchor;
            if (archor == null) return;
            var pingWnd = new PingWindow(archor.GetIp());
            pingWnd.Show();
        }

        private void MenuSetArchor_Click(object sender, RoutedEventArgs e)
        {
            var archor = DataGrid3.SelectedItem as UDPArchor;
            if (archor == null) return;
            var wnd = new UDPArchorInfoWindow(archorManager, archor);
            wnd.Show();
        }

        private void MenuRestartOne_OnClick(object sender, RoutedEventArgs e)
        {
            var archor = DataGrid3.SelectedItem as UDPArchor;
            if (archor == null) return;
            archorManager.Reset(archor);
        }

        public UDPArchorList subArchorList;

        private void CbFilterCondition_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        public void Filter()
        {
            if (_archorList == null) return;
            int id = CbFilterCondition.SelectedIndex;
            subArchorList = new UDPArchorList();
            foreach (var item in _archorList)
            {
                if (id == 0)//全部
                {
                    subArchorList.Add(item);
                }
                else if (id == 1)//连通
                {
                    if (!string.IsNullOrEmpty(item.IsConnected))
                    {
                        subArchorList.Add(item);
                    }
                }
                else if (id == 2)//不连通
                {
                    if (string.IsNullOrEmpty(item.IsConnected))
                    {
                        subArchorList.Add(item);
                    }
                }
                else if (id == 3)//Ping通
                {
                    if (!string.IsNullOrEmpty(item.Ping))
                    {
                        subArchorList.Add(item);
                    }
                }
                else if (id == 4)//不Ping通
                {
                    if (string.IsNullOrEmpty(item.Ping))
                    {
                        subArchorList.Add(item);
                    }
                }
                else if (id == 5)//3
                {
                    if (item.GetClientIP().StartsWith("192.168.3."))
                    {
                        subArchorList.Add(item);
                    }
                }
                else if (id == 6)//4
                {
                    if (item.GetClientIP().StartsWith("192.168.4."))
                    {
                        subArchorList.Add(item);
                    }
                }
                else if (id == 7)//5
                {
                    if (item.GetClientIP().StartsWith("192.168.5."))
                    {
                        subArchorList.Add(item);
                    }
                }
                else if (id == 8)//1999端口
                {
                    if (item.ServerPort == 1999)
                    {
                        subArchorList.Add(item);
                    }
                }
                else if (id == 9)//有DbInfo
                {
                    if (!string.IsNullOrEmpty(item.DbInfo))
                    {
                        subArchorList.Add(item);
                    }
                }
                else if (id == 10)//11222902
                {
                    if (item.SoftVersion == "11222902")
                    {
                        subArchorList.Add(item);
                    }
                }
                else if (id == 11)//11222906
                {
                    if (item.SoftVersion == "11222906")
                    {
                        subArchorList.Add(item);
                    }
                }
                else if (id == 12)//11222906
                {
                    if (item.SoftVersion == "11222907")
                    {
                        subArchorList.Add(item);
                    }
                }
                else if (id == 13)//3506
                {
                    if (item.SoftVersion == "3156"|| item.SoftVersion == "3157")
                    {
                        subArchorList.Add(item);
                    }
                }
                else if (id == 14)//网关错误
                {
                    if (!string.IsNullOrEmpty(item.Ip) && !string.IsNullOrEmpty(item.Gateway) && !IpHelper.IsSameDomain(item.Ip, item.Gateway))
                    {
                        subArchorList.Add(item);
                    }
                }
                else if (id == 15)//IsChecked
                {
                    if (item.IsChecked)
                    {
                        subArchorList.Add(item);
                    }
                }
                else if (id == 16)//有DbInfo
                {
                    if (!string.IsNullOrEmpty(item.DbInfo))
                    {
                        subArchorList.Add(item);
                    }
                }
                else if (id == 17)//无DbInfo
                {
                    if (string.IsNullOrEmpty(item.DbInfo))
                    {
                        subArchorList.Add(item);
                    }
                }
                else if (id == 18)//非25IP
                {
                    if (!string.IsNullOrEmpty(item.IsConnected) && item.ServerIp != "172.16.100.25")
                    {
                        subArchorList.Add(item);
                    }
                }
                else if (id == 19)//ping有问题的
                {
                    if (string.IsNullOrEmpty(item.Ping)||item.Ping=="*")
                    {
                        subArchorList.Add(item);
                    }
                    else
                    {
                        string[] parts = item.Ping.Split('/');
                        if (parts[0] != parts[1])
                        {
                            subArchorList.Add(item);
                        }
                    }
                }
                else if (id == 20)//ping空
                {
                    if (string.IsNullOrEmpty(item.Ping))
                    {
                        subArchorList.Add(item);
                    }
                }
                else if (id == 21)//ping失败
                {
                    if (item.Ping == "*")
                    {
                        subArchorList.Add(item);
                    }
                }
                else if (id == 22)//ping丢包
                {
                    if (!string.IsNullOrEmpty(item.Ping))
                    {
                        string[] parts = item.Ping.Split('/');
                        if (parts.Length==2&&parts[0] != parts[1])
                        {
                            subArchorList.Add(item);
                        }
                    }
                }
            }
            DataGrid3.ItemsSource = subArchorList;
            LbCount.Content = string.Format("{0}/{1}", subArchorList.GetConnectedCount(), subArchorList.Count);

        }

        private void CbAreas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var area = CbAreas.SelectedItem as string;
            string[] parts = area.Split(' ');
            subArchorList = new UDPArchorList();
            foreach (var item in _archorList)
            {
                if (area == "全部")
                {
                    subArchorList.Add(item);
                }
                else if (item.RealArea==area)
                {
                    subArchorList.Add(item);
                }
            }
            DataGrid3.ItemsSource = subArchorList;
            LbCount.Content = string.Format("{0}/{1}", subArchorList.GetConnectedCount(), subArchorList.Count);
        }

        private void BtnGetAreas_Click(object sender, RoutedEventArgs e)
        {
            if (_archorList == null) return;
            List<string> areas = _archorList.GetAreas();
            areas.Insert(0, "全部");
            CbAreas.ItemsSource = areas;
        }

        private void BtnCountByArea_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string msg = _archorList.GetCountByArea();
                DataTable dt = _archorList.GetCountByAreaTable();
                FileInfo file = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "\\Data\\基站信息\\基站数量按区域统计.xls");
                ExcelLib.ExcelHelper.Save(dt, file, "");
                System.Windows.Clipboard.SetText(msg);
                MessageBox.Show(msg);
                Process.Start(file.Directory.FullName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void BtnFilter_Click(object sender, RoutedEventArgs e)
        {
            //if (_archorList == null) return;
            //int id = CbFilterCondition.SelectedIndex;
            //subArchorList = new UDPArchorList();
            //foreach (var item in _archorList)
            //{
            //    if (CbConnected.IsChecked == true)
            //    {
            //        if (!string.IsNullOrEmpty(item.IsConnected))
            //        {
            //            subArchorList.Add(item);
            //        }
            //    }
            //    else
            //    {
            //        if (string.IsNullOrEmpty(item.IsConnected))
            //        {
            //            subArchorList.Add(item);
            //        }
            //    }

            //    if (CbPing.IsChecked == true)
            //    {
            //        if (!string.IsNullOrEmpty(item.Ping))
            //        {
            //            subArchorList.Add(item);
            //        }
            //    }
            //    else
            //    {
            //        if (string.IsNullOrEmpty(item.Ping))
            //        {
            //            subArchorList.Add(item);
            //        }
            //    }

            //    if (id == 0)//全部
            //    {
            //        subArchorList.Add(item);
            //    }
            //    else if (id == 5)//3
            //    {
            //        if (item.GetClientIP().StartsWith("192.168.3."))
            //        {
            //            subArchorList.Add(item);
            //        }
            //    }
            //    else if (id == 6)//4
            //    {
            //        if (item.GetClientIP().StartsWith("192.168.4."))
            //        {
            //            subArchorList.Add(item);
            //        }
            //    }
            //    else if (id == 7)//5
            //    {
            //        if (item.GetClientIP().StartsWith("192.168.5."))
            //        {
            //            subArchorList.Add(item);
            //        }
            //    }
            //    else if (id == 8)//1999端口
            //    {
            //        if (item.ServerPort == 1999)
            //        {
            //            subArchorList.Add(item);
            //        }
            //    }
            //}
            //DataGrid3.ItemsSource = subArchorList;
            //LbCount.Content = string.Format("{0}/{1}", subArchorList.GetConnectedCount(), subArchorList.Count);

        }

        private void CbFilterCodition2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_archorList == null) return;
            int id = CbFilterCodition2.SelectedIndex;
            UDPArchorList list = new UDPArchorList();
            foreach (var item in subArchorList)
            {
                if (id == 0)//全部
                {
                    list.Add(item);
                }
                else if (id == 1)//有值
                {
                    if (!string.IsNullOrEmpty(item.Value))
                    {
                        list.Add(item);
                    }
                }
                else if (id == 2)//无值
                {
                    if (string.IsNullOrEmpty(item.Value))
                    {
                        list.Add(item);
                    }
                }
                
            }
            DataGrid3.ItemsSource = list;
            LbCount.Content = string.Format("{0}/{1}", list.GetConnectedCount(), list.Count);
        }

        private void BtnFilterNoValue_Click(object sender, RoutedEventArgs e)
        {
            if (_archorList == null) return;
            int id = CbFilterCodition2.SelectedIndex;
            UDPArchorList list = new UDPArchorList();
            foreach (var item in _archorList)
            {
                if (string.IsNullOrEmpty(item.Value)&&!string.IsNullOrEmpty(item.IsConnected))
                {
                    list.Add(item);
                }
            }
            DataGrid3.ItemsSource = list;
            LbCount.Content = string.Format("{0}/{1}", list.GetConnectedCount(), list.Count);
        }

        private void BtnNoDbInfo_Click(object sender, RoutedEventArgs e)
        {
            if (_archorList == null) return;
            int id = CbFilterCodition2.SelectedIndex;
            UDPArchorList list = new UDPArchorList();
            foreach (var item in _archorList)
            {
                if (string.IsNullOrEmpty(item.DbInfo) && !string.IsNullOrEmpty(item.IsConnected))
                {
                    list.Add(item);
                }
            }
            DataGrid3.ItemsSource = list;
            LbCount.Content = string.Format("{0}/{1}", list.GetConnectedCount(), list.Count);
        }

        public DataGrid DataGrid
        {
            get
            {
                return DataGrid3;
            }
        }

        public ContextMenu DataGridMenu
        {
            get
            {
                return DataGrid3.ContextMenu;
            }
        }
    }
}
