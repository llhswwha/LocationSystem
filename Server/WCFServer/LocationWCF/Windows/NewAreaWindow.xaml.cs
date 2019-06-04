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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="area"></param>
        /// <param name="type">0:默认 1:柱子</param>
        public NewAreaWindow(PhysicalTopology area,int type)
        {
            InitializeComponent();
            this.parent = area;
            this.type = type;
        }

        private int type = 0;

        private void BtnSave_OnClick(object sender, RoutedEventArgs e)
        {
            Bll bll = AppContext.GetLocationBll();

            var area = new Area();
            area.Name = TbName.Text;
            area.Type = (AreaTypes)TbType.SelectedItem;
            area.ParentId = parent.Id;

            if (CbHaveBound.IsChecked == true)
            {
                Bound bound = null;
                if (CbPostionType.SelectedIndex == 0)//中心点
                {
                    float x1 = (float)(TbPosition.X - TbSize.X / 2 + TbZero.X);
                    float y1 = (float)(TbPosition.Y - TbSize.Y / 2 + TbZero.Y);
                    float x2 = (float)(TbPosition.X + TbSize.X / 2 + TbZero.X);
                    float y2 = (float)(TbPosition.Y + TbSize.Y / 2 + TbZero.Y);
                    bound = new Bound(x1, y1, x2, y2, 0, 0.5f, false);
                }
                else if (CbPostionType.SelectedIndex == 1)//左下角点
                {
                    float x1 = (float)(TbPosition.X + 0 + TbZero.X);
                    float y1 = (float)(TbPosition.Y + 0 + TbZero.Y);
                    float x2 = (float)(TbPosition.X + TbSize.X + TbZero.X);
                    float y2 = (float)(TbPosition.Y + TbSize.Y + TbZero.Y);
                    bound = new Bound(x1, y1, x2, y2, 0, 0.5f, false);
                }
                
                
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

            if (type == 1)
            {
                CbPostionType.SelectedIndex = 1;
                TbType.SelectedItem = AreaTypes.CAD;
                TbName.Text = "Block";
                TbSize.X = 8.9747f;
                TbSize.Y = 8.9747f;
            }

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
