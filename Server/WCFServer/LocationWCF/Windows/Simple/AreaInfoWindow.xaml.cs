using LocationServer.Windows.Simple;
using LocationServices.Locations.Services;
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
using TEntity = Location.TModel.Location.AreaAndDev.PhysicalTopology;

namespace LocationServer.Windows
{
    /// <summary>
    /// Interaction logic for AreaInfoWindow.xaml
    /// </summary>
    public partial class AreaInfoWindow : Window
    {
        public AreaInfoWindow()
        {
            InitializeComponent();
        }

        private void ArchorListWindow_OnLoaded(object sender, RoutedEventArgs e)
        {

        }

        private object _item;

        public void ShowInfo(object item)
        {
            _item = item;
            PropertyGrid1.SelectedObject = _item;
        }

        private void MenuSave_Click(object sender, RoutedEventArgs e)
        {
            var areaService = new AreaService();
            var area = areaService.Put(_item as TEntity);
            if (area == null)
            {
                MessageBox.Show("保存失败");
            }
            else
            {
                MessageBox.Show("保存成功");
            }
        }

        private void MenuInitBound_Click(object sender, RoutedEventArgs e)
        {
            var area = _item as TEntity;
            var win = new BoundWindow();
            win.Bound = area.InitBound;
            win.Show();
        }
    }
}
