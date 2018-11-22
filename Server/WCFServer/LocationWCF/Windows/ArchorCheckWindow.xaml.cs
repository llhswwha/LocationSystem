using ArchorUDPTool.Controls;
using ArchorUDPTool.Models;
using BLL;
using DbModel.Location.AreaAndDev;
using DbModel.Tools;
using LocationServer.Models.EngineTool;
using LocationServer.Tools;
using System;
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
using System.Windows.Shapes;
using WPFClientControlLib.Extensions;

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

            DataGrid2 = new UDPArchorListBox();
            DataGrid2.DataGrid.SelectionChanged += DataGrid2_SelectionChanged;
            DataGrid2.DataGridMenu.AddMenu("定位", (obj) =>
            {
                Bll bll = new Bll();
                var list = new List<Archor>();
                foreach (var item in DataGrid2.SelectedItems)
                {
                    var archor = item as UDPArchor;
                    Archor dbArchor = bll.Archors.FindByCode(archor.Id);
                    if(dbArchor!=null)
                        list.Add(dbArchor);
                }
                var win = new AreaCanvasWindow(list.ToArray());
                win.Show();
            });
            Group2.Content = DataGrid2;
        }

        UDPArchorListBox DataGrid2;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();

        }

        private void LoadData()
        {
            fileArchorList= ArchorHelper.LoadArchoDevInfo().ArchorList;
            Group1.Header += " " + fileArchorList.Count;
            DataGrid1.ItemsSource = fileArchorList;

            BLL.Bll bll = new BLL.Bll();

            dbArchorList= bll.Archors.ToList();
            DataGridDb.ItemsSource = dbArchorList;

            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\基站信息\\UDPArchorList.xml";
            udpArchorList=XmlSerializeHelper.LoadFromFile<UDPArchorList>(path);
            Group2.Header += " " + udpArchorList.Count;
            DataGrid2.ItemsSource = udpArchorList;

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
            foreach (var item in dbArchorList)
            {
                if (!item.GetCode().StartsWith("Code"))
                {
                    count3++;
                }
            }
            Group3.Header += string.Format(" ({0}/{1})", count3, dbArchorList.Count);

            int count2 = 0;
            int count22 = 0;
            foreach (var item2 in udpArchorList)
            {
                if (!string.IsNullOrEmpty(item2.IsConnected))
                {
                    count22++;
                }
                var i3 = dbArchorList.Find(i => i.Code == item2.Id);
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

                var i1 = fileArchorList.Find(i => i.ArchorID == item2.Id);
                if (i1 != null)
                {
                    if (i1.ArchorIp == item2.GetClientIP())
                    {
                        item2.Path1 = i1.InstallArea;
                    }
                    else
                    {
                        item2.Path1 = string.Format("IP不同:{0},{1}",i1.ArchorIp,item2.GetClientIP());
                    }
                }
                else
                {
                    item2.Path1 = "*";
                }
            }
            Group2.Header = string.Format("扫描基站(录入Id:{0}/{1},连接基站 {2}/{1})", count2, udpArchorList.Count, count22);

            int count1 = 0;
            foreach (var item1 in fileArchorList)
            {
                var i2 = udpArchorList.Find(i => i.Id == item1.ArchorID && i.GetClientIP() == item1.ArchorIp);
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
            Group1.Header = string.Format("设备清单({0}/{1})", count1, fileArchorList.Count);
        }

        List<ArchorDev> fileArchorList;
        UDPArchorList udpArchorList;
        List<Archor> dbArchorList;

        private void MenuScan_Click(object sender, RoutedEventArgs e)
        {
            Bll bll = new Bll();
            var list3 = bll.Archors.ToList();
            var win = new ArchorConfigureWindow(list3);
            win.Show();

            //udpArchorList = win.archorManager.archorList;
            //Group2.Header += " " + udpArchorList.Count;
            //DataGrid2.ItemsSource = udpArchorList;

            //Check();
        }

        private void MenuCheck_Click(object sender, RoutedEventArgs e)
        {
            Check();
        }

        private void DataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item2 = DataGrid2.SelectedItem as UDPArchor;
            if (item2 == null) return;
            var i1 = fileArchorList.Find(i => i.ArchorID == item2.Id);
            if (i1 != null)
            {
                DataGrid1.SelectedItem = i1;
                DataGrid1.ScrollIntoView(DataGrid1.SelectedItem);
            }

            var i3 = dbArchorList.Find(i => i.Code == item2.Id);
            if (i3 != null)
            {
                DataGridDb.SelectedItem = i3;
                DataGridDb.ScrollIntoView(DataGridDb.SelectedItem);
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
            foreach (var item in fileArchorList)
            {
                if (item.IsConnected != "*")
                {
                    count1++;
                }
                dt.Rows.Add(item.ArchorID, item.ArchorIp, item.InstallArea, item.IsConnected);
            }
            FileInfo file = new FileInfo(AppDomain.CurrentDomain.BaseDirectory+string.Format("基站设备校对清单({0}_{1}).xls",count1,fileArchorList.Count));
            ExcelLib.ExcelHelper.Save(dt, file, string.Format("基站设备校对清单({0}_{1})", count1, fileArchorList.Count));
            Process.Start(file.Directory.FullName);
        }

        private void NotInFileList_Click(object sender, RoutedEventArgs e)
        {
            var dbList = new List<Archor>();
            foreach (var item in dbArchorList)
            {
                if (!item.Code.Contains("Code"))
                {
                    var fileItem = fileArchorList.Find(i => i.ArchorID == item.Code);
                    if (fileItem == null)
                    {
                        dbList.Add(item);
                    }
                }
            }
            Group3.Header = string.Format("数量 :{0}", dbList.Count);
            DataGridDb.ItemsSource = dbList;
        }

        private void InFileList_Click(object sender, RoutedEventArgs e)
        {
            var dbList = new List<Archor>();
            foreach (var item in dbArchorList)
            {
                if (!item.Code.Contains("Code"))
                {
                    var fileItem = fileArchorList.Find(i => i.ArchorID == item.Code);
                    if (fileItem != null)
                    {
                        dbList.Add(item);
                    }
                }
            }
            Group3.Header = string.Format("数量 :{0}", dbList.Count);
            DataGridDb.ItemsSource = dbList;
        }

        private void BtnRepeatIds_Click(object sender, RoutedEventArgs e)
        {
            var dbList = new List<Archor>();
            foreach (var item in dbArchorList)
            {
                if (!item.Code.Contains("Code"))
                {
                    var list = dbArchorList.FindAll(i => i.Code == item.Code);
                    if (list.Count > 1)
                    {
                        dbList.Add(item);
                    }
                }
            }
            Group3.Header = string.Format("数量 :{0}", dbList.Count);
            DataGridDb.ItemsSource = dbList;
        }

        private void MenuLocalArchor_Click(object sender, RoutedEventArgs e)
        {
            var list = new List<Archor>();
            foreach (var item in DataGridDb.SelectedItems)
            {
                var archor = item as Archor;
                list.Add(archor);
            }
            var win = new AreaCanvasWindow(list.ToArray());
            win.Show();
        }

        private void BtnNoIds_Click(object sender, RoutedEventArgs e)
        {
            var dbList = new List<Archor>();
            foreach (var item in dbArchorList)
            {
                if (item.Code.Contains("Code"))
                {
                        dbList.Add(item);
                }
            }
            Group3.Header = string.Format("数量 :{0}", dbList.Count);
            DataGridDb.ItemsSource = dbList;
        }
    }
}
