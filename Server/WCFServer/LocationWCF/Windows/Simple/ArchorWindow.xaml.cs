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
using LocationServices.Locations.Services;
using TEntity = TModel.Location.AreaAndDev.Archor;

namespace LocationServer.Windows.Simple
{
    /// <summary>
    /// ArchorWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ArchorWindow : Window
    {
        public ArchorWindow()
        {
            InitializeComponent();
        }

        ArchorService service = new ArchorService();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataGrid1.ItemsSource = service.GetList();
            CbKey.ItemsSource = new String[] {"Code", "Ip","Name", "Area"};
            CbKey.SelectedIndex = 0;
        }

        private void BtnSearch_OnClick(object sender, RoutedEventArgs e)
        {
            string key = CbKey.SelectedItem.ToString();
            string value = TbValue.Text;
            DataGrid1.ItemsSource = service.Search(key, value);
        }

        private void MenuLocalArchor_Click(object sender, RoutedEventArgs e)
        {
            var list = new List<int>();
            foreach (var item in DataGrid1.SelectedItems)
            {
                var archor = item as TEntity;
                //list.Add(archor);
                list.Add(archor.Id);
            }
            var win = new AreaCanvasWindow(list.ToArray());
            win.Show();
        }
    }
}
