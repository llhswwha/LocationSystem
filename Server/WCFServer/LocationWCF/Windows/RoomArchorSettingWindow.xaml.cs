using DbModel.Location.AreaAndDev;
using Location.TModel.FuncArgs;
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

            item.RelativeX = archor.X.ToString("F2");
            item.RelativeY = archor.Z.ToString("F2");
            item.Height = archor.Y;

            var floor = area;

            item.AreaName = floor.Name;

            var building = floor.Parent;
            //var building = areas.Find(j => j.Id == floor.ParentId);

            var minX = floor.InitBound.MinX + building.InitBound.MinX;
            var minY = floor.InitBound.MinY + building.InitBound.MinY;
            item.AreaMinX = minX.ToString("F2");
            item.AreaMinY = minY.ToString("F2");

            item.AbsoluteX = (archor.X + minX).ToString("F2");
            item.AbsoluteY = (archor.Z + minY).ToString("F2");

            //var rooms = areas.FindAll(j => j.ParentId == floor.Id);
            var rooms = bll.Areas.FindAll(j => j.ParentId == floor.Id);
            var room = BLL.Bll.GetArchorRoom(rooms, archor);
            //PropertyGrid3.SelectedObject = item;

            LbId.Text = archor.Id + "";
            TbName.Text = archor.Name;
            TbCode.Text = archor.Code;
            PcArchor.X = archor.X;
            PcArchor.Y = archor.Z;
            TbBuildingName.Text = building.Name;
            TbFloorName.Text = floor.Name;
            if (room != null)
            {
                TbRoomName.Text = room.Name;
                PcArchor.IsEnabled = false;
                PcZero.X = room.InitBound.MinX;
                PcZero.Y = room.InitBound.MinY;
                PcRelative.X = archor.X - room.InitBound.MinX;
                PcRelative.Y = archor.Z - room.InitBound.MinY;
            }
            else
            {
                PcArchor.IsEnabled = true;
                //PcZero.X = room.InitBound.MinX;
                //PcZero.Y = room.InitBound.MinY;
                //PcRelative.X = archor.X - room.InitBound.MinX;
                //PcRelative.Y = archor.Z - room.InitBound.MinY;
            }

            PcAbsolute.X = building.InitBound.MinX + floor.InitBound.MinX + archor.X;
            PcAbsolute.Y = building.InitBound.MinY + floor.InitBound.MinY + archor.Z;
        }

        

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {

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
    }
}
