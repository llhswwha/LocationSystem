using System;
using System.Collections;
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
using TModel.Location.AreaAndDev;

namespace WPFClientControlLib
{
    /// <summary>
    /// Interaction logic for AchorListBox.xaml
    /// </summary>
    public partial class TAchorListBox : UserControl
    {
        public TAchorListBox()
        {
            InitializeComponent();
        }

        public void LoadData(Archor[] list)
        {
            DataGrid1.ItemsSource = list;
        }

        private void DataGrid1_OnCellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var archor2 = DataGrid1.SelectedItem as Archor;
            if (archor2 == null) return;
            ChangedArchors.Add(archor2);
        }


        public List<Archor> ChangedArchors = new List<Archor>();

        public object SelectedItem
        {
            get { return DataGrid1.SelectedItem; }
        }

        public IEnumerable ItemsSource
        {
            get { return DataGrid1.ItemsSource; }
            set { DataGrid1.ItemsSource = value; }
        }
    }
}
