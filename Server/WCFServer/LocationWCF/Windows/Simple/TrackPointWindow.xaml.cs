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
using IModel.Enums;
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


        float x;
        float y;

        public DevInfo _tp;

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
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("保存失败");
                DialogResult = false;
            }
        }

        float floorHeight = 0;

        Area _room;
        Area _floor;

        internal bool? Show(int areaId, Point point)
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
            else
            {
                return false;
            }
            
            TbBuildingName.Text = _floor.Parent.Name;
            TbFloorName.Text = _floor.Name;
            floorHeight = _floor.InitBound.MinZ;
            //TbFloorHeight.Text = floorHeight.ToString("F2");
            if (_room != null)
            {
                TbRoomName.Text = _room.Name;
            }

            var devs = bll.DevInfos.Where(i => i.ParentId == _floor.Id && i.Local_TypeCode == TypeCodes.TrackPoint);

            _tp = new DevInfo();
            _tp.Local_TypeCode = TypeCodes.TrackPoint;
            _tp.PosX = point.X;
            _tp.PosZ = point.Y;
            if (_room != null)
            {
                _tp.Name = _room.Name + "_测点_" + (devs.Count + 1);
            }
            else
            {
                _tp.Name = _floor.Name + "_测点_" + (devs.Count + 1);
            }
            _tp.ParentId = _floor.Id;

            TbName.Text = _tp.Name;
            PcArchor.X = _tp.PosX;
            PcArchor.Y = _tp.PosZ;

            return this.ShowDialog();
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
