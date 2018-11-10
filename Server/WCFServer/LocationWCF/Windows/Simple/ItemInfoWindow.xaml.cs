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

namespace LocationServer.Windows
{
    /// <summary>
    /// ItemInfoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ItemInfoWindow : Window
    {
        public ItemInfoWindow()
        {
            InitializeComponent();
        }

        public ItemInfoWindow(object item)
        {
            InitializeComponent();
            ShowInfo(item);
        }

        public void ShowInfo(object item)
        {
            PropertyGrid1.SelectedObject = item;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
