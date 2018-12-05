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
using BLL;
using DbModel.Location.AreaAndDev;
using DbModel.Tools;
using Location.TModel.Location.AreaAndDev;
using Bound = DbModel.Location.AreaAndDev.Bound;

namespace LocationServer.Windows
{
    /// <summary>
    /// Interaction logic for NewAreaWindow.xaml
    /// </summary>
    public partial class NewAreaWindow : Window
    {
        private PhysicalTopology parent;

        public NewAreaWindow()
        {
            InitializeComponent();
        }

        public NewAreaWindow(PhysicalTopology area)
        {
            InitializeComponent();

            TbType.ItemsSource = Enum.GetValues(typeof(AreaTypes));

            this.parent = area;
            TbPId.Text = area.Name;
            TbType.SelectedItem = (AreaTypes) ((int) area.Type + 1);
        }

        private void BtnSave_OnClick(object sender, RoutedEventArgs e)
        {
            Bll bll = AppContext.GetLocationBll();

            var area = new Area();
            area.Name = TbName.Text;
            area.Type = (AreaTypes)TbType.SelectedItem;
            area.ParentId = parent.Id;

            if (CbHaveBound.IsChecked == true)
            {
                float x1 = (float)(TbCenterPosition.X - TbSize.X / 2);
                float y1 = (float)(TbCenterPosition.Y - TbSize.Y / 2);
                float x2 = (float)(TbCenterPosition.X + TbSize.X / 2);
                float y2 = (float)(TbCenterPosition.Y + TbSize.Y / 2);
                var bound = new Bound(x1, y1, x2, y2, 0, 0.5f, false);
                if (bll.Bounds.Add(bound) == false)
                {
                    MessageBox.Show("坐标添加失败");
                }
                area.SetBound(bound);
            }
            else
            {
                
            }

            if (bll.Areas.Add(area) == false)
            {
                MessageBox.Show("区域添加失败");
            }
            this.DialogResult = true;
        }

        private void NewAreaWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
