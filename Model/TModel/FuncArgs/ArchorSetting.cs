using Location.IModel;
using System.ComponentModel.DataAnnotations;
namespace Location.TModel.FuncArgs
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

        [Display(Name = "离地高度")]
        public double Height { get; set; }

        [Display(Name = "所在区域")]
        public string AreaName { get; set; }

        [Display(Name = "所在区域左下角X")]
        public string AreaMinX { get; set; }

        [Display(Name = "所在区域左下角Y")]
        public string AreaMinY { get; set; }

        [Display(Name = "所在机房")]
        public string RoomName { get; set; }

        [Display(Name = "所在机房左下角X")]
        public string RoomMinX { get; set; }

        [Display(Name = "所在机房左下角Y")]
        public string RoomMinY { get; set; }

        [Display(Name = "相对坐标X")]
        public string RelativeX { get; set; }

        [Display(Name = "相对坐标Y")]
        public string RelativeY { get; set; }

        /// <summary>
        /// 0:绝对坐标，1:相对电厂中的某一参考点，2:相对楼层中的某一参考点(包括房间)
        /// </summary>
        [Display(Name = "坐标模式")]
        public RelativeMode RelativeMode { get; set; }

        [Display(Name = "机房参考点X")]
        public string RoomZeroX { get; set; }

        [Display(Name = "机房参考点Y")]
        public string RoomZeroY { get; set; }

        [Display(Name = "园区参考点X")]
        public string ParkZeroX { get; set; }

        [Display(Name = "园区参考点Y")]
        public string ParkZeroY { get; set; }

        [Display(Name = "绝对(CAD)坐标X")]
        public string AbsoluteX { get; set; }

        [Display(Name = "绝对(CAD)坐标Y")]
        public string AbsoluteY { get; set; }

        public ArchorSetting()
        {
            AreaMinX = "0";
            AreaMinY = "0";
            RelativeX = "0";
            RelativeY = "0";
            RoomZeroX = "0";
            RoomZeroY = "0";

            ParkZeroX = "0";
            ParkZeroY = "0";
            AbsoluteX = "0";
            AbsoluteY = "0";
        }
    }
}
