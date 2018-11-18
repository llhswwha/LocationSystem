using DbModel.Tools;
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

namespace ArchorUDPTool
{
    /// <summary>
    /// CodeTestWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CodeTestWindow : Window
    {
        public CodeTestWindow()
        {
            InitializeComponent();
            Tb1.Text = @"10 01 20 03 04 02 02 02 02 4F 7F A4 F8
10 01 20 03 04 00 00 00 00 3A CA BB B3
10 01 20 03 04 00 00 00 01 4D CD 8B 25
10 01 20 03 04 00 00 00 02 D4 C4 DA 9F
10 01 20 03 04 00 00 00 03 A3 C3 EA 09
10 01 20 03 04 00 00 00 04 3D A7 7F AA
10 01 20 03 04 00 00 00 05 4A A0 4F 3C
10 01 20 03 04 00 00 00 06 D3 A9 1E 86
10 01 20 03 04 00 00 01 00 23 D1 8A F2
10 01 20 03 04 00 01 00 00 3B 08 D1 84
10 01 20 03 04 01 00 00 00 82 76 DC D6";
        }

        private void MenuChange_Click(object sender, RoutedEventArgs e)
        {
            string txt = "";

            string t1 = Tb1.Text;
            string[] lines = t1.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            foreach(var line in lines)
            {
                var bytes = ByteHelper.HexToBytes(line);
                foreach (var item in bytes)
                {
                    txt += item + "\t";
                }
                txt += "\n";
            }

            Tb2.Text = txt;
        }
    }
}
