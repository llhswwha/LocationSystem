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
using TModel.Tools;
using DevInfo = DbModel.Location.AreaAndDev.DevInfo;
using Point = Location.TModel.Location.AreaAndDev.Point;

namespace LocationServer.Windows.Simple
{
    /// <summary>
    /// TrackPointWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TrackPointWindow : Window
    {
        public TrackPointWindow()
        {
            InitializeComponent();
        }

        private void TbHeight_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            double height = TbHeight.Text.ToDouble();
            TbHeight2.Text = (height + floorHeight).ToString("F3");
        }

        private void BtnShowPoint_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAutoSelectPoint_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSelectPoint_Click(object sender, RoutedEventArgs e)
        {
            var win = new PointSelectWindow(_floor, x,y, 0);
            win.SelectedAreaChanged += (area) =>
            {

            };
            win.SelectedPointChanged += (point) =>
            {
                SetZeroPoint(point.X, point.Y);
            };
            win.Owner = this;
            win.Show();
        }


        private void SetZeroPoint(double zx, double zy)
        {
            PcZero.X = zx;
            PcZero.Y = zy;
            PcRelative.X = x - zx;
            PcRelative.Y = y - zy;
            //OnShowPoint();
        }

        float x;
        float y;

        DevInfo _tp;

        Bll bll = new Bll();

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            _tp.Name = TbName.Text;
            _tp.PosX = (float)PcArchor.X;
            _tp.PosY = (float)PcArchor.Y;

            bool r=bll.DevInfos.Add(_tp);
            if (r)
            {
                MessageBox.Show("保存成功");
            }
            else
            {
                MessageBox.Show("保存失败");
            }
        }

        float floorHeight = 0;

        Area _room;
        Area _floor;

        internal void Show(int areaId, Point point)
        {
            var area= bll.Areas.Find(areaId);
            if (area.Type == AreaTypes.楼层)
            {
                _floor = area;
                _room = Bll.GetRoomByPos(_floor.Id, point.X, point.Y);
            }
            else if(area.Type == AreaTypes.机房)
            {
                _room = area;
                _floor = _room.Parent;
            }
            
            TbBuildingName.Text = _floor.Parent.Name;
            TbFloorName.Text = _floor.Name;
            floorHeight = _floor.InitBound.MinZ;
            TbFloorHeight.Text = floorHeight.ToString("F2");

            
            if (_room != null)
            {
                TbRoomName.Text = _room.Name;
            }

            _tp = new DevInfo();
            _tp.Local_TypeCode = 100001;
            _tp.PosX = point.X;
            _tp.PosZ = point.Y;
            _tp.Name = _room.Name+"_测点_1";
            _tp.ParentId = _floor.Id;

            TbName.Text = _tp.Name;
            PcArchor.X = _tp.PosX;
            PcArchor.Y = _tp.PosZ;

            this.Show();
        }

        internal void Show(DevInfo tp)
        {
            _tp = tp;
            x = tp.PosX;
            y = tp.PosY;
            this.Show();
        }
    }
}
