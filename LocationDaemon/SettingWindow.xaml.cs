using Base.Tools;
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

namespace LocationDaemon
{
    /// <summary>
    /// SettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingWindow : Window
    {
        public SettingWindow()
        {
            InitializeComponent();
        }

        private void SettingWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            bool isAutoRun = RegeditRW.ReadIsAutoRun();
            CbAutoRun.IsChecked = isAutoRun;
        }

        private void BtnSave_OnClick(object sender, RoutedEventArgs e)
        {
            bool isAutoRun = (bool)CbAutoRun.IsChecked;
            bool r = RegeditRW.SetIsAutoRun(isAutoRun);
            if (r == false)
            {
                MessageBox.Show("保存失败");
            }
            else
            {
                MessageBox.Show("保存成功");
            }
        }
    }
}
