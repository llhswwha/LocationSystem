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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BLL;
using DbModel.Location.AreaAndDev;
using DbModel.LocationHistory.Data;
using DbModel.Tools;
using LocationServer;
using LocationServices.Locations;
using LocationServices.Locations.Services;

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
            LoadAreaTree();
            LoadDepTree();
            StartServer();
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

        private void StartServer()
        {
            if (server == null)
            {
                server = new SimulationServer();
                server.Start();
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
                            Position pos = new Position();
                            pos.SetTime();
                            var tPos = ps.Person.Pos;
                            pos.Code = tPos.Tag;
                            pos.X = tPos.X;
                            pos.Y = tPos.Z;
                            pos.Z = tPos.Y;
                            pos.Power = tPos.Power;
                            pos.Number = i;
                            pos.Flag = tPos.Flag;
                            pos.Archors = tPos.Archors;
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
            var tree = bll.GetAreaTree();
            TopoTreeView1.LoadData(tree);
            TopoTreeView1.ExpandLevel(2);
            TopoTreeView1.Tree.SelectedItemChanged += Tree_SelectedItemChanged;
            TopoTreeView1.SelectFirst();
        }

        private Area area;

        private void Tree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            area = TopoTreeView1.SelectedObject as Area;
            if (area == null) return;
            //AreaCanvas1.ShowDev = true;
            AreaCanvas1.ShowArea(area);


            var service = new PersonService();
            var persons=service.GetListByArea(area.Id+"");

            AreaCanvas1.ShowPersons(persons);
        }
    }
}
