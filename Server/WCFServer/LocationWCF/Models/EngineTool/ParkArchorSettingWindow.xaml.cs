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
using DbModel.Location.Settings;
using Location.TModel.FuncArgs;
using TModel.Tools;
using WPFClientControlLib;

namespace LocationServer.Windows
{
    /// <summary>
    /// ParkArchorSettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ParkArchorSettingWindow : Window
    {
        public ParkArchorSettingWindow()
        {
            InitializeComponent();
        }

        Archor _archor;
        DevInfo _dev;
        ArchorSetting _item;
        Rectangle _rect;
        Area park;
        double floorHeight = 0;

        private string _code;

        public bool ShowInfo(Rectangle rect, int devId)
        {
            Bll bll = new Bll();
            this._dev = bll.DevInfos.Find(devId);
            this._rect = rect;
            _archor = bll.Archors.Find(i => i.DevInfoId == _dev.Id);
            if (_archor == null)
            {
                return false;//不是基站的设备
            }
            _item = new ArchorSetting();
            _item.Id = _archor.Id;
            _code = _archor.Code;
            _item.Code = _archor.Code;
            _item.Name = _archor.Name;

            var area = _dev.Parent;

            _item.RelativeMode = RelativeMode.相对楼层;

            double x = _dev.PosX;
            double z = _dev.PosZ;

            _item.SetRelative(x, z);
            _item.RelativeHeight = _dev.PosY;

            park = area;

            var minX = park.InitBound.MinX;
            var minY = park.InitBound.MinY;

            _item.SetAbsolute(x+minX,z+minY);

            LbId.Text = _archor.Id + "";
            TbName.Text = _archor.Name;
            TbCode.Text = _archor.GetCode();
            IPCode1.Text = _archor.Ip;
            TbHeight.Text = _item.RelativeHeight.ToString("F2");

            PcArchor.X = x;
            PcArchor.Y = z;
         

            PcArchor.IsEnabled = true;
            PcZero.X = ZeroX;
            PcZero.Y = ZeroY;
            PcRelative.X = x - ZeroX;
            PcRelative.Y = z - ZeroY;
            PcAbsolute.X = PcZero.X + PcRelative.X;
            PcAbsolute.Y = PcZero.Y + PcRelative.Y;

            PcZero.ValueChanged += PcZero_OnValueChanged;
            PcRelative.ValueChanged += PcRelative_OnValueChanged;

            var setting = bll.ArchorSettings.GetByCode(_code);
            if (setting != null)
            {
                PcZero.X = setting.ZeroX.ToDouble();
                PcZero.Y = setting.ZeroY.ToDouble();

                Title += " [已配置]";
            }

            return true;
        }

        private void PcArchor_ValueChanged(WPFClientControlLib.PointControl obj)
        {
            //PcAbsolute.X = _building.InitBound.MinX + park.InitBound.MinX + obj.X;
            //PcAbsolute.Y = _building.InitBound.MinY + park.InitBound.MinY + obj.Y;
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
            Bll bll = new Bll();

            var archorNew = bll.Archors.Find(_archor.Id);

            string code = _archor.Code;

            _archor.X = PcAbsolute.X;
            _archor.Z = PcAbsolute.Y;
            _archor.Y = TbHeight.Text.ToDouble();
            _archor.Name = TbName.Text;
            _archor.Code = TbCode.Text;
            _archor.Ip = IPCode1.Text;

            archorNew.X = PcAbsolute.X;
            archorNew.Z = PcAbsolute.Y;
            archorNew.Y = TbHeight.Text.ToDouble();
            archorNew.Name = TbName.Text;
            archorNew.Code = TbCode.Text;
            archorNew.Ip = IPCode1.Text;

            var devNew = bll.DevInfos.Find(_dev.Id);

            devNew.Name = TbName.Text;
            devNew.PosX = (float)PcAbsolute.X;
            devNew.PosZ = (float)PcAbsolute.Y;
            devNew.PosY = TbHeight.Text.ToFloat();

            _dev.Name = TbName.Text;
            _dev.PosX = (float)PcAbsolute.X;
            _dev.PosZ = (float)PcAbsolute.Y;
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

            bool isAdd = false;
            ArchorSetting archorSetting = bll.ArchorSettings.GetByCode(_code);
            if (archorSetting == null)
            {
                archorSetting = new ArchorSetting();
                isAdd = true;
            }
            archorSetting.Code = _archor.Code;
            archorSetting.Name = _archor.Name;
            archorSetting.RelativeHeight = TbHeight.Text.ToDouble();

            archorSetting.RelativeMode = RelativeMode.相对园区;
            archorSetting.RelativeHeight = _archor.Y;
            archorSetting.AbsoluteHeight = _archor.Y;

            var x = _dev.PosX;
            var y = _dev.PosZ;
            //var leftBottom = park.InitBound.GetLeftBottomPoint();

            archorSetting.SetZero(PcZero.X, PcZero.Y);
            archorSetting.SetRelative(PcRelative.X, PcRelative.Y);
            archorSetting.SetAbsolute(PcAbsolute.X, PcAbsolute.Y);

            if (isAdd)
            {
                if (bll.ArchorSettings.Add(archorSetting)==false)
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

            if (RefreshDev != null)
            {
                RefreshDev(devNew);
            }

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

        private void MenuDetail_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public event Action<DevInfo> RefreshDev;

        private void MenuArchorInfo_OnClick(object sender, RoutedEventArgs e)
        {
            var busArchor = new Bll().bus_anchors.Find(i => i.anchor_id == _archor.Code);
            var wnd = new ItemInfoWindow();
            wnd.ShowInfo(busArchor);
            wnd.Show();
        }

        private void MenuArchorList_OnClick(object sender, RoutedEventArgs e)
        {
            var win = new ArchorListWindow();
            win.Show();
        }

        private void PcZero_OnValueChanged(PointControl obj)
        {
            ZeroX = obj.X;
            ZeroY = obj.Y;

            PcRelative.X = _dev.PosX - ZeroX;
            PcRelative.Y = _dev.PosZ - ZeroY;
        }

        public static double ZeroX;
        public static double ZeroY;

        private void PcRelative_OnValueChanged(PointControl obj)
        {
            PcAbsolute.X = PcRelative.X + PcZero.X;
            PcAbsolute.Y = PcRelative.Y + PcZero.Y;
            PcArchor.X = PcAbsolute.X;
            PcArchor.Y = PcAbsolute.Y;
        }

        private void BtnShowPoint_OnClick(object sender, RoutedEventArgs e)
        {
            if (ShowPointEvent != null)
            {
                ShowPointEvent(PcZero.X, PcZero.Y);
            }
        }

        public event Action<double, double> ShowPointEvent;

        private void IPCode1_TextChanged(object sender, TextChangedEventArgs e)
        {
            ArchorDevList archorList = AreaCanvasWindow.ArchorList;
            if (archorList != null && archorList.ArchorList != null)
            {
                ArchorDev dev = archorList.ArchorList.FirstOrDefault(i => (i.ArchorIp).ToLower() == (IPCode1.Text).ToLower());
                if (dev != null)
                {
                    TbCode.Text = dev.ArchorID;
                }
            }
        }

        private void TbCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            ArchorDevList archorList = AreaCanvasWindow.ArchorList;
            if (archorList != null)
            {
                ArchorDev dev = archorList.ArchorList.FirstOrDefault(i => (i.ArchorID).ToLower() == (TbCode.Text).ToLower());
                if (dev != null)
                {
                    IPCode1.Text = dev.ArchorIp;
                }
            }
        }
    }
}
