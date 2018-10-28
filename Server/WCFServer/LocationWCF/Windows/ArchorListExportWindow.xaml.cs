using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
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
using DbModel.Location.AreaAndDev;
using ExcelLib;
using Location.TModel.FuncArgs;

namespace LocationServer
{
    /// <summary>
    /// Interaction logic for ArchorListExportWindow.xaml
    /// </summary>
    public partial class ArchorListExportWindow : Window
    {
        public ArchorListExportWindow()
        {
            InitializeComponent();
        }

        
        private void ArchorListExportWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            ArchorListExportControl1.LoadData();
        }

    }
}
