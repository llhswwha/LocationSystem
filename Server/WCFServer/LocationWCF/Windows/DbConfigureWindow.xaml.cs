using BLL;
using BLL.Tools;
using Location.BLL.Tool;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
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
using DbModel.Tools;
using DbModel.Tools.InitInfos;
using LocationClient.Tools;
using Location.Model.InitInfos;
using DbModel.Location.AreaAndDev;

namespace LocationServer.Windows
{
    /// <summary>
    /// Interaction logic for DbConfigureWindow.xaml
    /// </summary>
    public partial class DbConfigureWindow : Window
    {
        public DbConfigureWindow()
        {
            InitializeComponent();
            //Debug.Listeners.Add(new TraceListener());
            Log.NewLogEvent += Log_NewLogEvent;
            Log.StartWatch();
            this.Closing += DbConfigureWindow_Closing   ;
        }

        private void DbConfigureWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Log.StopWatch();
            Log.NewLogEvent -= Log_NewLogEvent;
        }

        private void Log_NewLogEvent(string obj)
        {
            this.Dispatcher.Invoke(new Action(() => {
                TbConsole.Text = obj + "\n" + TbConsole.Text;
                //TbConsole.AppendText(obj);
            }));
        }

        private void MenuInitMSSql_Click(object sender, RoutedEventArgs e)
        {
            //var aa = new AuthorizationArea();
            //aa.Id = 1;
            //aa.Name = "aa";
            //aa.ParentId = 0;
            //aa.Children.Add(new AuthorizationArea() { Id = 2, ParentId = 1, Name = "bb" });
            //XmlSerializeHelper.Save(aa, AppDomain.CurrentDomain.BaseDirectory + "\\Data\\AuthorizationTree.xml");


            //var xf = new XmlFile();
            //xf.Id = 1;
            //xf.Name = "aa";
            //xf.ParentId = 0;
            //xf.Children.Add(new XmlFile() { Id = 2, ParentId = 1, Name = "bb" });
            //XmlSerializeHelper.Save(xf, AppDomain.CurrentDomain.BaseDirectory + "\\Data\\XmlFile.xml");


            AppContext.DeleteDb(0);
            AppContext.InitDbAsync(0, 0, (bll) =>
             {
                 InitImage(bll);
                 MessageBox.Show("初始化完成");
             });
        }

        private void InitImage(Bll bll)
        {
            string strName = "顶视图";
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\Images\\顶视图.png";
            byte[] byteArray = ImageHelper.LoadImageFile(path);
            bll.Pictures.Update(strName, byteArray);
        }

        private void MenuInitSqlite_Click(object sender, RoutedEventArgs e)
        {
            AppContext.DeleteDb(1);
            AppContext.InitDbAsync(1, 0, (bll) =>
            {
                InitImage(bll);
                MessageBox.Show("初始化完成");
            });
        }

        private void MenuInitTopo_Click(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(()=>
            {
                AreaTreeInitializer initializer=new AreaTreeInitializer(new Bll());
                initializer.InitAreaAndDev();
                MessageBox.Show("完成");
            });
            thread.Start();
        }

        private void MenuRemoveArchor_Click(object sender, RoutedEventArgs e)
        {
            DevInfoHelper.RemoveArchorDev();
            MessageBox.Show("完成");
        }

        private void MenuImportDevs_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuInitAA_OnClick(object sender, RoutedEventArgs e)
        {
            DbInitializer initializer = new DbInitializer(new Bll());
            initializer.InitAuthorization();
            MessageBox.Show("完成");
        }

        private void MenuRealPos_OnClick(object sender, RoutedEventArgs e)
        {
            DbInitializer initializer = new DbInitializer(new Bll());
            initializer.InitRealTimePositions();
            MessageBox.Show("完成");
        }

        private void MenuPersonAndCard_OnClick(object sender, RoutedEventArgs e)
        {
            DbInitializer initializer = new DbInitializer(new Bll());
            initializer.InitCardAndPerson();
            MessageBox.Show("完成");
        }

        private void MenuDeleteSqlServer_Click(object sender, RoutedEventArgs e)
        {
            AppContext.DeleteDb(0);
        }

        private void MenuExportArchorData_Click(object sender, RoutedEventArgs e)
        {
            LocationDeviceList list = new LocationDeviceList();
            list.DepList = new List<LocationDevices>();

            Dictionary<int, List<Archor>> dict = new Dictionary<int, List<Archor>>();

            Bll bll = new Bll();
            var archorList=bll.Archors.ToList();
            foreach (var item in archorList)
            {
                int pId = (int)item.ParentId;
                if (!dict.ContainsKey(pId))
                {
                    dict[pId] = new List<Archor>();
                }
                dict[pId].Add(item);
            }

            foreach (var item in dict.Keys)
            {
                var area = bll.Areas.Find(item);
                var archors = dict[item];
                LocationDevices devs = new LocationDevices();
                devs.DevList = new List<LocationDevice>();
                devs.Name = area.Name;

                list.DepList.Add(devs);
                foreach (var archor in archors)
                {
                    var dev = new LocationDevice();
                    dev.AbsolutePosX = archor.X.ToString();
                    dev.AbsolutePosY = archor.Y.ToString();
                    dev.AbsolutePosZ = archor.Z.ToString();
                    dev.AnchorId = archor.Code;
                    dev.IP = archor.Ip;
                    dev.Name = archor.Name;
                    dev.XPos = archor.DevInfo.PosX.ToString();
                    dev.YPos = archor.DevInfo.PosY.ToString();
                    dev.ZPos= archor.DevInfo.PosZ.ToString();
                    devs.DevList.Add(dev);
                }
            }

            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = basePath + "Data\\基站信息\\基站信息.xml";

            XmlSerializeHelper.Save(list,filePath);
        }
    }
}
