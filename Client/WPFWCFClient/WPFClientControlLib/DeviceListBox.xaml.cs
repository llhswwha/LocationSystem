using Location.TModel.Location.AreaAndDev;
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

namespace WPFClientControlLib
{
    /// <summary>
    /// DeviceListBox.xaml 的交互逻辑
    /// </summary>
    public partial class DeviceListBox : UserControl
    {
        public DeviceListBox()
        {
            InitializeComponent();
        }

        public void LoadData(DevInfo[] devList)
        {
            DataGrid1.ItemsSource = devList;
        }

        private void MenuDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        public void AddMenu(string menuHeader, RoutedEventHandler clickAction)
        {
            MenuItem menu = new MenuItem() { Header = menuHeader };
            menu.Click += clickAction;
            DataGrid1.ContextMenu.Items.Add(menu);
        }

        public DevInfo CurrentDev
        {
            get
            {
                return DataGrid1.SelectedItem as DevInfo;
            }
        }
    }
}
