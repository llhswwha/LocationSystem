using Location.BLL.Tool;
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
using WPFClientControlLib;

namespace LocationServer.Controls
{
    /// <summary>
    /// LogTextBox.xaml 的交互逻辑
    /// </summary>
    public partial class LogTextBox : UserControl
    {
        public LogTextBox()
        {
            InitializeComponent();
        }

        public LogTextBoxController controller;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        public LogTextBoxController AddLog(LogInfo info)
        {
            if (controller == null)
            {
                controller = new LogTextBoxController(TbLog, info.Tag);
                controller.AddLog(info);
            }
            return controller;
        }

        //public void Init(string tag)
        //{
        //    controller = new LogTextBoxController(TbLog, tag);
        //}

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (controller != null)
            {
                controller.Dispose();
            }
        }
    }
}
