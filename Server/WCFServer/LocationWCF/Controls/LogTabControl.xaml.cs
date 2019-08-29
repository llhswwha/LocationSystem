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

namespace LocationServer.Controls
{
    /// <summary>
    /// LogTabControl.xaml 的交互逻辑
    /// </summary>
    public partial class LogTabControl : UserControl
    {
        public LogTabControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private Dictionary<string, LogTextBox> items = new Dictionary<string, LogTextBox>();

        public void AddLog(LogInfo info)
        {
            if (!items.ContainsKey(info.Tag))
            {
                var max = AppContext.LogTextBoxMaxLength;

                LogTextBox tb = new LogTextBox();

                TabItem tabItem = new TabItem();
                tabItem.Header = info.Tag;
                tabItem.Content = tb;

                TabControl1.Items.Add(tabItem);
                var controller = tb.AddLog(info, max);
                controller.LogChanged += (i, c) =>
                {
                    tabItem.Header = string.Format("{0}({1})", i.Tag, c);
                };
                items.Add(info.Tag, tb);
            }
        }
    }
}
