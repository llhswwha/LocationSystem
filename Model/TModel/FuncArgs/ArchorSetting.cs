namespace Location.TModel.FuncArgs
{
    public enum RelativeMode
    {
        Absolute,Park,Floor,Room
    }

    public class ArchorSetting
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public double Height { get; set; }

        public string AreaName { get; set; }
        public string AreaMinX { get; set; }
        public string AreaMinY { get; set; }

        public string RoomName { get; set; }
        public string RoomMinX { get; set; }
        public string RoomMinY { get; set; }

        public string RelativeX { get; set; }

        public string RelativeY { get; set; }

        /// <summary>
        /// 0:绝对坐标，1:相对电厂中的某一参考点，2:相对楼层中的某一参考点(包括房间)
        /// </summary>
        public int RelativeMode { get; set; }

        public string RoomZeroX { get; set; }

        public string RoomZeroY { get; set; }

        public string ParkZeroX { get; set; }

        public string ParkZeroY { get; set; }

        public string AbsoluteX { get; set; }

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
