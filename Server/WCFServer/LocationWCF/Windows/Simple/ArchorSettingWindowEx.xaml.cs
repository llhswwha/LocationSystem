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
using BLL;
using DbModel.Location.Settings;

namespace LocationServer.Windows
{
    /// <summary>
    /// ArchorSettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ArchorSettingWindowEx : Window
    {
        public ArchorSettingWindowEx()
        {
            InitializeComponent();
        }

        private void MenuSave_Click(object sender, RoutedEventArgs e)
        {

        }

        Archor _archor;
        DevInfo dev;
        ArchorSetting archorSetting;

        public void ShowInfo(Archor archor)
        {
            this._archor = archor;
            dev = archor.DevInfo;
            BLL.Bll bll = new BLL.Bll();

            
            PropertyGrid1.SelectedObject = archor;
            PropertyGrid2.SelectedObject = archor.DevInfo;

            archorSetting = bll.ArchorSettings.GetByArchor(archor);
            if (archorSetting == null)
            {
                //archorSetting = new ArchorSetting(archor);
                dev = archor.DevInfo;
                //var archor = archors[i];
                archorSetting = new ArchorSetting(archor.Code, archor.Id);
                archorSetting.Name = archor.Name;
                var area = dev.Parent;
                var x = dev.PosX;
                var y = dev.PosZ;
                if (dev.ParentId == 2) //电厂
                {
                    archorSetting.RelativeMode = RelativeMode.相对园区;
                    archorSetting.RelativeHeight = archor.Y;
                    archorSetting.AbsoluteHeight = archor.Y;

                    var park = area;
                    var leftBottom = park.InitBound.GetLeftBottomPoint();

                    archorSetting.SetZero(leftBottom.X, leftBottom.Y);
                    archorSetting.SetRelative((x - leftBottom.X), (y - leftBottom.Y));
                    archorSetting.SetAbsolute(x, y);
                }
                else
                {

                    var floor = area;
                    var building = floor.Parent;

                    archorSetting.RelativeHeight = archor.Y;
                    archorSetting.AbsoluteHeight = (archor.Y + building.GetFloorHeight(floor.Id));

                    var minX = floor.InitBound.MinX + building.InitBound.MinX;
                    var minY = floor.InitBound.MinY + building.InitBound.MinY;

                    var room = Bll.GetDevRoom(floor, dev);
                    if (room != null)
                    {
                        archorSetting.RelativeMode = RelativeMode.相对机房;
                        var roomX = room.InitBound.MinX;
                        var roomY = room.InitBound.MinY;
                        archorSetting.SetPath(room, floor, building);
                        archorSetting.SetZero(roomX, roomY);
                        archorSetting.SetRelative((x - roomX), (y - roomY));
                        archorSetting.SetAbsolute((minX + x), (minY + y));
                    }
                    else
                    {
                        archorSetting.RelativeMode = RelativeMode.相对楼层;
                        archorSetting.SetPath(null, floor, building);
                        archorSetting.SetZero(0, 0);
                        archorSetting.SetRelative(x, y);
                        archorSetting.SetAbsolute((minX + x), (minY + y));
                    }
                }
            }

            PropertyGrid3.SelectedObject = archorSetting;
        }
    }
}
