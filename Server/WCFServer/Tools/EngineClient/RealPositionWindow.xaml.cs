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

namespace EngineClient
{
    /// <summary>
    /// Interaction logic for RealPositionWindow.xaml
    /// </summary>
    public partial class RealPositionWindow : Window
    {
        public RealPositionWindow()
        {
            InitializeComponent();
        }

        private void RealPositionWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            Bll bll = new Bll();
            DataGrid1.ItemsSource = bll.LocationCardPositions.ToList();
        }
    }
}
