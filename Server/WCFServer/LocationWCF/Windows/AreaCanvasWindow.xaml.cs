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
using LocationServer.Windows;
using LocationServices.Converters;
using LocationServices.Locations.Services;

//using AreaEntity= DbModel.Location.AreaAndDev.Area;
using AreaEntity = Location.TModel.Location.AreaAndDev.PhysicalTopology;
//using DevEntity=DbModel.Location.AreaAndDev.DevInfo;
using DevEntity = Location.TModel.Location.AreaAndDev.DevInfo;
using PersonEntity = Location.TModel.Location.Person.Personnel;

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

        private void AreaCanvasWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            areaService = new AreaService();
            AreaCanvas1.Init();
            LoadData();
        }

        public void LoadData()
        {
            LoadAreaTree();

            //LoadAreaList();
        }

        private void LoadAreaList()
        {
                AreaListBox1.LoadData(areaService.GetList());
        }

        private void LoadAreaTree()
        {
            var tree = areaService.GetTree(1);
            TopoTreeView1.LoadData(tree);
            TopoTreeView1.Tree.SelectedItemChanged += Tree_SelectedItemChanged;
            TopoTreeView1.ExpandLevel(2);
            TopoTreeView1.SelectFirst();
        }

        private AreaEntity area;

        private void Tree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            area = TopoTreeView1.SelectedObject as AreaEntity;
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
            var service = new PersonService();
            var persons = service.GetListByArea(area.Id + "");
            if (persons == null)
            {
                persons = service.GetListByArea("");
            }
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
            var parentArea = areaService.GetEntity(obj.ParentId+"");
            if (parentArea.IsPark())//电厂
            {
                var bound = parentArea.InitBound;
                //if (bound.Points == null)
                //{
                //    bound.Points = new Bll().Points.FindAll(i => i.BoundId == bound.Id);
                //}
                var leftBottom= bound.GetLeftBottomPoint();

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
                win2.RefreshDev += (dev) =>
                {
                    AreaCanvas1.RefreshDev(dev.ToTModel());
                };
                win2.ShowPointEvent += (x, y) =>
                {
                    AreaCanvas1.ShowPoint(x, y);
                };
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
                win2.RefreshDev += (dev) =>
                {
                    AreaCanvas1.RefreshDev(dev.ToTModel());
                };
                win2.ShowPointEvent += (x, y) =>
                {
                    AreaCanvas1.ShowPoint(x, y);
                };
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
    }
}
