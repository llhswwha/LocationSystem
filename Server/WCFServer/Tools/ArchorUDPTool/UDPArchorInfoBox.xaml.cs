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

namespace ArchorUDPTool
{
    /// <summary>
    /// UDPArchorInfoBox.xaml 的交互逻辑
    /// </summary>
    public partial class UDPArchorInfoBox : UserControl
    {
        public string Label
        {
            get
            {
                return LbLabel.Content.ToString();
            }
            set
            {
                LbLabel.Content = value;
            }
        }

        public string Value
        {
            get
            {
                return TbValue.Text;
            }
            set
            {
                TbValue.Text = value;
            }
        }

        public string GetCmd { get; set; }

        public string SetCmd { get; set; }

        public UDPArchorInfoBox()
        {
            InitializeComponent();
        }

        private void BtnGet_Click(object sender, RoutedEventArgs e)
        {
            var am = new ArchorManager();
            //am.ScanArchor();
        }

        private void BtnSet_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
