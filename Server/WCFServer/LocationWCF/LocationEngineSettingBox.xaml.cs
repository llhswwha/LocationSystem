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
using BLL;

namespace LocationServer
{
    /// <summary>
    /// Interaction logic for LocationEngineSettingBox.xaml
    /// </summary>
    public partial class LocationEngineSettingBox : UserControl
    {
        public LocationEngineSettingBox()
        {
            InitializeComponent();
        }

        public void LoadData()
        {
            Bll bll = AppContext.GetLocationBll();
            var archors1 = bll.bus_anchors.ToList();
            DataGridArchor1.ItemsSource = archors1;

            var tags = bll.bus_tags.ToList();
            DataGrid2.ItemsSource = tags;

            var archors2 = bll.Archors.ToList();
            DataGridArchor2.ItemsSource = archors2;
        }

        private void BtnModifyArchor1FromArchor2_OnClick(object sender, RoutedEventArgs e)
        {
            Bll bll = AppContext.GetLocationBll();
            var archors2 = bll.Archors.ToList();
            bll.bus_anchors.UpdateList(archors2);
            bll.Archors.EditRange(archors2);
            LoadData();
        }

        private void BtnBindingArchor_OnClick(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
