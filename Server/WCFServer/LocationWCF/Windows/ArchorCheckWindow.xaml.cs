using ArchorUDPTool.Models;
using DbModel.Location.AreaAndDev;
using DbModel.Tools;
using LocationServer.Models.EngineTool;
using LocationServer.Tools;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace LocationServer.Windows
{
    /// <summary>
    /// ArchorCheckWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ArchorCheckWindow : Window
    {
        public ArchorCheckWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            list1= ArchorHelper.LoadArchoDevInfo().ArchorList;
            Group1.Header += " " + list1.Count;
            DataGrid1.ItemsSource = list1;

            BLL.Bll bll = new BLL.Bll();

            list3= bll.Archors.ToList();
            DataGrid3.ItemsSource = list3;

            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\基站信息\\UDPArchorList.xml";
            list2=XmlSerializeHelper.LoadFromFile<UDPArchorList>(path);
            Group2.Header += " " + list2.Count;
            DataGrid2.ItemsSource = list2;

            var list4 = bll.bus_anchors.ToList();
            Group4.Header += " " + list4.Count;
            DataGrid4.ItemsSource = list4;

            areas = bll.Areas.ToList();

            Check();
        }

        List<Area> areas;

        private void Check()
        {
            int count3 = 0;
            foreach (var item in list3)
            {
                if (!item.GetCode().StartsWith("Code"))
                {
                    count3++;
                }
            }
            Group3.Header += string.Format(" ({0}/{1})", count3, list3.Count);

            int count2 = 0;
            foreach (var item2 in list2)
            {
                var i3 = list3.Find(i => i.Code == item2.Id);
                if (i3 != null)
                {
                    var area = areas.Find(i => i.Id == i3.ParentId);
                    item2.Path2 = area.Name;
                    count2++;
                }
                else
                {
                    item2.Path2 = "*";
                }

                var i1 = list1.Find(i => i.ArchorID == item2.Id);
                if (i1 != null)
                {
                    if (i1.ArchorIp == item2.Ip)
                    {
                        item2.Path1 = i1.InstallArea;
                    }
                    else
                    {
                        item2.Path1 = string.Format("IP不同:{0},{1}",i1.ArchorIp,item2.Ip);
                    }
                }
                else
                {
                    item2.Path1 = "*";
                }
            }
            Group2.Header = string.Format("扫描基站({0}/{1})", count2, list2.Count);

            int count1 = 0;
            foreach (var item1 in list1)
            {
                var i2 = list2.Find(i => i.Id == item1.ArchorID && i.Ip == item1.ArchorIp);
                if (i2 != null)
                {
                    item1.IsConnected = "可以";
                    count1++;
                }
                else
                {
                    item1.IsConnected = "*";
                }
            }
            Group1.Header = string.Format("设备清单({0}/{1})", count1, list1.Count);
        }

        List<ArchorDev> list1;
        UDPArchorList list2;
        List<Archor> list3;

        private void MenuScan_Click(object sender, RoutedEventArgs e)
        {
            var win = new ArchorConfigureWindow();
            win.ShowDialog();
            list2= win.archorManager.archorList;
            Group2.Header += " " + list2.Count;
            DataGrid2.ItemsSource = list2;

            Check();
        }

        private void MenuCheck_Click(object sender, RoutedEventArgs e)
        {
            Check();
        }

        private void DataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item2 = DataGrid2.SelectedItem as UDPArchor;
            if (item2 == null) return;
            var i1 = list1.Find(i => i.ArchorID == item2.Id);
            if (i1 != null)
            {
                DataGrid1.SelectedItem = i1;
                DataGrid1.ScrollIntoView(DataGrid1.SelectedItem);
            }

            var i3 = list3.Find(i => i.Code == item2.Id);
            if (i3 != null)
            {
                DataGrid3.SelectedItem = i3;
                DataGrid3.ScrollIntoView(DataGrid3.SelectedItem);
            }
        }
        private void ExportListFile_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID号");
            dt.Columns.Add("IP地址");
            dt.Columns.Add("安装区域");
            dt.Columns.Add("网络能通");

            int count1 = 0;
            foreach (var item in list1)
            {
                if (item.IsConnected != "*")
                {
                    count1++;
                }
                dt.Rows.Add(item.ArchorID, item.ArchorIp, item.InstallArea, item.IsConnected);
            }
            FileInfo file = new FileInfo(AppDomain.CurrentDomain.BaseDirectory+string.Format("基站设备校对清单({0}_{1}).xls",count1,list1.Count));
            ExcelLib.ExcelHelper.Save(dt, file, string.Format("基站设备校对清单({0}_{1})", count1, list1.Count));
        }
    }
}
