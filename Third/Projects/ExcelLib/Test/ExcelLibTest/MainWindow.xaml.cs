using System;
using System.Data;
using System.IO;
using System.Windows;
using ExcelLib;

namespace ExcelLibTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuOpenFile_OnClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "BBB.xlsx";
            //DataTable table=ExcelHelper.LoadTable(new FileInfo(path), "Table1", false);
            DataSet set=ExcelHelper.Load(new FileInfo(path), false);
            DataGrid1.DataContext = set.Tables[0];
        }
    }
}
