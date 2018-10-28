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
using TModel.Tools;

namespace WPFClientControlLib
{
    /// <summary>
    /// PointControl.xaml 的交互逻辑
    /// </summary>
    public partial class PointControl : UserControl
    {
        public PointControl()
        {
            InitializeComponent();
        }

        public double X
        {
            get
            {
                return TbX.Text.ToDouble();
            }
            set
            {
                TbX.Text = value.ToString("F2");
            }
        }

        public double Y
        {
            get
            {
                return TbY.Text.ToDouble();
            }
            set
            {
                TbY.Text = value.ToString("F2");
            }
        }

        public bool IsReadOnly
        {
            set
            {
                TbY.IsReadOnly = value;
                TbX.IsReadOnly = value;
            }
        }

        private void TbX_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ValueChanged != null)
            {
                ValueChanged(this);
            }
        }

        public event Action<PointControl> ValueChanged;

        private void TbY_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ValueChanged != null)
            {
                ValueChanged(this);
            }
        }
    }
}
