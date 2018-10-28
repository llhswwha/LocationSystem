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
    public partial class ArchorSettingWindowEx : Window
    {
        public ArchorSettingWindowEx()
        {
            InitializeComponent();
        }

        private void MenuSave_Click(object sender, RoutedEventArgs e)
        {

        }

        Archor archor;
        DevInfo dev;
        ArchorSetting item;

        public void ShowInfo(Archor archor)
        {
            this.archor = archor;
            BLL.Bll bll = new BLL.Bll();

            dev = archor.DevInfo;
            PropertyGrid1.SelectedObject = archor;
            PropertyGrid2.SelectedObject = dev;

            //var archor = archors[i];
            item = new ArchorSetting();
            item.Id = archor.Id;
            item.Code = archor.Code;
            item.Name = archor.Name;
            //var dev = archor.DevInfo;//大量循环获取sql数据的话采用按需加载的方式会慢很多
            //var dev = devs.Find(j => j.Id == archor.DevInfoId);//应该采用全部事先获取并从列表中搜索的方式，具体680多个，从35s变为1s
                                                               var area = dev.Parent; 
            //var area = areas.Find(j => j.Id == dev.ParentId);
            if (dev.ParentId == 2) //电厂
            {
                item.RelativeMode = (int)RelativeMode.CAD坐标;

                item.AbsoluteX = archor.X.ToString("F2");
                item.AbsoluteY = archor.Z.ToString("F2");
                item.Height = archor.Y;

                var floor = area;
                item.AreaName = floor.Name;
                item.AreaMinX = floor.InitBound.MinX.ToString("F2");
                item.AreaMinY = floor.InitBound.MinY.ToString("F2");

                item.ParkZeroX = item.AreaMinX;
                item.ParkZeroY = item.AreaMinY;

                item.RelativeX = (archor.X - floor.InitBound.MinX).ToString("F2");
                item.RelativeY = (archor.Z - floor.InitBound.MinY).ToString("F2");
            }
            else
            {
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
                var inRooms = rooms.FindAll(j => j.InitBound != null && j.InitBound.Contains(archor.X, archor.Z));
                if (inRooms.Count > 0)
                {
                    if (inRooms.Count == 1)
                    {
                        item.RoomName = inRooms[0].Name;
                        item.RoomMinX = inRooms[0].InitBound.MinX.ToString("F2");
                        item.RoomMinY = inRooms[0].InitBound.MinY.ToString("F2");
                    }
                    else
                    {
                        foreach (var inRoom in inRooms)
                        {
                            item.RoomName = inRoom.Name + ";";
                        }
                        item.RoomMinX = inRooms[0].InitBound.MinX.ToString("F2");
                        item.RoomMinY = inRooms[0].InitBound.MinY.ToString("F2");
                    }
                }
            }

            PropertyGrid3.SelectedObject = item;
        }
    }
}
