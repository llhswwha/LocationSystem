using Location.TModel.Location.AreaAndDev;
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
    /// ResourceTreeView.xaml 的交互逻辑
    /// </summary>
    public partial class ResourceTreeView : UserControl
    {
        public ResourceTreeView()
        {
            InitializeComponent();
        }

        public void LoadData(PhysicalTopology tree1, Department tree2)
        {
            TopoTreeView1.LoadData(tree1);
            DepTreeView1.LoadData(tree2);
        }
    }
}
