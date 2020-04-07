using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using DbModel.Location.AreaAndDev;
using TModel.Tools;

namespace Location.Model.InitInfos
{
    [Serializable]
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

        [XmlAttribute]
        public string ZeroX { get; set; }

        [XmlAttribute]
        public string ZeroY { get; set; }

        [XmlAttribute]
        public string OffsetX { get; set; }

        [XmlAttribute]
        public string OffsetY { get; set; }

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

        /// <summary>
        /// 扩大面积，简单的矩形扩大2倍，坐标相应调整
        /// </summary>
        /// <param name="power"></param>
        public void Scale(float power)
        {
            float centerX = 0;
            float centerY = 0;
            for (int i = 0; i < Points.Count; i++)
            {
                PointInfo pi = Points[i];
                centerX += pi.X;
                centerY += pi.Y;
            }

            centerX = centerX / (float)Points.Count;
            centerY = centerY / (float)Points.Count;

            for (int i = 0; i < Points.Count; i++)
            {
                PointInfo pi = Points[i];
                float offsetX = pi.X - centerX;
                float offsetY = pi.Y - centerY;
                float xNew = centerX + offsetX * power;
                float yNew = centerY + offsetY * power;
                PointInfo piNew=new PointInfo(xNew,yNew);
                Points[i] = piNew;
            }
        }

        /// <summary>
        /// 把不规则图形改成矩形
        /// </summary>
        /// <param name="power"></param>
        public void SetRectangle()
        {
            float pMinX = float.MaxValue;
            float pMinY = float.MaxValue;
            float pMaxX = float.MinValue;
            float pMaxY = float.MinValue;

            for (int i = 0; i < Points.Count; i++)
            {
                PointInfo p1 = Points[i];

                if (p1.X < pMinX)
                {
                    pMinX = p1.X;
                }

                if (p1.Y < pMinY)
                {
                    pMinY = p1.Y;
                }

                if (p1.Y > pMaxY)
                {
                    pMaxY = p1.Y;
                }

                if (p1.X > pMaxX)
                {
                    pMaxX = p1.X;
                }
            }

            Points.Clear();
            Points.Add(new PointInfo(pMinX, pMinY));
            Points.Add(new PointInfo(pMaxX, pMaxY));
        }
    }
}
