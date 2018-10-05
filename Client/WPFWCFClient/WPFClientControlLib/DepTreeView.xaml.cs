using Location.TModel.Location.Person;
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
    /// DepTreeView.xaml 的交互逻辑
    /// </summary>
    public partial class DepTreeView : UserControl
    {
        public DepTreeView()
        {
            InitializeComponent();
        }

        public void LoadData(Department root)
        {
            TreeView1.ItemsSource = root.Children;
        }
    }
}
