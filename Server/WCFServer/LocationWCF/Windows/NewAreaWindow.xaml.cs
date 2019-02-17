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
using LocationServer.Models.EngineTool;
using LocationServices.Converters;
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
            this.parent = area;
            
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
                float x1 = (float)(TbCenterPosition.X - TbSize.X / 2 + TbZero.X);
                float y1 = (float)(TbCenterPosition.Y - TbSize.Y / 2 + TbZero.Y);
                float x2 = (float)(TbCenterPosition.X + TbSize.X / 2 + TbZero.X);
                float y2 = (float)(TbCenterPosition.Y + TbSize.Y / 2 + TbZero.Y);
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
            NewArea = area;
            this.DialogResult = true;
        }

        public Area NewArea;

        public double MinX;
        public double MinY;

        private void NewAreaWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            TbType.ItemsSource = Enum.GetValues(typeof(AreaTypes));
            TbPId.Text = parent.Name;
            TbType.SelectedItem = (AreaTypes)((int)parent.Type + 1);

            var bound = parent.InitBound;
            if (bound != null)
            {
                bound.SetMinMaxXY();
                MinX = bound.MinX;
                MinY = bound.MinY;
            }
            TbZero.X = 0;
            TbZero.Y = 0;
            OnShowPoint();
        }

        private void BtnSelectZero_Click(object sender, RoutedEventArgs e)
        {
            var win = new PointSelectWindow(parent.ToDbModel(), null,1);
            win.SelectedAreaChanged += (area) =>
            {

            };
            win.SelectedPointChanged += (point) =>
            {
                //SetZeroPoint(point.X, point.Y);
                TbZero.X = point.X;
                TbZero.Y = point.Y;
                OnShowPoint();
            };
            win.Owner = this;
            win.Show();
        }

        public void OnShowPoint()
        {
            if (ShowPointEvent != null)
            {
                ShowPointEvent(TbZero.X+MinX, TbZero.Y+MinY);
            }
        }

        public event Action<double, double> ShowPointEvent;
    }
}
