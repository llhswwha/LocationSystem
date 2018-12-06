using BLL;
using DbModel.Location.AreaAndDev;
using Location.TModel.FuncArgs;
using LocationServices.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Configuration;
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
using DbModel.Location.Settings;
using TModel.Tools;
using LocationServer.Tools;
using LocationServer.Models.EngineTool;

using DbDev = DbModel.Location.AreaAndDev.DevInfo;
using TDev = Location.TModel.Location.AreaAndDev.DevInfo;
using LocationServices.Converters;

namespace LocationServer.Windows
{
    /// <summary>
    /// ArchorSettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RoomArchorSettingWindow : Window
    {
        public RoomArchorSettingWindow()
        {
            InitializeComponent();
        }

        Archor _archor;
        DevInfo _dev;
        ArchorSetting _item;
        Rectangle _rect;
        Area _building;
        Area _floor;
        Area _room;
        double floorHeight = 0;
        private string _code;

        double x;

        double z;

        public bool ShowInfo(Rectangle rect, int devId)
        {
            PcZero.ValueChanged -= PcZero_ValueChanged;
            PcRelative.ValueChanged -= PcRelative_ValueChanged;
            PcArchor.ValueChanged -= PcArchor_ValueChanged;

            ArchorDevList archorList = ArchorHelper.ArchorList;

            TbCode.ItemsSource = archorList.ArchorList;
            TbCode.DisplayMemberPath = "ArchorID";

            IPCode1.ItemsSource = archorList.ArchorList;
            IPCode1.DisplayMemberPath = "ArchorIp";

            Bll bll = new Bll();
            this._dev = bll.DevInfos.Find(devId);
            this._rect = rect;
            _archor = bll.Archors.Find(i => i.DevInfoId == _dev.Id);
            if (_archor == null)
            {
                return false;
            }
            _code = _archor.GetCode();

            _item = new ArchorSetting(_code, _archor.Id);
            _item.Name = _archor.Name;
            var area = _dev.Parent;

            x = _dev.PosX;
            z = _dev.PosZ;

            _item.SetRelative(x, z);
            _item.RelativeHeight = _dev.PosY;

            _floor = area;

            _building = _floor.Parent;

            if (_building.Children == null)
            {
                _building.Children = bll.Areas.Where(i => i.ParentId == _building.Id);
            }
            floorHeight = _building.GetFloorHeight(_floor.Id);
            TbFloorHeight.Text = floorHeight.ToString("F2");

            //var building = areas.Find(j => j.Id == floor.ParentId);

            var minX = _floor.InitBound.MinX + _building.InitBound.MinX;
            var minY = _floor.InitBound.MinY + _building.InitBound.MinY;

            _item.AbsoluteX = (x + minX).ToString("F2");
            _item.AbsoluteY = (z + minY).ToString("F2");

            //var rooms = areas.FindAll(j => j.ParentId == floor.Id);
            _room = Bll.GetDevRoom(_floor, _dev);
            //PropertyGrid3.SelectedObject = item;

            LbId.Text = _archor.Id + "";
            TbName.Text = _archor.Name;
            TbCode.Text = _archor.GetCode();
            IPCode1.Text = _archor.Ip;
            double height = _item.RelativeHeight;
            //if (height == 2)
            //{
            //    height = 2.6;//现场实际一般高度是2.6左右
            //}
            TbHeight.Text = height.ToString("F2");
            TbHeight2.Text = (floorHeight + height).ToString("F2");
            PcArchor.X = x;
            PcArchor.Y = z;
            TbBuildingName.Text = _building.Name;
            TbFloorName.Text = _floor.Name;

            PcAbsolute.X = _building.InitBound.MinX + _floor.InitBound.MinX + x;
            PcAbsolute.Y = _building.InitBound.MinY + _floor.InitBound.MinY + z;

            if (_room != null)
            {
                TbRoomName.Text = _room.Name;
                PcArchor.IsEnabled = false;
                //PcZero.X = _room.InitBound.MinX;
                //PcZero.Y = _room.InitBound.MinY;
                //PcRelative.X = x - _room.InitBound.MinX;
                //PcRelative.Y = z - _room.InitBound.MinY;
                var setting = bll.ArchorSettings.GetByCode(_code, _archor.Id);
                if (setting != null)
                {
                    SetZeroPoint(setting.ZeroX.ToFloat(), setting.ZeroY.ToFloat());
                    Title += " [已配置]";
                }
                else
                {
                    if (ArchorSettingContext.ZeroX != 0)
                    {
                        SetZeroPoint(ArchorSettingContext.ZeroX, ArchorSettingContext.ZeroY);
                    }
                    else
                    {
                        SetZeroPoint(_room.InitBound.MinX, _room.InitBound.MinY);
                    }
                }
            }
            else
            {
                PcArchor.IsEnabled = true;
                PcRelative.X = PcArchor.X;
                PcRelative.Y = PcArchor.Y;
            }

            PcZero.ValueChanged += PcZero_ValueChanged;
            PcRelative.ValueChanged += PcRelative_ValueChanged;
            PcArchor.ValueChanged += PcArchor_ValueChanged;

            //OnShowPoint();
            return true;
        }

        private void SetZeroPoint(double zx, double zy)
        {
            PcZero.X = zx;
            PcZero.Y = zy;
            PcRelative.X = x - zx;
            PcRelative.Y = z - zy;
            OnShowPoint(); 
        }

        private void PcZero_ValueChanged(WPFClientControlLib.PointControl obj)
        {
            PcRelative.ValueChanged -= PcRelative_ValueChanged;
            ArchorSettingContext.ZeroX = obj.X;
            ArchorSettingContext.ZeroY = obj.Y;

            PcRelative.X = _dev.PosX - obj.X;
            PcRelative.Y = _dev.PosZ - obj.Y;
            PcRelative.ValueChanged += PcRelative_ValueChanged;
        }

        private void PcArchor_ValueChanged(WPFClientControlLib.PointControl obj)
        {
            PcAbsolute.X = _building.InitBound.MinX + _floor.InitBound.MinX + obj.X;
            PcAbsolute.Y = _building.InitBound.MinY + _floor.InitBound.MinY + obj.Y;

            PcRelative.ValueChanged -= PcRelative_ValueChanged;
            PcRelative.X = obj.X - PcZero.X;
            PcRelative.Y = obj.Y - PcZero.Y;
            PcRelative.ValueChanged += PcRelative_ValueChanged;
        }

        private void PcRelative_ValueChanged(WPFClientControlLib.PointControl obj)
        {
            PcArchor.ValueChanged -= PcArchor_ValueChanged;
            PcZero.ValueChanged -= PcZero_ValueChanged;
            if (_room != null)
            {
                PcAbsolute.X = _building.InitBound.MinX + _floor.InitBound.MinX + PcZero.X + obj.X;
                PcAbsolute.Y = _building.InitBound.MinY + _floor.InitBound.MinY + PcZero.Y + obj.Y;

                PcArchor.X = PcZero.X + obj.X;
                PcArchor.Y = PcZero.Y + obj.Y;
            }
            else
            {
                PcAbsolute.X = _building.InitBound.MinX + _floor.InitBound.MinX + obj.X;
                PcAbsolute.Y = _building.InitBound.MinY + _floor.InitBound.MinY + obj.Y;

                PcArchor.X = PcZero.X + obj.X;
                PcArchor.Y = PcZero.Y + obj.Y;
            }
            PcZero.ValueChanged += PcZero_ValueChanged;
            PcArchor.ValueChanged += PcArchor_ValueChanged;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            //LocationService service = new LocationService();
            //service.EditBusAnchor();
            if (string.IsNullOrEmpty(TbCode.Text))
            {
                MessageBox.Show("编号不能为空");
                return;
            }

            ArchorDevList archorList = ArchorHelper.ArchorList;

            TbCode.ItemsSource = archorList.ArchorList;
            TbCode.DisplayMemberPath = "ArchorID";

            IPCode1.ItemsSource = archorList.ArchorList;
            IPCode1.DisplayMemberPath = "ArchorIp";

            Bll bll = new Bll();

            var archorNew = bll.Archors.Find(_archor.Id);

            string code = _archor.Code;

            _archor.X = PcAbsolute.X;
            _archor.Z = PcAbsolute.Y;
            _archor.Y = TbHeight2.Text.ToDouble();
            _archor.Name = TbName.Text;
            _archor.Code = TbCode.Text;
            _archor.Ip = IPCode1.Text;

            archorNew.X = PcAbsolute.X;
            archorNew.Z = PcAbsolute.Y;
            archorNew.Y = TbHeight2.Text.ToDouble();
            archorNew.Name = TbName.Text;
            archorNew.Code = TbCode.Text;
            archorNew.Ip = IPCode1.Text;

            var devNew = bll.DevInfos.Find(_dev.Id);

            devNew.Name = TbName.Text;
            devNew.PosX = (float)PcArchor.X;
            devNew.PosZ = (float)PcArchor.Y;
            devNew.PosY = TbHeight.Text.ToFloat();

            _dev.Name = TbName.Text;
            _dev.PosX = (float)PcArchor.X;
            _dev.PosZ = (float)PcArchor.Y;
            _dev.PosY = TbHeight.Text.ToFloat();
          

            if (bll.bus_anchors.Update(code, _archor) == false)
            {
                MessageBox.Show("保存失败1");
                return;
            }
            
            if (bll.Archors.Edit(archorNew) == false)
            {
                MessageBox.Show("保存失败2");
                return;
            }
            if (bll.DevInfos.Edit(devNew) == false)
            {
                MessageBox.Show("保存失败3");
                return;
            }


            ArchorSetting archorSetting = bll.ArchorSettings.GetByCode(_code, _archor.Id);
            bool isAdd = false;
            if (archorSetting == null)
            {
                archorSetting = new ArchorSetting(_code, _archor.Id);
                isAdd = true;
            }
            archorSetting.Code = _archor.Code;
            archorSetting.Name = _archor.Name;
            archorSetting.RelativeHeight = TbHeight.Text.ToDouble();
            archorSetting.AbsoluteHeight = TbHeight2.Text.ToDouble();
            if (_room != null)
            {
                archorSetting.RelativeMode = RelativeMode.相对机房;
                archorSetting.SetPath(_room, _floor, _building);
                archorSetting.SetZero(PcZero.X, PcZero.Y);
                archorSetting.SetRelative(PcRelative.X, PcRelative.Y);
                archorSetting.SetAbsolute(PcAbsolute.X, PcAbsolute.Y);
            }
            else
            {
                archorSetting.RelativeMode = RelativeMode.相对楼层;
                archorSetting.SetPath(null, _floor, _building);
                archorSetting.SetZero(PcZero.X, PcZero.Y);
                archorSetting.SetRelative(PcArchor.X, PcArchor.Y);
                archorSetting.SetAbsolute(PcAbsolute.X, PcAbsolute.Y);
            }

            if (archorSetting.CalAbsolute() == false)//75个里面有43个录入的绝对坐标有问题
            {
                MessageBox.Show("有bug，请检查代码!");
            }

            if (isAdd)
            {
                if (bll.ArchorSettings.Add(archorSetting) == false)
                {
                    MessageBox.Show("保存失败4");
                    return;
                }
            }
            else
            {
                if (bll.ArchorSettings.Edit(archorSetting) == false)
                {
                    MessageBox.Show("保存失败5");
                    return;
                }
            }

            OnRefreshDev();

            MessageBox.Show("保存完成");
        }



        public bool EditBusAnchor(Archor archor)
        {
            Bll db = new Bll();
            bool bDeal = false;

            try
            {
                int nFlag = 0;
                var bac = db.bus_anchors.FirstOrDefault(p => p.anchor_id == archor.Code);
                if (bac == null)
                {
                    bac = new DbModel.Engine.bus_anchor();
                    nFlag = 1;
                }

                bac.anchor_id = archor.Code;
                bac.anchor_x = (int)(archor.X * 100);
                bac.anchor_y = (int)(archor.Z * 100);
                bac.anchor_z = (int)(archor.Y * 100);
                bac.anchor_type = (int)archor.Type;
                bac.anchor_bno = 0;
                bac.syn_anchor_id = null;
                bac.offset = 0;
                bac.min_x = 90000000;
                bac.max_x = 90000000;
                bac.min_y = 90000000;
                bac.max_y = 90000000;
                bac.min_z = 90000000;
                bac.max_z = 90000000;
                bac.enabled = 1;

                if (nFlag == 0)
                {
                    bDeal = db.bus_anchors.Edit(bac);
                }
                else
                {
                    bDeal = db.bus_anchors.Add(bac);
                }
            }
            catch (Exception ex)
            {
                string strError = ex.Message;
            }

            return bDeal;
        }

        private void MenuDetail_Click(object sender, RoutedEventArgs e)
        {
            var win = new ArchorSettingWindowEx();
            win.Show();
            win.ShowInfo(_archor);
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            OnRefreshDev();
        }

        private void OnRefreshDev()
        {
            if (RefreshDev != null)
            {
                var tDev = _dev.ToTModel();
                tDev.DevDetail = _archor.ToTModel();
                RefreshDev(tDev);
            }
        }

        public event Action<TDev> RefreshDev;

        private void MenuArchorInfo_OnClick(object sender, RoutedEventArgs e)
        {
            var busArchor = new Bll().bus_anchors.Find(i => i.anchor_id == _archor.Code);
            var wnd = new ItemInfoWindow();
            wnd.ShowInfo(busArchor);
            wnd.Show();
        }

        private void TbHeight_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            double height = TbHeight.Text.ToDouble();
            TbHeight2.Text = (height + floorHeight).ToString("F2");
        }

        private void MenuArchorList_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new BusArchorListWindow();
            win.Show();
        }

        private void BtnShowPoint_OnClick(object sender, RoutedEventArgs e)
        {
            OnShowPoint();
        }

        public void OnShowPoint()
        {
            if (ShowPointEvent != null)
            {
                ShowPointEvent(PcZero.X, PcZero.Y);
            }
        }

        public event Action<double, double> ShowPointEvent;


        private void IPCode1_KeyUp(object sender, KeyEventArgs e)
        {
            ArchorDevList archorList = ArchorHelper.ArchorList;
            if (archorList != null && archorList.ArchorList != null)
            {
                var ip = (IPCode1.Text).ToLower();
                var devs = archorList.ArchorList.Where(i => (i.ArchorIp).ToLower().Contains(ip)).ToList();
                if (devs != null && devs.Count() > 0)
                {
                    TbCode.ItemsSource = devs;
                    IPCode1.ItemsSource = devs;
                    IPCode1.IsDropDownOpen = true;

                    if (devs.Count() == 1)
                    {
                        IPCode1.SelectedItem = devs[0];
                    }
                }
            }
        }

        private void IPCode1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dev = IPCode1.SelectedItem as ArchorDev;
            TbCode.SelectedItem = dev;
        }

        private void TbCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dev = TbCode.SelectedItem as ArchorDev;
            IPCode1.SelectedItem = dev;
        }

        private void TbCode_KeyUp(object sender, KeyEventArgs e)
        {
            ArchorDevList archorList = ArchorHelper.ArchorList;
            if (archorList != null && archorList.ArchorList != null)
            {
                var code = (TbCode.Text).ToLower();
                var devs = archorList.ArchorList.Where(i => (i.ArchorID).ToLower().Contains(code)).ToList();
                if (devs != null && devs.Count() > 0)
                {
                    TbCode.ItemsSource = devs;
                    IPCode1.ItemsSource = devs;
                    TbCode.IsDropDownOpen = true;

                    if (devs.Count() == 1)
                    {
                        TbCode.SelectedItem = devs[0];
                    }
                }
            }
        }

        private void BtnSelectPoint_Click(object sender, RoutedEventArgs e)
        {
            var win = new PointSelectWindow(_floor,_dev);
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

        private void BtnAutoSelectPoint_Click(object sender, RoutedEventArgs e)
        {
            if (_room != null)
            {
                if (_floor.Name.Contains("主厂房"))//主厂房有柱子
                {
                    //var p=_room.InitBound.GetClosePoint(x, z);
                    var p = _floor.GetClosePointEx(x, z);
                    SetZeroPoint(p.X, p.Y);
                }
                else
                {
                    var p=_room.InitBound.GetClosePoint(x, z);
                    //var p = _floor.GetClosePointEx(x, z);
                    SetZeroPoint(p.X, p.Y);
                }
            }
        }
    }
}
