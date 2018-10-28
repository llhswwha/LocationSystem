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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DbModel.Location.AreaAndDev;
using DbModel.Tools;

namespace WPFClientControlLib
{
    /// <summary>
    /// Interaction logic for AreaListBox.xaml
    /// </summary>
    public partial class AreaListBox : UserControl
    {
        public AreaListBox()
        {
            InitializeComponent();
        }

        public IEnumerable<Area> Data { get; set; }

        public void LoadData(IEnumerable<Area> list)
        {
            Data = list;
            DataGrid1.ItemsSource = Data;
        }

        private void MenuInitBound_OnClick(object sender, RoutedEventArgs e)
        {
            Area area = DataGrid1.SelectedItem as Area;
            if (area == null) return;
            var wnd = new ItemInfoWindow();
            wnd.LoadData(area.InitBound);
            wnd.Show();
        }

        private void MenuEditBound_OnClick(object sender, RoutedEventArgs e)
        {
            Area area = DataGrid1.SelectedItem as Area;
            if (area == null) return;
            var wnd = new ItemInfoWindow();
            wnd.LoadData(area.EditBound);
            wnd.Show();
        }

        private void MenuItemProperty_OnClick(object sender, RoutedEventArgs e)
        {
            Area area = DataGrid1.SelectedItem as Area;
            if (area == null) return;
            var wnd = new ItemInfoWindow();
            wnd.LoadData(area);
            wnd.Show();
        }

        private void CbOnlyShowAbsoluteArea_OnChecked(object sender, RoutedEventArgs e)
        {
            DataGrid1.ItemsSource = Data.Where(i=>i.IsRelative==false);
        }

        private void CbOnlyShowAbsoluteArea_OnUnchecked(object sender, RoutedEventArgs e)
        {
            DataGrid1.ItemsSource = Data;
        }

        private void CbOnlyShowBuilding_OnChecked(object sender, RoutedEventArgs e)
        {
            DataGrid1.ItemsSource = Data.Where(i => i.Type == AreaTypes.大楼);
        }

        private void CbOnlyShowBuilding_OnUnchecked(object sender, RoutedEventArgs e)
        {
            DataGrid1.ItemsSource = Data;
        }

        private void DataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Area item = DataGrid1.SelectedItem as Area;
            //if (item == null) return;
            if (SelectedItemChanged != null)
            {
                SelectedItemChanged(item);
            }
        }

        public event Action<Area> SelectedItemChanged;
    }
}
