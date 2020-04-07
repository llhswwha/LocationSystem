using ArchorUDPTool;
using DbModel.Location.AreaAndDev;
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
            if (archorList == null)
            {
                MessageBox.Show("基站列表为空");
            }
            else
            {
                try
                {
                    ArchorConfigureBox1.DbArchorList = archorList;
                }
                catch (Exception ex)
                {
                    Log.Error("设置基站列表出出错:" + ex);
                    MessageBox.Show("设置基站列表出出错:"+ex);
                }
                
            }
            
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
