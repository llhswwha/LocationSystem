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
using WCFServiceForWPF.LocationServices;

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
            ListBox1.ItemsSource = devList;
            ListBox1.DisplayMemberPath = "Name";
        }
    }
}
