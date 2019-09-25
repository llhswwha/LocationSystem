using ArchorUDPTool;
using DbModel.Location.AreaAndDev;
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

namespace LocationServer.Models.EngineTool
{
    /// <summary>
    /// ArchorConfigureWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ArchorConfigureWindow : Window
    {
        public ArchorConfigureWindow()
        {
            InitializeComponent();
        }

        public ArchorConfigureWindow(List<ArchorInfo> archorList)
        {
            InitializeComponent();
            ArchorConfigureBox1.DbArchorList = archorList;
        }

        public ArchorManager archorManager { get
            {
                return ArchorConfigureBox1.archorManager;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            ArchorConfigureBox1.Close();
        }
    }
}
