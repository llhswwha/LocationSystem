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
using System.Windows.Shapes;
using BLL;
using DbModel.Location.AreaAndDev;
using Location.TModel.Location.AreaAndDev;
using LocationServer.Windows;
using LocationServices.Converters;
using LocationServices.Locations.Services;

//using AreaEntity= DbModel.Location.AreaAndDev.Area;
using AreaEntity = Location.TModel.Location.AreaAndDev.PhysicalTopology;
//using DevEntity=DbModel.Location.AreaAndDev.DevInfo;
using DevEntity = Location.TModel.Location.AreaAndDev.DevInfo;
using PersonEntity = Location.TModel.Location.Person.Personnel;
using WPFClientControlLib.Extensions;
using System.Windows.Threading;

namespace LocationServer
{
    /// <summary>
    /// Interaction logic for AreaCanvas.xaml
    /// </summary>
    public partial class AreaCanvasWindow : Window
    {
        private Bll bll;

        public AreaCanvasWindow()
        {
            InitializeComponent();
            bll = AppContext.GetLocationBll();
        }

        private AreaService areaService;
        private DepartmentService depService;

        private void AreaCanvasWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            areaService = new AreaService();
            depService = new DepartmentService();

            InitAreaCanvas();
            LoadData();
            //StartPersonTimer();
        }

        private void PersonTimer_Tick(object sender, EventArgs e)
        {
            ShowPersons();
            LoadPersonTree();
        }

        DispatcherTimer personTimer;

        void StartPersonTimer()
        {
            if (personTimer == null)
            {
                personTimer = new DispatcherTimer();
                personTimer.Interval = TimeSpan.FromMilliseconds(250);
                personTimer.Tick += PersonTimer_Tick;
                personTimer.Start();
            }
        }

        void StopPersonTimer()
        {
            if (personTimer != null)
            {
                personTimer.Stop();
                personTimer = null;
            }
        }

        void InitAreaCanvas()
        {
            AreaCanvas1.Init();
            ContextMenu devContextMenu = new ContextMenu();
            devContextMenu.AddMenu("设置", () =>
            {
                SetDevInfo(AreaCanvas1.SelectedDev, AreaCanvas1.SelectedDev.Tag as DevEntity);
            });
            devContextMenu.Items.Add(new MenuItem() { Header = "删除" });
            AreaCanvas1.DevContextMenu = devContextMenu;
            ContextMenu areaContextMenu = new ContextMenu();
            areaContextMenu.AddMenu("设置", () =>
            {
                var win = new AreaInfoWindow();
                win.Show();
                var area = AreaCanvas1.SelectedArea;
                if (area.Children == null)
                {
                    area.Children = areaService.GetListByPid(area.Id + "");
                }
                win.ShowInfo(area);
            });
            areaContextMenu.Items.Add(new MenuItem() { Header = "删除" });
            AreaCanvas1.AreaContextMenu = areaContextMenu;
        }

        public void LoadData()
        {
            LoadAreaTree();
            LoadDepTree();
            LoadPersonTree();
            //LoadAreaList();
        }

        private void LoadAreaList()
        {
                AreaListBox1.LoadData(areaService.GetList());
        }

        private void LoadAreaTree()
        {
            var tree = areaService.GetTree(1);
            var topoTree = ResourceTreeView1.TopoTree;
            topoTree.LoadData(tree);
            topoTree.Tree.SelectedItemChanged += Tree_SelectedItemChanged;
            topoTree.ExpandLevel(2);
            topoTree.SelectFirst();
        }

        private void LoadDepTree()
        {
            var tree = depService.GetTree(1);
            var depTree = ResourceTreeView1.DepTree;
            depTree.LoadData(tree);
            depTree.ExpandLevel(2);
        }

        private void LoadPersonTree()
        {
            var tree = areaService.GetBasicTree(2);
            var depTree = ResourceTreeView1.PersonTree;
            depTree.LoadData(tree);
            depTree.ExpandLevel(2);

            var persons=tree.GetAllPerson();
        }

        private AreaEntity area;

        private void Tree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            area = ResourceTreeView1.TopoTree.SelectedObject as AreaEntity;
            if (area == null) return;
            AreaCanvas1.ShowDev = true;
            AreaCanvas1.ShowArea(area);
            AreaListBox1.LoadData(area.Children);
            DeviceListBox1.LoadData(area.LeafNodes);

            ShowPersons();

            ArchorListExportControl1.Clear();
            TabControl1.SelectionChanged -= TabControl1_OnSelectionChanged;
            TabControl1.SelectionChanged += TabControl1_OnSelectionChanged;

            if (TabControl1.SelectedIndex == 2)
            {
                ArchorListExportControl1.LoadData(area.Id);
                TabControl1.SelectionChanged -= TabControl1_OnSelectionChanged;
            }
        }

        private void ShowPersons()
        {
            if (area == null) return;
            if (AreaCanvas1 == null) return;
            var service = new PersonService();
            //var persons = service.GetListByArea(area.Id + "");
            //if (persons == null)
            //{
            //    persons = service.GetListByArea("");
            //}
            var persons = service.GetList(true);
            AreaCanvas1.ShowPersons(persons);
        }

        private void AreaListBox1_SelectedItemChanged(AreaEntity obj)
        {
            AreaCanvas1.SelectArea(obj);
        }

        private void DeviceListBox1_SelectedItemChanged(DevEntity obj)
        {
            AreaCanvas1.SelectDev(obj);
        }

        private void AreaCanvas1_DevSelected(Rectangle rect, DevEntity obj)
        {
            //SetDevInfo(rect, obj);
        }

        private void SetDevInfo(Rectangle rect, DevEntity obj)
        {
            var parentArea = areaService.GetEntity(obj.ParentId + "");
            if (parentArea.IsPark()) //电厂
            {
                var bound = parentArea.InitBound;
                //if (bound.Points == null)
                //{
                //    bound.Points = new Bll().Points.FindAll(i => i.BoundId == bound.Id);
                //}
                var leftBottom = bound.GetLeftBottomPoint();

                var win2 = new ParkArchorSettingWindow();
                ParkArchorSettingWindow.ZeroX = leftBottom.X;
                ParkArchorSettingWindow.ZeroY = leftBottom.Y;
                win2.Owner = this;
                win2.Show();

                if (win2.ShowInfo(rect, obj.Id) == false)
                {
                    win2.Close();
                    return;
                }
                win2.RefreshDev += (dev) => { AreaCanvas1.RefreshDev(dev.ToTModel()); };
                win2.ShowPointEvent += (x, y) => { AreaCanvas1.ShowPoint(x, y); };
            }
            else
            {
                var win2 = new RoomArchorSettingWindow();
                win2.Owner = this;
                win2.Show();
                if (win2.ShowInfo(rect, obj.Id) == false)
                {
                    win2.Close();
                    return;
                }
                win2.RefreshDev += (dev) => { AreaCanvas1.RefreshDev(dev.ToTModel()); };
                win2.ShowPointEvent += (x, y) => { AreaCanvas1.ShowPoint(x, y); };
            }
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            
        }

        private void TabControl1_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TabControl1.SelectedIndex == 2)
            {
                ArchorListExportControl1.LoadData(area.Id);
                TabControl1.SelectionChanged -= TabControl1_OnSelectionChanged;
            }
        }

        private void MenuRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void MenuStartTimer_Click(object sender, RoutedEventArgs e)
        {
            if(MenuStartTimer.Header.ToString()== "启动定时器")
            {
                StartPersonTimer();
                MenuStartTimer.Header = "停止定时器";
            }
            else
            {
                StopPersonTimer();
                MenuStartTimer.Header = "启动定时器";
            }
        }
    }
}
