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

namespace WPFClientControlLib
{
    /// <summary>
    /// Interaction logic for BoundInfoWindow.xaml
    /// </summary>
    public partial class ItemInfoWindow : Window
    {
        public ItemInfoWindow()
        {
            InitializeComponent();
        }

        public void LoadData(object data)
        {
            if (data == null) return;
            DataGrid1.ItemsSource = new [] {data};
        }
    }
}
