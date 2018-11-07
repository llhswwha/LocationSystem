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

        private void AreaCanvasWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
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
                AreaListBox1.LoadData(bll.Areas.ToList());
        }

        private void LoadAreaTree()
        {
            var tree = bll.GetAreaTree();
            TopoTreeView1.LoadData(tree);
            TopoTreeView1.Tree.SelectedItemChanged += Tree_SelectedItemChanged;
            TopoTreeView1.ExpandLevel(2);
            TopoTreeView1.SelectFirst();
        }

        private Area area;

        private void Tree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            area = TopoTreeView1.SelectedObject as Area;
            if (area == null) return;
            AreaCanvas1.ShowDev = true;
            AreaCanvas1.ShowArea(area);
            AreaListBox1.LoadData(area.Children);
            DeviceListBox1.LoadData(area.LeafNodes);

            ArchorListExportControl1.Clear();
            TabControl1.SelectionChanged -= TabControl1_OnSelectionChanged;
            TabControl1.SelectionChanged += TabControl1_OnSelectionChanged;

            if (TabControl1.SelectedIndex == 2)
            {
                ArchorListExportControl1.LoadData(area);
                TabControl1.SelectionChanged -= TabControl1_OnSelectionChanged;
            }
        }

        private void AreaListBox1_SelectedItemChanged(Area obj)
        {
            AreaCanvas1.SelectArea(obj);
        }

        private void DeviceListBox1_SelectedItemChanged(DevInfo obj)
        {
            AreaCanvas1.SelectDev(obj);
        }

        private void AreaCanvas1_DevSelected(Rectangle rect,DevInfo obj)
        {
            if (obj.Parent.IsPark())//电厂
            {
                var bound = obj.Parent.InitBound;
                if (bound.Points == null)
                {
                    bound.Points = new Bll().Points.FindAll(i => i.BoundId == bound.Id);
                }
                var leftBottom= bound.GetLeftBottomPoint();

                var win2 = new ParkArchorSettingWindow();
                ParkArchorSettingWindow.ZeroX = leftBottom.X;
                ParkArchorSettingWindow.ZeroY = leftBottom.Y;
                win2.Owner = this;
                win2.Show();
                
                if (win2.ShowInfo(rect, obj) == false)
                {
                    win2.Close();
                    return;
                }
                win2.RefreshDev += (dev) =>
                {
                    AreaCanvas1.RefreshDev(dev);
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
                if (win2.ShowInfo(rect, obj) == false)
                {
                    win2.Close();
                    return;
                }
                win2.RefreshDev += (dev) =>
                {
                    AreaCanvas1.RefreshDev(dev);
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
                ArchorListExportControl1.LoadData(area);
                TabControl1.SelectionChanged -= TabControl1_OnSelectionChanged;
            }
        }
    }
}
