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
//using DevEntity=DbModel.Location.AreaAndDev.DevInfo;
using DevEntity = Location.TModel.Location.AreaAndDev.DevInfo;
namespace WPFClientControlLib
{
    /// <summary>
    /// Interaction logic for DbDeviceListBox.xaml
    /// </summary>
    public partial class DbDeviceListBox : UserControl
    {
        public DbDeviceListBox()
        {
            InitializeComponent();
        }
        public void LoadData(IEnumerable<DevEntity> list)
        {
            DataGrid1.ItemsSource = list;
        }

        private void DataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = DataGrid1.SelectedItem as DevEntity;
            //if (item == null) return;
            if (SelectedItemChanged != null)
            {
                SelectedItemChanged(item);
            }
        }

        public event Action<DevEntity> SelectedItemChanged;
    }
}
