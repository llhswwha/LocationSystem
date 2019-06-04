using System.Collections.Generic;
using System.Xml.Serialization;
using DbModel.Location.AreaAndDev;
using TModel.Tools;

namespace Location.Model.InitInfos
{
    public class BoundInfo
    {
        /// <summary>
        /// 厚度值
        /// </summary>
        [XmlAttribute]
        public float Thickness { get; set; }

        /// <summary>
        /// 底部高度值
        /// </summary>
        [XmlAttribute]
        public float BottomHeight { get; set; }

        [XmlAttribute]
        public bool IsRelative { get; set; }

        /// <summary>
        /// 是否是普通区域范围
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
            SetBound(bound);
        }

        private void SetBound(Bound bound)
        {
            if (bound != null)
            {
                Thickness = (float)((decimal)bound.MaxZ - (decimal)bound.MinZ);
                IsRelative = bound.IsRelative;
                BottomHeight = bound.MinZ;
                
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

        public BoundInfo(Area topo) : this()
        {
            if(topo.Name== "集控楼13.1m层")
            {

            }
            if (topo != null)
            {
                IsRelative = topo.IsRelative;
                IsCreateAreaByData = topo.IsCreateAreaByData;
                IsOnAlarmArea = topo.IsOnAlarmArea;
                IsOnLocationArea = topo.IsOnLocationArea;

                SetBound(topo.InitBound);
            }
        }
    }
}
