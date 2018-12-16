using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BLL;
using DbModel.Location.AreaAndDev;
using DbModel.LocationHistory.Data;
using DbModel.Tools;
using Location.TModel.Location.AreaAndDev;
using LocationServer;
using LocationServices.Converters;
using LocationServices.Locations;
using LocationServices.Locations.Services;
using TModel.Tools;

namespace PositionSimulation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SimulationWindow : Window
    {
        private Bll bll;

        private SimulationServer server;

        public SimulationWindow()
        {
            InitializeComponent();
            bll = AppContext.GetLocationBll();
        }

        private void SimulationWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            AreaCanvas1.Init();
            
            InitCbList();

            LoadData();
        }

        private void LoadData()
        {
            LoadAreaTree();
            LoadDepTree();
        }

        private void InitCbList()
        {
            CbIpList.ItemsSource = IpHelper.GetLocalList();
            CbIpList.SelectedIndex = 0;

            TbPort.ItemsSource = new int[] {3455, 3456};
            TbPort.SelectedIndex = 0;
        }


        private void SimulationWindow_OnClosed(object sender, EventArgs e)
        {
            StopServer();
        }

        private void StopServer()
        {
            if (server != null)
            {
                server.Stop();
                server = null;
            }
            if (serverThread != null)
            {
                serverThread.Abort();
            }
        }

        private Thread serverThread;

        private void StartServer(IPAddress localIP, int localPort)
        {
            if (server == null)
            {
                server = new SimulationServer();
                server.Start(localIP, localPort);
            }

            if (serverThread == null)
            {
                serverThread = ThreadTool.Start(() =>
                {
                    while (true)
                    {
                        for (int i = 0; i < AreaCanvas1.PersonShapeList.Count; i++)
                        {
                            var ps = AreaCanvas1.PersonShapeList[i];
                            ps.SavePos();
                            var tPos = ps.Pos;
                            var dbPos = tPos.ToDbModel();

                            Position pos = new Position();
                            pos.SetTime();
                            pos.SetProperty(dbPos);
                            server.Send(pos.GetText());
                        }

                        Thread.Sleep(250);
                    }
                });
            }
        }

        private void LoadDepTree()
        {
            var service = new LocationService();
            var tree = service.GetDepartmentTree();
            DepTreeView1.LoadData(tree);
        }

        private void LoadAreaTree()
        {
            var service = new AreaService();
            var tree = service.GetTree(1);
            TopoTreeView1.LoadData(tree);
            TopoTreeView1.ExpandLevel(2);
            TopoTreeView1.SelectedObjectChanged += TopoTreeView1_SelectedObjectChanged;
            TopoTreeView1.SelectFirst();
        }

        private void TopoTreeView1_SelectedObjectChanged(object obj)
        {
            area = obj as PhysicalTopology;
            if (area == null) return;
            AreaCanvas1.ShowDev = true;
            AreaCanvas1.ShowArea(area);
            var service = new PersonService();
            var persons = service.GetListByArea(area.Id + "");
            if (persons == null)
            {
                persons = service.GetListByArea("");
            }
            AreaCanvas1.ShowPersons(persons);
        }

        private PhysicalTopology area;

        private void BtnStartListen_OnClick(object sender, RoutedEventArgs e)
        {
            if (BtnStartListen.Content.ToString() == "开始")
            {
                string ip = CbIpList.Text;
                string port = TbPort.Text;
                StartServer(IPAddress.Parse(ip),port.ToInt());
                BtnStartListen.Content = "结束";
            }
            else
            {
                StopServer();
                BtnStartListen.Content = "开始";
            }
        }

        private void MenuRefresh_OnClick(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void MenuPersonSetting_OnClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void MenuSavePos_OnClick(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < AreaCanvas1.PersonShapeList.Count; i++)
            {
                var ps = AreaCanvas1.PersonShapeList[i];
                ps.SavePos();
                var tPos = ps.Pos;
                var dbPos = tPos.ToDbModel();
                var dbPos2 = bll.LocationCardPositions.FindByCode(dbPos.Id);
                dbPos2.Edit(dbPos);
                if (bll.LocationCardPositions.Edit(dbPos2) == false)
                {
                    MessageBox.Show("保存失败");
                    return;
                }

                //Position pos = new Position();
                //pos.SetTime();
                //pos.SetProperty(dbPos);
                //server.Send(pos.GetText());
            }
        }
    }
}
