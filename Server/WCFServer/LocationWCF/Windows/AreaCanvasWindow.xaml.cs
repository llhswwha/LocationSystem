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
            TopoTreeView1.ExpandLevel(2);
            TopoTreeView1.Tree.SelectedItemChanged += Tree_SelectedItemChanged;
            TopoTreeView1.SelectFirst();
        }

        private void Tree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Area area = TopoTreeView1.SelectedObject as Area;
            if (area == null) return;
            AreaCanvas1.ShowArea(area);
            AreaListBox1.LoadData(area.Children);
            DeviceListBox1.LoadData(area.LeafNodes);
        }

        private void AreaListBox1_SelectedItemChanged(Area obj)
        {
            AreaCanvas1.SelectArea(obj);
        }

        private void DeviceListBox1_SelectedItemChanged(DevInfo obj)
        {
            AreaCanvas1.SelectDev(obj);
        }
    }
}
