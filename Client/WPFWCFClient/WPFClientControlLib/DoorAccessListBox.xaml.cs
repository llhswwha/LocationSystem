using Location.TModel.Location.AreaAndDev;
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
    /// DoorAccessListBox.xaml 的交互逻辑
    /// </summary>
    public partial class DoorAccessListBox : UserControl
    {
        public DoorAccessListBox()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 载入数据
        /// </summary>
        /// <param name="list"></param>
        public void LoadData(Dev_DoorAccess[] list)
        {
            DataGrid1.ItemsSource = list;
        }

        /// <summary>
        /// 添加右键操作菜单
        /// </summary>
        /// <param name="menuHeader"></param>
        /// <param name="clickAction"></param>
        public void AddMenu(string menuHeader, RoutedEventHandler clickAction)
        {
            MenuItem menu = new MenuItem() { Header = menuHeader };
            menu.Click += clickAction;
            if (DataGrid1.ContextMenu == null) DataGrid1.ContextMenu = new ContextMenu();
            DataGrid1.ContextMenu.Items.Add(menu);
        }

        /// <summary>
        /// 当前选中设备
        /// </summary>
        public Dev_DoorAccess CurrentDev
        {
            get
            {
                return DataGrid1.SelectedItem as Dev_DoorAccess;
            }
        }
    }
}
