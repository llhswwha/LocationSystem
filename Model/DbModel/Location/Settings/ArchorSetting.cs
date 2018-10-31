using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DbModel.Location.AreaAndDev;

namespace DbModel.Location.Settings
{
    public enum RelativeMode
    {
        CAD坐标,相对园区,相对楼层,相对机房
    }

    public class ArchorSetting
    {
        public int Id { get; set; }

        [Display(Name="基站编号(Id)")]
        public string Code { get; set; }

        [Display(Name = "名称")]
        public string Name { get; set; }

        [Display(Name = "相对高度")]
        public double RelativeHeight { get; set; }

        [Display(Name = "绝对高度")]
        public double AbsoluteHeight { get; set; }

        [Display(Name = "所在机房")]
        public string RoomName { get; set; }

        [Display(Name = "所在机房左下角X")]
        public string RoomMinX { get; set; }

        [Display(Name = "所在机房左下角Y")]
        public string RoomMinY { get; set; }

        [Display(Name = "所在楼层")]
        public string FloorName { get; set; }

        [Display(Name = "所在楼层左下角X")]
        public string FloorMinX { get; set; }

        [Display(Name = "所在楼层左下角Y")]
        public string FloorMinY { get; set; }

        [Display(Name = "所在建筑")]
        public string BuildingName { get; set; }

        [Display(Name = "所在建筑左下角X")]
        public string BuildingMinX { get; set; }

        [Display(Name = "所在建筑左下角Y")]
        public string BuildingMinY { get; set; }

        public void SetPath(Area room,Area floor,Area building)
        {
            if (room != null)
            {
                RoomName = room.Name;
                RoomMinX = room.InitBound.MinX.ToString("F2");
                RoomMinY = room.InitBound.MinY.ToString("F2");
            }
            if (floor != null)
            {
                FloorName = floor.Name;
                FloorMinX = floor.InitBound.MinX.ToString("F2");
                FloorMinY = floor.InitBound.MinY.ToString("F2");
            }
            if (building != null)
            {
                BuildingName = building.Name;
                BuildingMinX = building.InitBound.MinX.ToString("F2");
                BuildingMinY = building.InitBound.MinY.ToString("F2");
            }
           
        }

     
        /// <summary>
        /// 0:绝对坐标，1:相对电厂中的某一参考点，2:相对楼层中的某一参考点(包括房间)
        /// </summary>
        [Display(Name = "坐标模式")]
        public RelativeMode RelativeMode { get; set; }

        [Display(Name = "参考点X")]
        public string ZeroX { get; set; }

        [Display(Name = "参考点Y")]
        public string ZeroY { get; set; }

        public void SetZero(double x,double y)
        {
            ZeroX = x.ToString("F2");
            ZeroY = y.ToString("F2");
        }

        [Display(Name = "相对坐标X")]
        public string RelativeX { get; set; }

        [Display(Name = "相对坐标Y")]
        public string RelativeY { get; set; }

        public void SetRelative(double x, double y)
        {
            RelativeX = x.ToString("F2");
            RelativeY = y.ToString("F2");
        }


        [Display(Name = "绝对(CAD)坐标X")]
        public string AbsoluteX { get; set; }

        [Display(Name = "绝对(CAD)坐标Y")]
        public string AbsoluteY { get; set; }

        public void SetAbsolute(double x, double y)
        {
            AbsoluteX = x.ToString("F2");
            AbsoluteY = y.ToString("F2");
        }


        public ArchorSetting()
        {
            ZeroX = "0";
            ZeroY = "0";
            RelativeX = "0";
            RelativeY = "0";
            AbsoluteX = "0";
            AbsoluteY = "0";

            RoomName = "";
            RoomMinX = "0";
            RoomMinY = "0";
            FloorName = "";
            FloorMinX = "0";
            FloorMinY = "0";
            BuildingName = "";
            BuildingMinX = "0";
            BuildingMinY = "0";
        }

        public ArchorSetting(Archor archor)
        {
            var dev = archor.DevInfo;
            Id = archor.Id;
            Code = archor.Code;
            Name = archor.Name;
            var area = dev.Parent;
            var x = dev.PosX;
            var y = dev.PosZ;
            if (dev.Parent.IsPark()) //电厂
            {
                RelativeMode = RelativeMode.相对园区;
                RelativeHeight = archor.Y;
                AbsoluteHeight = archor.Y;

                var park = area;
                var leftBottom = park.InitBound.GetLeftBottomPoint();

                SetZero(leftBottom.X, leftBottom.Y);
                SetRelative((x - leftBottom.X), (y - leftBottom.Y));
                SetAbsolute(x, y);
            }
            else
            {

                var floor = area;
                var building = floor.Parent;

                RelativeHeight = archor.Y;
                AbsoluteHeight = (archor.Y + building.GetFloorHeight(floor.Id));

                var minX = floor.InitBound.MinX + building.InitBound.MinX;
                var minY = floor.InitBound.MinY + building.InitBound.MinY;

                var room = GetDevRoom(floor.Children, dev);
                if (room != null)
                {
                    RelativeMode = RelativeMode.相对机房;
                    var roomX = room.InitBound.MinX;
                    var roomY = room.InitBound.MinY;
                    SetPath(room, floor, building);
                    SetZero(roomX, roomY);
                    SetRelative((x - roomX), (y - roomY));
                    SetAbsolute((minX + x), (minY + y));
                }
                else
                {
                    RelativeMode = RelativeMode.相对楼层;
                    SetPath(null, floor, building);
                    SetZero(0, 0);
                    SetRelative(x, y);
                    SetAbsolute((minX + x), (minY + y));
                }
            }
        }

        /// <summary>
        /// 获取设备所在的机房
        /// </summary>
        /// <param name="rooms"></param>
        /// <param name="dev"></param>
        /// <returns></returns>
        public static Area GetDevRoom(List<Area> rooms, DevInfo dev)
        {
            var inRooms = rooms.FindAll(j => j.InitBound != null && j.InitBound.Contains(dev.PosX, dev.PosZ));
            if (inRooms.Count > 0)
            {
                if (inRooms.Count == 1)
                {
                    return inRooms[0];
                }
                else
                {
                    //Log.Warn("设备有多个机房:" + dev.Name);
                    return inRooms[0];
                }
            }
            else
            {
                return null;
            }
        }
    }
}
