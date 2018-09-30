using System.Collections.Generic;
using System.Xml.Serialization;
using Location.Model.Base;

namespace Location.Model.InitInfos
{
    public class BoundInfo
    {
        /// <summary>
        /// 厚度值
        /// </summary>
        [XmlAttribute]
        public double Thickness { get; set; }

        /// <summary>
        /// 底部高度值
        /// </summary>
        [XmlAttribute]
        public double BottomHeight { get; set; }

        [XmlAttribute]
        public bool IsRelative { get; set; }

        /// <summary>
        /// 创建区域范围是否通过数据，还是物体自身的大小
        /// </summary>
        [XmlAttribute]
        public bool IsCreateAreaByData { get; set; }

        /// <summary>
        /// 是否是告警区域范围
        /// </summary>
        [XmlAttribute]
        public bool IsOnAlarmArea { get; set; }

        /// <summary>
        /// 是否是定位区域范围
        /// </summary>
        [XmlAttribute]
        public bool IsOnLocationArea { get; set; }

        //[XmlElement]
        public List<PointInfo> Points { get; set; }

        public BoundInfo()
        {
            Points = new List<PointInfo>();
            Thickness = 1;
            IsRelative = true;
        }

        public BoundInfo(Bound bound):this()
        {
            if (bound != null)
            {
                Thickness = bound.MaxZ - bound.MinZ;
                IsRelative = bound.IsRelative;

                if (bound.Points != null)
                    foreach (Point point in bound.Points)
                    {
                        Points.Add(new PointInfo(point));
                    }
                //else
                //{
                //    Points.Add(new PointInfo());
                //}
            }
        }
    }
}
