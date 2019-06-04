using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DbModel.Location.AreaAndDev;
using Location.IModel;
using TModel.Tools;

namespace DbModel.Location.Settings
{
    public enum RelativeMode
    {
        CAD坐标,相对园区,相对楼层,相对机房
    }

    public class ArchorSetting:IComparable<ArchorSetting>,IEntity
    {
        [NotMapped]
        public bool IsNew { get; set; }

        public int Id { get; set; }

        [Display(Name="基站编号(Id)")]
        [MaxLength(16)]
        public string Code { get; set; }

        [Display(Name = "基站Id")]
        public int ArchorId { get; set; }

        [NotMapped]
        public Archor Archor { get; set; }

        public bool Error { get; set; }

        [Display(Name = "名称")]
        [MaxLength(32)]
        public string Name { get; set; }

        [Display(Name = "相对高度")]
        public double RelativeHeight { get; set; }

        [Display(Name = "绝对高度")]
        public double AbsoluteHeight { get; set; }

        [Display(Name = "所在机房")]
        [MaxLength(128)]
        public string RoomName { get; set; }

        [Display(Name = "所在机房左下角X")]
        [MaxLength(64)]
        public string RoomMinX { get; set; }

        [Display(Name = "所在机房左下角Y")]
        [MaxLength(64)]
        public string RoomMinY { get; set; }

        [Display(Name = "所在楼层")]
        [MaxLength(64)]
        public string FloorName { get; set; }

        [Display(Name = "所在楼层左下角X")]
        [MaxLength(64)]
        public string FloorMinX { get; set; }

        [Display(Name = "所在楼层左下角Y")]
        [MaxLength(64)]
        public string FloorMinY { get; set; }

        [Display(Name = "所在建筑")]
        [MaxLength(64)]
        public string BuildingName { get; set; }

        [Display(Name = "所在建筑左下角X")]
        [MaxLength(64)]
        public string BuildingMinX { get; set; }

        [Display(Name = "所在建筑左下角Y")]
        [MaxLength(64)]
        public string BuildingMinY { get; set; }

        public void SetPath(Area room,Area floor,Area building)
        {
            if (room != null)
            {
                RoomName = room.Name;
                RoomMinX = room.InitBound.MinX.ToString("F3");
                RoomMinY = room.InitBound.MinY.ToString("F3");
            }
            if (floor != null)
            {
                FloorName = floor.Name;
                FloorMinX = floor.InitBound.MinX.ToString("F3");
                FloorMinY = floor.InitBound.MinY.ToString("F3");
            }
            if (building != null)
            {
                BuildingName = building.Name;
                BuildingMinX = building.InitBound.MinX.ToString("F3");
                BuildingMinY = building.InitBound.MinY.ToString("F3");
            }
           
        }

     
        /// <summary>
        /// 0:绝对坐标，1:相对电厂中的某一参考点，2:相对楼层中的某一参考点(包括房间)
        /// </summary>
        [Display(Name = "坐标模式")]
        public RelativeMode RelativeMode { get; set; }

        [Display(Name = "参考点X")]
        [MaxLength(64)]
        public string ZeroX { get; set; }

        [Display(Name = "参考点Y")]
        [MaxLength(64)]
        public string ZeroY { get; set; }

        public void SetZero(double x,double y)
        {
            ZeroX = x.ToString("F3");
            ZeroY = y.ToString("F3");
        }

        [Display(Name = "相对坐标X")]
        [MaxLength(64)]
        public string RelativeX { get; set; }

        [Display(Name = "相对坐标Y")]
        [MaxLength(64)]
        public string RelativeY { get; set; }

        public void SetRelative(double x, double y)
        {
            RelativeX = x.ToString("F3");
            RelativeY = y.ToString("F3");
        }


        [Display(Name = "绝对(CAD)坐标X")]
        [MaxLength(64)]
        public string AbsoluteX { get; set; }

        [Display(Name = "绝对(CAD)坐标Y")]
        [MaxLength(64)]
        public string AbsoluteY { get; set; }

        [NotMapped]
        public double ParkOffsetX { get; set; }

        [NotMapped]
        public double ParkOffsetY { get; set; }

        [NotMapped]
        public double ArchorX { get; set; }

        [NotMapped]
        public double ArchorY { get; set; }

        [NotMapped]
        public int ArchorX100 { get; set; }

        [NotMapped]
        public int ArchorY100 { get; set; }

        [NotMapped]
        public int Height100 { get; set; }

        [Display(Name = "测量坐标X(测量点)")]
        [MaxLength(64)]
        public string MeasureX { get; set; }

        [Display(Name = "测量坐标Y(测量点)")]
        [MaxLength(64)]
        public string MeasureY { get; set; }

        public void SetAbsolute(double x, double y)
        {
            AbsoluteX = x.ToString("F3");
            AbsoluteY = y.ToString("F3");

        }

        /// <summary>
        /// 设置附加信息（给品铂用）
        /// </summary>
        public void SetExtensionInfo(double offx,double offy)
        {
            ParkOffsetX = offx;
            ParkOffsetY = offy;
            double x = AbsoluteX.ToDouble();
            double y = AbsoluteY.ToDouble();
            ArchorX = (x - offx);
            ArchorY = (y - offy);
            ArchorX100 = (ArchorX * 100).ToString("F0").ToInt();
            ArchorY100 = (ArchorY * 100).ToString("F0").ToInt();
            Height100 = (AbsoluteHeight*100).ToString("F0").ToInt();
        }


        public ArchorSetting()
        {
            IsNew = true;
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

        public ArchorSetting(string code,int archorId):this()
        {
            Code = code;
            ArchorId = archorId;
        }

        public ArchorSetting(Archor archor)
        {
            var dev = archor.DevInfo;
            Id = archor.Id;
            ArchorId = archor.Id;
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

        public bool CalAbsolute()
        {
            float x1= FloorMinX.ToFloat()+BuildingMinX.ToFloat() + ZeroX.ToFloat() + RelativeX.ToFloat();
            float x2 = AbsoluteX.ToFloat();

            float y1 = FloorMinY.ToFloat() + BuildingMinY.ToFloat() + ZeroY.ToFloat() + RelativeY.ToFloat();
            float y2 = AbsoluteY.ToFloat();

            if (Math.Abs(x1 - x2)>0.1 || Math.Abs(y1-y2)>0.1)
            {
                AbsoluteX = x1.ToString();
                AbsoluteY = y1.ToString();
                return false;
            }
            else
            {
                return true;
            }
            
        }

        public string GetPath()
        {
            return BuildingName +"."+ FloorName + "." + RoomName + "." + Name;
        }

        public int CompareTo(ArchorSetting other)
        {
            return this.GetPath().CompareTo(other.GetPath());
        }
    }
}
