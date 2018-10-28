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
        public void LoadData(IEnumerable<DevInfo> list)
        {
            DataGrid1.ItemsSource = list;
        }

        private void DataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DevInfo item = DataGrid1.SelectedItem as DevInfo;
            //if (item == null) return;
            if (SelectedItemChanged != null)
            {
                SelectedItemChanged(item);
            }
        }

        public event Action<DevInfo> SelectedItemChanged;
    }
}
