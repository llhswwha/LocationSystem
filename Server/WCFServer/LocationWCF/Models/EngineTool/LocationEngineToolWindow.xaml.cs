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
using DbModel.Engine;
using DbModel.Location.AreaAndDev;
using LocationServices.Converters;

namespace LocationServer
{
    /// <summary>
    /// Interaction logic for LocationEngineToolWindow.xaml
    /// </summary>
    public partial class LocationEngineToolWindow : Window
    {

        private Bll bll;
        public LocationEngineToolWindow()
        {
            InitializeComponent();
        }

        private List<bus_anchor> changedArchor1 = new List<bus_anchor>();

        public void LoadData()
        {
            changedArchor1.Clear();

            var archors1 = bll.bus_anchors.ToList();
            Group1.Header = "列表1 ["+archors1.Count+"]";
            DataGridArchor1.ItemsSource = archors1;

            var tags = bll.bus_tags.ToList();
            DataGrid2.ItemsSource = tags;

            var archors2 = bll.Archors.ToList();
            Group2.Header = "列表2 [" + archors2.Count + "]";
            DataGridArchor2.ItemsSource = archors2;
        }

        private void UpdateBusAnchors()
        {
            var archors2 = bll.Archors.ToList();
            bll.bus_anchors.UpdateList(archors2);
            bll.Archors.EditRange(archors2);
            LoadData();
        }

        private void LocationEngineToolWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            bll = AppContext.GetLocationBll();
            LoadData();

            var tree=bll.GetAreaTree();
            TopoTreeView1.LoadData(tree);
            TopoTreeView1.Tree.SelectedItemChanged += Tree_SelectedItemChanged;
        }

        private void Tree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Area area = TopoTreeView1.SelectedObject as Area;
            if (area == null) return;
            if (CbOnlyShowListOfDep.IsChecked==true)
            {
                var archors2 = bll.Archors.FindAll(i => i.ParentId==area.Id);
                Group2.Header = "列表2 [" + archors2.Count + "]";
                DataGridArchor2.ItemsSource = archors2;
            }
        }

        private void BtnModifyArchor1FromArchor2_OnClick(object sender, RoutedEventArgs e)
        {
            UpdateBusAnchors();
        }

        private void BtnClearCode_OnClick(object sender, RoutedEventArgs e)
        {
            bll.Archors.ClearCode();
            LoadData();
        }

        private void BtnGenerateCode_OnClick(object sender, RoutedEventArgs e)
        {
            bll.Archors.GenerateCode();
            LoadData();
        }

        private void BtnSetCodeFromArchor1_OnClick(object sender, RoutedEventArgs e)
        {
            var archor1 = DataGridArchor1.SelectedItem as bus_anchor;
            if (archor1 == null) return;
            var archor2 = DataGridArchor2.SelectedItem as Archor;
            if (archor2 == null) return;
            archor2.Code = archor1.anchor_id;
            bll.Archors.Edit(archor2);
            RefreshData();
        }

        private void RefreshData()
        {
            var data = DataGridArchor2.ItemsSource;
            DataGridArchor2.ItemsSource = null;
            DataGridArchor2.ItemsSource = data;
        }

        private void MenuRefresh_OnClick(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void DataGridArchor2_OnCellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
           
        }

        private void BtnSave1_OnClick(object sender, RoutedEventArgs e)
        {
            bll.bus_anchors.EditRange(changedArchor1);
            changedArchor1.Clear();
            LoadData();
        }

        private void DataGridArchor1_OnCellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var archor1 = DataGridArchor1.SelectedItem as bus_anchor;
            if (archor1 == null) return;
            changedArchor1.Add(archor1);
        }

        private void BtnSave2_OnClick(object sender, RoutedEventArgs e)
        {
            bll.Archors.EditRange(DataGridArchor2.ChangedArchors);
            DataGridArchor2.ChangedArchors.Clear();
            LoadData();
        }

        private void CbOnlyShowEmptyCode_OnChecked(object sender, RoutedEventArgs e)
        {
            var archors2 = bll.Archors.FindAll(i => string.IsNullOrEmpty(i.Code));
            Group2.Header = "列表2 [" + archors2.Count + "]";
            DataGridArchor2.ItemsSource = archors2;
        }

        private void CbOnlyShowEmptyCode_OnUnchecked(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void CbOnlyShowListOfDep_OnUnchecked(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void BtnDeleteDataFromList2_OnClick(object sender, RoutedEventArgs e)
        {
            var list = bll.bus_anchors.FindAll(i => i.anchor_id.StartsWith("Code_"));
            bll.bus_anchors.RemoveList(list);
            LoadData();
        }
    }
}
