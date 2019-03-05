using System.Collections.Generic;
using System.Runtime.Serialization;
using Location.TModel.Tools;
using System;
using IModel.Tools;
using IModel;
using System.Xml.Serialization;

namespace Location.TModel.Location.AreaAndDev
{
    /// <summary>
    ///     边界信息 地图和区域
    /// </summary>
    [DataContract] [Serializable]
    public class Bound
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [DataMember]
        //[Display(Name = "主键Id")]
        public int Id { get; set; }

        /// <summary>
        /// 最小X值
        /// </summary>
        [DataMember]
        //[Display(Name = "最小X值")]
        public float MinX { get; set; }

        /// <summary>
        /// 最大X值
        /// </summary>
        [DataMember]
        //[Display(Name = "最大X值")]
        public float MaxX { get; set; }

        /// <summary>
        /// 最小Y值
        /// </summary>
        [DataMember]
        //[Display(Name = "最小Y值")]
        public float MinY { get; set; }

        /// <summary>
        /// 最大Y值
        /// </summary>
        [DataMember]
        //[Display(Name = "最大Y值")]
        public float MaxY { get; set; }

        /// <summary>
        /// 最小Z值
        /// </summary>
        [DataMember]
        //[Display(Name = "最小Z值")]
        public float MinZ { get; set; }

        /// <summary>
        /// 最大Z值
        /// </summary>
        [DataMember]
        //[Display(Name = "最大Z值")]
        public float MaxZ { get; set; }

        /// <summary>
        /// 是否长方形
        /// </summary>
        [DataMember]
        //[Display(Name = "是否长方形")]
        public int Shape { get; set; }

        /// <summary>
        /// 是否相对坐标
        /// </summary>
        [DataMember]
        //[Display(Name = "是否相对坐标")]
        public bool IsRelative { get; set; }

        /// <summary>
        /// 位置点
        /// </summary>
        //[DataMember]
        //[Display(Name = "位置点")]
        [XmlIgnore]
        public List<Point> Points { get; set; }

        public Bound()
        {
            MaxZ = 1;
            IsRelative = true;
        }

        public Bound(float x1, float y1, float x2, float y2, float bottomHeightT, float thicknessT, bool isRelative) : this()
        {
            SetInitBound(x1, y1, x2, y2, bottomHeightT, thicknessT);
            Shape = 0;
            IsRelative = isRelative;
        }

        public Bound(Point[] points, float bottomHeightT, float thicknessT, bool isRelative) : this()
        {
            SetInitBound(points, bottomHeightT, thicknessT);
            Shape = 1;
            IsRelative = isRelative;
        }

        public void SetInitBound(TransformM tranM)
        {
            double bottomHeightT = tranM.Y - (tranM.SY / 2);
            //Init((float)(bottomHeightT), (float)tranM.SY);

            Point p1 = new Point((float)(tranM.X - tranM.SX / 2), (float)(tranM.Z - tranM.SZ / 2), 0);
            Point p2 = new Point((float)(tranM.X + tranM.SX / 2), (float)(tranM.Z - tranM.SZ / 2), 1);
            Point p3 = new Point((float)(tranM.X + tranM.SX / 2), (float)(tranM.Z + tranM.SZ / 2), 2);
            Point p4 = new Point((float)(tranM.X - tranM.SX / 2), (float)(tranM.Z + tranM.SZ / 2), 3);

            Point[] points = new Point[] { p1, p2, p3, p4 };

            SetInitBound(points, (float)(bottomHeightT), (float)tranM.SY);

            //double pX = (MinX + MaxX)/2.0;
            //double pY = (MinY + MaxY)/2.0;
            //double pZ = (MinZ + MaxZ)/2.0;

        }

        /// <summary>
        /// 用两点(对角点)初始化区域范围
        /// </summary>
        public List<Point> SetInitBound(float x1, float y1, float x2, float y2)
        {
            MinX = float.MaxValue;
            MinY = float.MaxValue;
            MaxX = float.MinValue;
            MaxY = float.MinValue;

            if (x1 < MinX)
            {
                MinX = x1;
            }
            if (x2 < MinX)
            {
                MinX = x2;
            }

            if (y1 < MinY)
            {
                MinY = y1;
            }
            if (y2 < MinY)
            {
                MinY = y2;
            }

            if (x1 > MaxX)
            {
                MaxX = x1;
            }
            if (x2 > MaxX)
            {
                MaxX = x2;
            }


            if (y1 > MaxY)
            {
                MaxY = y1;
            }
            if (y2 > MaxY)
            {
                MaxY = y2;
            }

            //double pX = (MinX + MaxX)/2.0;
            //double pY = (MinY + MaxY)/2.0;
            //double pZ = (MinZ + MaxZ)/2.0;
            Points = new List<Point>();
            Points.Add(new Point(MinX, MinY, 0));
            Points.Add(new Point(MaxX, MinY, 1));
            Points.Add(new Point(MaxX, MaxY, 2));
            Points.Add(new Point(MinX, MaxY, 3));

            foreach (var item in Points)
            {
                item.BoundId = this.Id;
            }
            return Points;
        }

        /// <summary>
        /// 用两点(对角点)初始化区域范围
        /// </summary>
        public void SetInitBound(float x1, float y1, float x2, float y2, float bottomHeightT, float thicknessT)
        {
            MinX = float.MaxValue;
            MinY = float.MaxValue;
            MaxX = float.MinValue;
            MaxY = float.MinValue;
            MinZ = 0 + bottomHeightT;
            MaxZ = thicknessT + bottomHeightT;

            if (x1 < MinX)
            {
                MinX = x1;
            }
            if (x2 < MinX)
            {
                MinX = x2;
            }

            if (y1 < MinY)
            {
                MinY = y1;
            }
            if (y2 < MinY)
            {
                MinY = y2;
            }

            if (x1 > MaxX)
            {
                MaxX = x1;
            }
            if (x2 > MaxX)
            {
                MaxX = x2;
            }


            if (y1 > MaxY)
            {
                MaxY = y1;
            }
            if (y2 > MaxY)
            {
                MaxY = y2;
            }

            //double pX = (MinX + MaxX)/2.0;
            //double pY = (MinY + MaxY)/2.0;
            //double pZ = (MinZ + MaxZ)/2.0;
            Points = new List<Point>();
            Points.Add(new Point(MinX, MinY, 0));
            Points.Add(new Point(MaxX, MinY, 1));
            Points.Add(new Point(MaxX, MaxY, 2));
            Points.Add(new Point(MinX, MaxY, 3));
            //Points.Add(new Point(MinX - MinX, MinY - MinY, 0));
            //Points.Add(new Point(MaxX - MinX, MinY - MinY, 1));
            //Points.Add(new Point(MaxX - MinX, MaxY - MinY, 2));
            //Points.Add(new Point(MinX - MinX, MaxY - MinY, 3));
        }

        /// <summary>
        /// 用两点(对角点)初始化区域范围
        /// </summary>
        public void SetInitBound(Point[] points, float bottomHeightT, float thicknessT)
        {
            if (Points == null)
            {
                Points = new List<Point>();
            }
            SetMinMax(points, bottomHeightT, thicknessT);
            for (int i = 0; i < points.Length; i++)
            {
                Point point = points[i];
                Point pointT = Points.Find((j) => j.Index == point.Index);
                if (pointT != null)
                {
                    pointT.SetPoint(point);
                }
                else
                {
                    Points.Add(new Point(point));
                }
            }
        }

        private void SetMinMax(Point[] points, float bottomHeightT, float thicknessT)
        {
            MinZ = 0 + bottomHeightT;
            MaxZ = thicknessT + bottomHeightT;
            SetMinMaxXY(points);
        }

        private void SetMinMaxXY(Point[] points)
        {
            MinX = float.MaxValue;
            MinY = float.MaxValue;
            MaxX = float.MinValue;
            MaxY = float.MinValue;

            for (int i = 0; i < points.Length; i++)
            {
                Point point = points[i];
                point.Index = i;
                if (point.X < MinX)
                {
                    MinX = point.X;
                }
                if (point.Y < MinY)
                {
                    MinY = point.Y;
                }
                if (point.X > MaxX)
                {
                    MaxX = point.X;
                }
                if (point.Y > MaxY)
                {
                    MaxY = point.Y;
                }
            }
        }

        public void AddPoint(Point point)
        {
            if (Points == null)
            {
                Points=new List<Point>();
            }
            Points.Add(point);

            SetMinMaxXY();
        }

        public void SetMinMaxXY()
        {
            SetMinMaxXY(Points.ToArray());
        }

        public Bound Clone()
        {
            Bound copy = new Bound();
            copy = this.CloneObjectByBinary();
            return copy;
        }

        public List<Point> GetPoints2D()
        {
            List<Point> points = new List<Point>();
            if (Points != null && Points.Count > 0)
            {
                points.AddRange(Points);
            }
            else
            {
                points.Add(new Point(MinX, MinY, 0, 0));
                points.Add(new Point(MinX, MaxY, 0, 1));
                points.Add(new Point(MaxX, MaxY, 0, 2));
                points.Add(new Point(MaxY, MinY, 0, 3));
            }
            return points;
        }

        public Point GetLeftBottomPoint()
        {
            Point leftBottom = null;
            var minLenth = double.MaxValue;
            foreach (var point in GetPoints2D())
            {
                var length = (point.X - MinX) * (point.X - MinX) + (point.Y - MinY) * (point.Y - MinY);
                if (length < minLenth)
                {
                    minLenth = length;
                    leftBottom = point;
                }
            }
            return leftBottom;
        }

        public double GetSizeX()
        {
            return MaxX - MinX;
        }

        public double GetSizeY()
        {
            return MaxY - MinY;
        }

        public double GetHeight()
        {
            return MaxZ - MinZ;
        }

        public double GetCenterX()
        {
            return MinX + (MaxX - MinX) / 2;
        }

        public double GetCenterY()
        {
            return MinY + (MaxY - MinY) / 2;
        }

        public Point GetCenter()
        {
            double x = 0;
            double y = 0;
            double z = 0;
            if (Points != null)
            {
                foreach (Point point in Points)
                {
                    x += point.X;
                    y += point.Y;
                    z += point.Z;
                }
                x /= Points.Count;
                y /= Points.Count;
                z /= Points.Count;
            }
            return new Point(x, y, z, 0);
        }

        public bool Contains(double x, double y)
        {
            if (MinX == 0 && MaxX == 0)
            {
                SetMinMaxXY();
            }
            if (Shape == 2)//圆形
            {
                var center = GetCenter();
                var sizeX = GetSizeX();
                var sizeY = GetSizeY();
                var size = sizeX > sizeY ? sizeX : sizeY;

                var distance = (center.X - x) * (center.X - x) + (center.Y - y) * (center.Y - y);
                var distance2 = size * size / 4;
                var r = distance2 > distance;
                return r;
            }
            else
            {
                if (Points != null && Points.Count > 4)//不规则多边形
                {
                    return MathTool.IsInRegion(new Point(x, y, 0, 0), Points.ConvertAll<IVector2>(i => i));
                }
                return x >= MinX && x <= MaxX && y >= MinY && y <= MaxY;
            }
        }

        public bool ContainsSimple(double x, double y)
        {
            return x >= MinX && x <= MaxX && y >= MinY && y <= MaxY;
        }
    }
}