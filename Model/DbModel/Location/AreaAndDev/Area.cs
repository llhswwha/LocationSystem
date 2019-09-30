using DbModel.Tools;
using Location.IModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using DbModel.Tools.InitInfos;
using Location.TModel.Tools;
using System;

namespace DbModel.Location.AreaAndDev
{
    /// <summary>
    /// 区域信息
    /// </summary>
    [Serializable]
    public class Area : ITreeNodeEx<Area, DevInfo>
    {
        private Bound _initBound;
        private Bound _editBound;

        /// <summary>
        /// 主键Id
        /// </summary>
        [DataMember]
        [Display(Name = "主键Id")]
        public int Id { get; set; }

        /// <summary>
        /// 对接Id
        /// </summary>
        [DataMember]
        [Display(Name = "对接Id")]
        public int? Abutment_Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        [Display(Name = "名称")]
        [MaxLength(32)]
        [Required]
        public string Name { get; set; }

        private string _kks = null;

        /// <summary>
        /// KKS编码
        /// </summary>
        [DataMember]
        [Display(Name = "KKS编码")]
        [MaxLength(128)]
        public string KKS
        {
            get
            {
                return _kks;
            }
            set
            {
                _kks = value;
                if(_kks== "J0CYE11EG901")
                {

                }
            }
        }

        public Area FindChild(int? id)
        {
            if (id == null) return this;
            var children=GetAllChildren(null);
            return children.Find(i => i.Id == id);
        }

        /// <summary>
        /// 父ID
        /// </summary>
        [DataMember]
        [Display(Name = "父ID")]
        public int? ParentId { get; set; }

        /// <summary>
        /// 父ID
        /// </summary>
        [DataMember]
        public virtual Area Parent { get; set; }

        /// <summary>
        /// 对接父ID
        /// </summary>
        [DataMember]
        [Display(Name = "对接父ID")]
        public int? Abutment_ParentId { get; set; }

        #region Transform信息
        /// <summary>
        /// X坐标
        /// </summary>
        [DataMember]
        [Display(Name = "X坐标")]
        public float? X { get; set; }

        /// <summary>
        /// y坐标
        /// </summary>
        [DataMember]
        [Display(Name = "y坐标")]
        public float? Y { get; set; }

        /// <summary>
        /// z坐标
        /// </summary>
        [DataMember]
        [Display(Name = "z坐标")]
        public float? Z { get; set; }

        /// <summary>
        /// x方向旋转角度
        /// </summary>
        [DataMember]
        [Display(Name = "x方向旋转角度")]
        public float? RX { get; set; }

        /// <summary>
        /// y方向旋转角度
        /// </summary>
        [DataMember]
        [Display(Name = "y方向旋转角度")]
        public float? RY { get; set; }

        /// <summary>
        /// z方向旋转角度
        /// </summary>
        [DataMember]
        [Display(Name = "z方向旋转角度")]
        public float? RZ { get; set; }

        /// <summary>
        /// x方向半径
        /// </summary>
        [DataMember]
        [Display(Name = "x方向半径")]
        public float? SX { get; set; }

        /// <summary>
        /// y方向半径
        /// </summary>
        [DataMember]
        [Display(Name = "y方向半径")]
        public float? SY { get; set; }

        /// <summary>
        /// z方向半径
        /// </summary>
        [DataMember]
        [Display(Name = "z方向半径")]
        public float? SZ { get; set; }

        /// <summary>
        /// 是否相对坐标
        /// </summary>
        [DataMember]
        [Display(Name = "是否相对坐标")]
        public bool IsRelative { get; set; }

        /// <summary>
        /// 创建区域范围是否通过数据，还是物体自身的大小
        /// </summary>
        [DataMember]
        public bool IsCreateAreaByData { get; set; }

        /// <summary>
        /// 是否是告警区域范围
        /// </summary>
        [DataMember]
        public bool IsOnAlarmArea { get; set; }

        /// <summary>
        /// 是否是定位区域
        /// </summary>
        [DataMember]
        public bool IsOnLocationArea { get; set; }

        public void SetTransform(TransformM transform)
        {
            if (transform == null)
            {
                return;
            }
            this.X = (float)transform.X;
            this.Y = (float)transform.Y;
            this.Z = (float)transform.Z;
            this.SX = (float)transform.SX;
            this.SY = (float)transform.SY;
            this.SZ = (float)transform.SZ;
            this.RX = (float)transform.RX;
            this.RY = (float)transform.RY;
            this.RZ = (float)transform.RZ;

            this.IsRelative = transform.IsRelative;
            this.IsCreateAreaByData = transform.IsCreateAreaByData;
            this.IsOnAlarmArea = transform.IsOnAlarmArea;
            this.IsOnLocationArea = transform.IsOnLocationArea;
        }

        /// <summary>
        /// 获取TransfromM信息
        /// </summary>
        /// <returns></returns>
        public TransformM GetTransformM()
        {
            if (!HaveTransform()) return null;
            TransformM transform = new TransformM();
            transform.X = this.X ?? 0;
            transform.Y = this.Y ?? 0;
            transform.Z = this.Z ?? 0;
            transform.SX = this.SX ?? 0;
            transform.SY = this.SY ?? 0;
            transform.SZ = this.SZ ?? 0;
            transform.RX = this.RX ?? 0;
            transform.RY = this.RY ?? 0;
            transform.RZ = this.RZ ?? 0;

            transform.IsRelative = this.IsRelative;
            transform.IsCreateAreaByData = this.IsCreateAreaByData;
            transform.IsOnAlarmArea = this.IsOnAlarmArea;
            transform.IsOnLocationArea = this.IsOnLocationArea;
            return transform;
        }

        public bool HaveTransform()
        {
            return X > 0 || Y > 0 || Z > 0 || SX > 0 || SY > 0 || SZ > 0;
        }

        #endregion

        /// <summary>
        /// 排序
        /// </summary>
        [Display(Name = "排序")]
        [DataMember]
        public int? Number { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [Display(Name = "类型")]
        [DataMember]
        [Required]
        public AreaTypes Type { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        [DataMember]
        [Display(Name = "说明")]
        [MaxLength(128)]
        public string Describe { get; set; }

        [DataMember]
        [ForeignKey("ParentId")]
        [NotMapped]//不能去掉，Children关系由代码来主动关联起来
        public virtual List<Area> Children { get; set; }

        class AreaDistance : IComparable<AreaDistance>
        {
            public double Distance { get; set; }

            public Area Area { get; set; }

            public int CompareTo(AreaDistance other)
            {
                return this.Distance.CompareTo(other.Distance);
            }

            public override string ToString()
            {
                return string.Format("{0} {1}", Distance, Area);
            }
        }

        public double GetDistance(double x,double y)
        {
            double x0 = X ?? 0;
            double y0 = Z ?? 0;
            return Math.Pow(((x0-x)*(x0 - x)+(y0 - y)*(y0 - y)),0.5);
        }

        public List<Area> SortByPoint(double x,double y)
        {
            var list0 = new List<AreaDistance>();
            foreach (var item in Children)
            {
                var ad = new AreaDistance();
                ad.Distance = item.GetDistance(x, y);
                ad.Area = item;
                list0.Add(ad);

                if (item.Type == AreaTypes.分组)
                {
                    if(item.Children!=null)
                        foreach (var building in item.Children)
                        {
                            var ad2 = new AreaDistance();
                            ad2.Distance = building.GetDistance(x, y);
                            ad2.Area = building;
                            list0.Add(ad2);
                        }
                }
            }
            list0.Sort();

            var list = new List<Area>();
            foreach (var item in list0)
            {
                list.Add(item.Area);
            }
            return list;
        }

        public Area GetCloseArea(double x,double y)
        {
            var list = SortByPoint(x, y);
            if (list.Count > 0)
            {
                return list[0];
            }
            return null;
        }

        public Point GetClosePointEx(double x,double y)
        {
            var area = GetCloseArea(x, y);
            if (area != null)
            {
                return area.InitBound.GetClosePoint(x, y);
            }
            else
            {
                return InitBound.GetClosePoint(x, y);
            }
        }

        /// <summary>
        /// 叶子节点：区域中的设备
        /// </summary>
        [DataMember]
        [ForeignKey("ParentId")]
        [NotMapped]
        public virtual List<DevInfo> LeafNodes { get; set; }


        [DataMember]
        public int? InitBoundId { get; set; }

        /// <summary>
        /// 初始设定的范围信息
        /// </summary>
        [DataMember]
        public virtual Bound InitBound
        {
            get { return _initBound; }
            set
            {
                _initBound = value;
                if (value != null)
                {
                    InitBoundId = value.Id;
                    //value.ParentId = this.Id;
                }
            }
        }

        public Area GetFloorByHeight(float y)
        {
            Area floor = null;
            if (Children != null)
                for (int i = 0; i < Children.Count; i++)
                {
                    Area item = Children[i];
                    double height = GetCeilingHeight(i);
                    if (y < height)
                    {
                        floor = item;
                        break;
                    }
                    //double maxZ = item.InitBound.MaxZ;
                    //if (y < maxZ)
                    //{
                    //    floor =item;
                    //    break;
                    //}
                }
            return floor;
        }

        [DataMember]
        public int? EditBoundId { get; set; }

        /// <summary>
        /// 三维中修改后的范围信息
        /// </summary>
        [DataMember]
        public virtual Bound EditBound
        {
            get { return _editBound; }
            set
            {
                _editBound = value;
                if (value != null)
                {
                    EditBoundId = value.Id;
                    //value.ParentId = this.Id;
                }
            }
        }



        public Area() { }

        public Area(Bound bound)
        {
            SetBound(bound);
        }

        public void SetBound(Bound bound)
        {
            IsRelative = bound.IsRelative;
            X = (float)((bound.MinX + bound.MaxX) / 2.0);
            Z = (float)((bound.MinY + bound.MaxY) / 2.0);//Z和Y调换一下
            Y = (float)((bound.MinZ + bound.MaxZ) / 2.0);

            SX = (bound.MaxX - bound.MinX);
            SZ = (bound.MaxY - bound.MinY);//Z和Y调换一下
            SY = (bound.MaxZ - bound.MinZ);

            RX = 0;
            RY = 0;
            RZ = 0;

            InitBound = bound;
        }

        public Area Clone()
        {
            Area copy = new Area();
            copy = this.CloneObjectByBinary();
            if (this.InitBound != null)
            {
                copy.InitBound = this.InitBound;
            }

            if (this.EditBound != null)
            {
                copy.EditBound = this.EditBound;
            }

            return copy;
        }

        /// <summary>
        /// 用两点(对角点)初始化区域范围
        /// </summary>
        public void SetInitBound(float x1, float y1, float x2, float y2, float thicknessT, bool isRelative, float heightT = 1, bool isOnNormalArea = true, bool isOnAlarmArea = false, bool isOnLocationArea = false)
        {
            InitBound = new Bound(x1, y1, x2, y2, heightT, thicknessT, isRelative);
            EditBound = new Bound(x1, y1, x2, y2, heightT, thicknessT, isRelative);
            TransformM transfrom = new TransformM(InitBound);
            transfrom.IsCreateAreaByData = isOnNormalArea;
            transfrom.IsOnAlarmArea = isOnAlarmArea;
            transfrom.IsOnLocationArea = isOnLocationArea;
            this.SetTransform(transfrom);
        }

        public List<Area> GetAllChildren(int? type)
        {
            var allChildren = new List<Area>();
            GetSubChildren(allChildren, this, type);
            return allChildren;
        }

        public void GetSubChildren(List<Area> list, Area node, int? type = null)
        {
            if (node.Children!=null)
                foreach (var child in node.Children)
                {
                    if (type == null || type == (int)child.Type)
                    {
                        list.Add(child);
                    }
                    GetSubChildren(list, child, type);
                }
        }

        private string _path = "";

        public string Path
        {
            get
            {
                return _path;
            }
        }

        public string GetPath(AreaTypes stopType,string interval)
        {
            if (!string.IsNullOrEmpty(_path)) return _path;

            if (this.Type == stopType)
            {
                return Name;
            }

                if (this.Id == 2)
            {
                _path = Name;
            }

            if (Parent != null)
            {
                if (Parent.Type == stopType)
                {
                    _path = Parent.Name + interval + Name;
                }
                else
                {
                    _path = Parent.GetPath(stopType, interval) + interval + Name;
                }
            }
            else
            {
                _path = Name;
            }
            return _path;
        }

        public string GetToBuilding(string interval)
        {
            if (!string.IsNullOrEmpty(_path)) return _path;
            if (this.Id == 2)
            {
                _path = Name;
            }

            if (Parent != null)
            {
                if (Parent.Id == 1)
                {
                    _path = Name;//根节点不用考虑进去
                }
                else if (Parent.Type == AreaTypes.大楼)
                {
                    _path = Parent.Name + interval + Name;
                }
                else if (this.Type == AreaTypes.机房)
                {
                    _path = Parent.GetToBuilding(interval);
                }
                else if (Parent.Type == AreaTypes.分组)//...建筑物
                {
                    //_path = Parent.GetToBuilding(interval) + interval + Name;
                    _path = Parent.Parent.GetToBuilding(interval) + interval + Name;//不显示...建筑物，不确定是否合理
                }
                else 
                {
                    _path = Parent.GetToBuilding(interval) + interval + Name;
                }
            }
            else
            {
                _path = Name;
            }
            return _path;
        }

        public Bound CreateBoundByChildren()
        {
            InitBound = new Bound();
            if (Children != null)
                foreach (var level1Item in Children) //建筑群
                {
                    InitBound.Combine(level1Item.InitBound);
                    if (level1Item.Children != null)
                        foreach (var level2Item in level1Item.Children) //建筑
                        {
                            InitBound.Combine(level2Item.InitBound);
                        }
                }
            return InitBound;
        }

        public Bound SetBoundByDevs()
        {
            List<Point> ps = new List<Point>();
            foreach (var item in LeafNodes)
            {
                ps.Add(new Point(item.PosX,item.PosZ,0));
            }
            InitBound.SetInitBound(ps.ToArray());
            return InitBound;
        }

        /// <summary>
        /// 获取某一个楼层的高度
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public double GetFloorHeight(int id)
        {
            double floorHeight = 0;
            int floorIndex = Children.FindIndex(i => i.Id == id);
            for (int i = 0; i < floorIndex; i++)
            {
                if (Children[i].InitBound == null) continue;
                floorHeight += Children[i].InitBound.GetHeight();
            }
            return floorHeight;
        }

        //public double GetCeilingHeight(int id)
        //{
        //    double height = 0;
        //    int index = Children.FindIndex(i => i.Id == id);
        //    for (int i = 0; i <= index; i++)
        //    {
        //        height += Children[i].InitBound.GetHeight();
        //    }
        //    return height;
        //}

        public double GetCeilingHeight(int index)
        {
            double height = 0;
            for (int i = 0; i <= index; i++)
            {
                height += Children[i].InitBound.GetHeight();
            }
            return height;
        }

        /// <summary>
        /// 是否电厂园区
        /// </summary>
        /// <returns></returns>
        public bool IsPark()
        {
            //return Name == AppSetting.ParkName;
            return this.Type == AreaTypes.园区;
        }


        public string GetSvgXml(Bound bound, List<Point> pointlist, int Scale, double OffsetX, double OffsetY, double CanvaHeight)
        {
            int nCount = 0;
            string strReturn = "";
            string strPoints = "";

            if (bound == null) return strReturn;

            foreach (var item in bound.GetPointsByPointList(pointlist))
            {
                double x = (item.X - OffsetX) * Scale;
                double y = CanvaHeight - ((item.Y - OffsetY) * Scale);
                string sx = Convert.ToString(x);
                string sy = Convert.ToString(y);

                if (nCount == 0)
                {
                    strPoints = sx + "," + sy;
                    nCount = 1;
                }
                else
                {
                    strPoints += " " + sx + "," + sy;
                }
            }

            if (strPoints != "")
            {
                strReturn = "<polygon id = \"" + Convert.ToString(Id) + "\" name = \"" + Name + "\" class=\"cls-1\" points=\"" + strPoints + "\" />";
            }

            return strReturn;
        }


        public override string ToString()
        {
            return Name;
        }
    }
}
