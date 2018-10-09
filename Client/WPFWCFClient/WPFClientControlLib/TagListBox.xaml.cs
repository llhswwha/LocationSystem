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
using Location.TModel.Location.AreaAndDev;

namespace WPFClientControlLib
{
    /// <summary>
    /// Interaction logic for TagListBox.xaml
    /// </summary>
    public partial class TagListBox : UserControl
    {
        public TagListBox()
        {
            InitializeComponent();
        }

        public void LoadData(Tag[] list)
        {
            DataGrid1.ItemsSource = list;
        }
    }
}
