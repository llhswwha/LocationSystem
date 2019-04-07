using System;
using System.Runtime.Serialization;

namespace Location.TModel.Location.AreaAndDev
{

    [DataContract] [Serializable]

    public partial class DevPos
    {
        //[Key]
        [DataMember]
        public string DevID { get; set; }
        [DataMember]
        public float PosX { get; set; }
        [DataMember]
        public float PosY { get; set; }
        [DataMember]
        public float PosZ { get; set; }
        [DataMember]
        public float RotationX { get; set; }
        [DataMember]
        public float RotationY { get; set; }
        [DataMember]
        public float RotationZ { get; set; }
        [DataMember]
        public float ScaleX { get; set; }
        [DataMember]
        public float ScaleY { get; set; }
        [DataMember]
        public float ScaleZ { get; set; }

        internal void Refresh(DevPos pos)
        {
            this.PosX = pos.PosX;
            this.PosY = pos.PosY;
            this.PosZ = pos.PosZ;
            this.RotationX = pos.RotationX;
            this.RotationY = pos.RotationY;
            this.RotationZ = pos.RotationZ;
            this.ScaleX = pos.ScaleX;
            this.ScaleY = pos.ScaleY;
            this.ScaleZ = pos.ScaleZ;
        }
    }
}
