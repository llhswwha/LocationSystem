using System;
using DbModel.Tools;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Location.TModel.Tools;
using IModel.Tools;
using IModel;
using Location.IModel;
using Location.Model.InitInfos;
using Location.TModel.Location.AreaAndDev;
using TModel.Tools;

namespace DbModel.Location.AreaAndDev
{
    /// <summary>
    ///     边界信息 地图和区域
    /// </summary>
    [DataContract]
    [Serializable]
    public class Bound:IId
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [DataMember]
        [Display(Name = "主键Id")]
        public int Id { get; set; }

        /// <summary>
        /// 最小X值
        /// </summary>
        [DataMember]
        [Display(Name = "最小X值")]
        public float MinX { get; set; }

        /// <summary>
        /// 最大X值
        /// </summary>
        [DataMember]
        [Display(Name = "最大X值")]
        public float MaxX { get; set; }

        /// <summary>
        /// 最小Y值
        /// </summary>
        [DataMember]
        [Display(Name = "最小Y值")]
        public float MinY { get; set; }

        /// <summary>
        /// 最大Y值
        /// </summary>
        [DataMember]
        [Display(Name = "最大Y值")]
        public float MaxY { get; set; }

        /// <summary>
        /// 最小Z值
        /// </summary>
        [DataMember]
        [Display(Name = "最小Z值")]
        public float MinZ { get; set; }

        /// <summary>
        /// 最大Z值
        /// </summary>
        [DataMember]
        [Display(Name = "最大Z值")]
        public float MaxZ { get; set; }

        /// <summary>
        /// 是否长方形
        /// </summary>
        [DataMember]
        [Display(Name = "形状")]//0:长方形，1:不规则多边形 2:圆形
        public int Shape { get; set; }

        /// <summary>
        /// 是否相对坐标
        /// </summary>
        [DataMember]
        [Display(Name = "是否相对坐标")]
        public bool IsRelative { get; set; }

        /// <summary>
        /// 子坐标系的原点坐标
        /// </summary>
        [DataMember]
        [Display(Name = "最小X值")]
        public float? ZeroX { get; set; }

        /// <summary>
        /// 子坐标系的原点坐标
        /// </summary>
        [DataMember]
        [Display(Name = "最大X值")]
        public float? ZeroY { get; set; }


        /// <summary>
        /// 位置点
        /// </summary>
        [DataMember]
        [Display(Name = "位置点")]
        public List<Point> Points { get; set; }

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

        //[DataMember]
        //public int ParentId { get; set; }

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

        public Bound(float x1, float y1, float x2, float y2, BoundInfo info) : this()
        {
            SetInitBound(x1, y1, x2, y2, info.BottomHeight, info.Thickness);
            Shape = 0;
            IsRelative = info.IsRelative;
            if(info.ZeroX!=null)
                ZeroX = info.ZeroX.ToFloat();
            if (info.ZeroY != null)
                ZeroY = info.ZeroY.ToFloat();
        }

        public Bound(Point[] points, float bottomHeightT, float thicknessT, bool isRelative) : this()
        {
            SetInitBound(points, bottomHeightT, thicknessT);
            Shape = 1;
            IsRelative = isRelative;
        }

        public Bound(Point[] points,BoundInfo info) : this()
        {
            SetInitBound(points, info.BottomHeight, info.Thickness);
            Shape = 1;
            IsRelative = info.IsRelative;
            if (info.ZeroX != null)
                ZeroX = info.ZeroX.ToFloat();
            if (info.ZeroY != null)
                ZeroY = info.ZeroY.ToFloat();
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

        public void SetInitBound(Bound Bd)
        {
            MinX = Bd.MinX;
            MaxX = Bd.MaxX;
            MinY = Bd.MinY;
            MaxY = Bd.MaxY;
            MinZ = Bd.MinZ;
            MaxZ = Bd.MaxZ;
            Shape = Bd.Shape;
            IsRelative = Bd.IsRelative;

            return;
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

        public void SetMinMaxXY()
        {
            if(Points!=null)
                SetMinMaxXY(Points.ToArray());
        }

        public void SetInitBound(TransformM tranM)
        {
            double bottomHeightT = tranM.Y - (tranM.SY / 2);
            Init((float)(bottomHeightT), (float)tranM.SY);

            Point p1 = new Point((float)(tranM.X - tranM.SX / 2), (float)(tranM.Z + tranM.SZ / 2), 0);
            Point p2 = new Point((float)(tranM.X + tranM.SX / 2), (float)(tranM.Z + tranM.SZ / 2), 0);
            Point p3 = new Point((float)(tranM.X + tranM.SX / 2), (float)(tranM.Z - tranM.SZ / 2), 0);
            Point p4 = new Point((float)(tranM.X - tranM.SX / 2), (float)(tranM.Z - tranM.SZ / 2), 0);
            Point[] points = new Point[] { p1, p2, p3, p4 };

            SetInitBound(points);

            //double pX = (MinX + MaxX)/2.0;
            //double pY = (MinY + MaxY)/2.0;
            //double pZ = (MinZ + MaxZ)/2.0;

        }

        /// <summary>
        /// 用两点(对角点)初始化区域范围
        /// </summary>
        public void SetInitBound(Point[] points, float bottomHeightT, float thicknessT)
        {
            Init(bottomHeightT, thicknessT);

            SetInitBound(points);

            //double pX = (MinX + MaxX)/2.0;
            //double pY = (MinY + MaxY)/2.0;
            //double pZ = (MinZ + MaxZ)/2.0;
        }

        private void Init(float bottomHeightT, float thicknessT)
        {
            Points = new List<Point>();

            MinX = float.MaxValue;
            MinY = float.MaxValue;
            MaxX = float.MinValue;
            MaxY = float.MinValue;
            MinZ = 0 + bottomHeightT;
            MaxZ = thicknessT + bottomHeightT;
        }

        public void SetInitBound(Point[] points)
        {
            if (points.Length == 0)
            {
                MinX = 0;
                MinY = 0;
                MaxX = 0;
                MaxY = 0;
            }
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

            for (int i = 0; i < points.Length; i++)
            {
                Point point = points[i];
                //point.X -= MinX;
                //point.Y -= MinY;
                Points.Add(new Point(point));
            }
        }

        private List<Bound> childrenBounds = new List<Bound>();

        public void Combine(Bound bound)
        {
            if (bound == null) return;
            if (Points == null)
            {
                Init(0, 0);
            }
            childrenBounds.Add(bound);
            SetInitBound(bound.GetPoints2D().ToArray());
        }

        public void AddPoint(Point point)
        {
            if (Points == null)
            {
                Points=new List<Point>();
            }
            Points.Add(point);
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
                points.Add(new Point(MinX, MinY,0,0));
                points.Add(new Point(MinX, MaxY, 0, 1));
                points.Add(new Point(MaxX, MaxY, 0, 2));
                points.Add(new Point(MaxY, MinY, 0, 3));
            }
            return points;
        }

        public List<Point> GetPointsByPointList(List<Point> pointlist)
        {
            List<Point> points = pointlist.FindAll(p => p.BoundId == Id);
            if (points == null)
            {
                points = new List<Point>();
                points.Add(new Point(MinX, MinY, 0, 0));
                points.Add(new Point(MinX, MaxY, 0, 1));
                points.Add(new Point(MaxX, MaxY, 0, 2));
                points.Add(new Point(MaxY, MinY, 0, 3));
            }
            
            return points;
        }

        public Bound Clone()
        {
            Bound copy = new Bound();
            copy = this.CloneObjectByBinary();
            return copy;
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

        //public bool Get

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

                var distance = (center.X - x) * (center.X - x)+ (center.Y - y) * (center.Y - y);
                var distance2 = size * size/4;
                var r= distance2 > distance;
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


        public Point GetLeftBottomPoint()
        {
            return GetClosePoint(MinX, MinY);
        }

        /// <summary>
        /// 得到最近的点
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        public Point GetClosePoint(double x, double y)
        {
            Point closePoint = null;
            var minLenth = double.MaxValue;
            foreach (var point in GetPoints2D())
            {
                var length = (point.X - x) * (point.X - x) + (point.Y - y) * (point.Y - y);
                if (length < minLenth)
                {
                    minLenth = length;
                    closePoint = point;
                }
            }
            return closePoint;
        }
    }
}