using BLL;
using DbModel.Location.AreaAndDev;
using Location.TModel.FuncArgs;
using LocationServices.Locations;
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
using TModel.Tools;

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

        Archor archor;
        DevInfo dev;
        ArchorSetting item;
        Rectangle rect;
        Area building;
        Area floor;
        Area room;

        public void ShowInfo(Rectangle rect,Archor archor)
        {
            this.rect = rect;
            this.archor = archor;
            BLL.Bll bll = new BLL.Bll();

            dev = archor.DevInfo;

            item = new ArchorSetting();
            item.Id = archor.Id;
            item.Code = archor.Code;
            item.Name = archor.Name;

            var area = dev.Parent;

            item.RelativeMode = RelativeMode.相对楼层;

            double x = dev.PosX;
            double z = dev.PosZ;

            item.RelativeX = x.ToString("F2");
            item.RelativeY = z.ToString("F2");
            item.Height = dev.PosZ;

            floor = area;

            item.AreaName = floor.Name;

            building = floor.Parent;
            //var building = areas.Find(j => j.Id == floor.ParentId);

            var minX = floor.InitBound.MinX + building.InitBound.MinX;
            var minY = floor.InitBound.MinY + building.InitBound.MinY;
            item.AreaMinX = minX.ToString("F2");
            item.AreaMinY = minY.ToString("F2");

            item.AbsoluteX = (x + minX).ToString("F2");
            item.AbsoluteY = (z + minY).ToString("F2");

            //var rooms = areas.FindAll(j => j.ParentId == floor.Id);
            var rooms = bll.Areas.FindAll(j => j.ParentId == floor.Id);
            room = BLL.Bll.GetArchorRoom(rooms, archor);
            //PropertyGrid3.SelectedObject = item;

            LbId.Text = archor.Id + "";
            TbName.Text = archor.Name;
            TbCode.Text = archor.Code;
            if (string.IsNullOrEmpty(TbCode.Text))
            {
                TbCode.Text = "Code_"+archor.Id;
            }
            TbHeight.Text = item.Height.ToString();
            PcArchor.X = x;
            PcArchor.Y = z;
            TbBuildingName.Text = building.Name;
            TbFloorName.Text = floor.Name;
            if (room != null)
            {
                TbRoomName.Text = room.Name;
                PcArchor.IsEnabled = false;
                PcZero.X = room.InitBound.MinX;
                PcZero.Y = room.InitBound.MinY;
                PcRelative.X = x - room.InitBound.MinX;
                PcRelative.Y = z - room.InitBound.MinY;
            }
            else
            {
                PcArchor.IsEnabled = true;
                //PcZero.X = room.InitBound.MinX;
                //PcZero.Y = room.InitBound.MinY;
                //PcRelative.X = archor.X - room.InitBound.MinX;
                //PcRelative.Y = archor.Z - room.InitBound.MinY;
            }

            PcAbsolute.X = building.InitBound.MinX + floor.InitBound.MinX + x;
            PcAbsolute.Y = building.InitBound.MinY + floor.InitBound.MinY + z;

            PcRelative.ValueChanged += PcRelative_ValueChanged;
        }


        private void PcRelative_ValueChanged(WPFClientControlLib.PointControl obj)
        {
            PcAbsolute.X = building.InitBound.MinX + floor.InitBound.MinX + room.InitBound.MinX+obj.X;
            PcAbsolute.Y = building.InitBound.MinY + floor.InitBound.MinY + room.InitBound.MinY + obj.Y ;

            PcArchor.X = room.InitBound.MinX+obj.X;
            PcArchor.Y = room.InitBound.MinY+obj.Y;
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
            archor.X = PcAbsolute.X;
            archor.Z = PcAbsolute.Y;
            archor.Y = TbHeight.Text.ToDouble();
            archor.Name = TbName.Text;
            string code = archor.Code;
            archor.Code = TbCode.Text;
            

            dev.Name = TbName.Text;
            dev.PosX = (float)PcArchor.X;
            dev.PosZ = (float)PcArchor.Y;
            dev.PosY = TbHeight.Text.ToFloat();

            Bll bll = new Bll();
            bll.bus_anchors.Update(code,archor);
            bll.Archors.Edit(archor);
            bll.DevInfos.Edit(dev);

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
            win.ShowInfo(archor);
        }

        private void MenuDetail_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (RefreshDev != null)
            {
                RefreshDev(archor);
            }
        }

        public event Action<Archor> RefreshDev;

    }
}
