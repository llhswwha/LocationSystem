using System.Collections.Generic;
using System.Runtime.Serialization;
using DbModel.Tools;
using Location.IModel;
using Location.TModel.ConvertCodes;
using System;
using Location.TModel.Location.Person;

namespace Location.TModel.Location.AreaAndDev
{
    //public enum Types { 区域, 分组, 大楼, 楼层, 机房, 设备, 部件, 范围}


    /// <summary>
    /// 物理逻辑拓扑
    /// </summary>
    [DataContract] [Serializable]
    [ByName("Area")]
    public class PhysicalTopology:ITreeNodeEx<PhysicalTopology,DevInfo>
    {
        private TransformM _transfrom;
        private Bound _initBound;
        private Bound _editBound;

        [DataMember]
        public int Id { get; set; }

        //[Display(Name = "名称")]
        [DataMember]
        //[Required]
        public string Name { get; set; }

        //[Display(Name = "父节点")]
        [DataMember]
        public int? ParentId { get; set; }

        [DataMember]
        public virtual PhysicalTopology Parent { get; set; }

        ////[NotMapped]
        ////[DataMember]
        //public string GetPath()
        //{
        //    //get
        //    {
        //        if (Parent != null)
        //        {
        //            return Parent.GetPath() + "."+Name;
        //        }
        //        else
        //        {
        //            return Name;
        //        }
        //    }
        //}

        //[Display(Name = "排序")]
        [DataMember]
        public int? Number { get; set; }

        //[Display(Name = "类型")]
        [DataMember]
        //[Required]
        public AreaTypes Type { get; set; }

        //[Display(Name = "描述")]
        [DataMember]
        public string Describe { get; set; }

        //[Display(Name = "标签")]
        [DataMember]
        public string Tag { get; set; }

        [DataMember]
        public List<PhysicalTopology> Children { get; set; }

        /// <summary>
        /// 叶子节点：区域中的设备
        /// </summary>
        [DataMember]
        public List<DevInfo> LeafNodes { get; set; }

        /// <summary>
        /// 区域内人员
        /// </summary>
        [DataMember]
        public List<Personnel> Persons { get; set; }

        public void AddPerson(Personnel p)
        {
            if (Persons == null)
            {
                Persons=new List<Personnel>();
            }
            Persons.Add(p);
        }

        [DataMember]
        public string KKS { get; set; }

        [DataMember]
        public int? TransfromId { get; set; }

        /// <summary>
        /// 三维展示用的范围信息
        /// </summary>
        [DataMember]
        public virtual TransformM Transfrom
        {
            get { return _transfrom; }
            set
            {
                _transfrom = value;
                if (value != null)
                {
                    TransfromId = value.Id;
                }
            }
        }


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
                }
            }
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
                }
            }
        }

        /// <summary>
        /// 是否相对坐标
        /// </summary>
        [DataMember]
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

        ///// <summary>
        ///// 用两点(对角点)初始化区域范围
        ///// </summary>
        //public void SetInitBound(double x1, double y1, double x2, double y2, double thicknessT, bool isRelative, double heightT = 1, bool isOnNormalArea = true, bool isOnAlarmArea = false, bool isOnLocationArea = false)
        //{
        //    InitBound = new Bound(x1, y1, x2, y2, heightT, thicknessT, isRelative);
        //    EditBound = new Bound(x1, y1, x2, y2, heightT, thicknessT, isRelative);
        //    Transfrom = new TransformM(InitBound);
        //    Transfrom.IsOnNormalArea = isOnNormalArea;
        //    Transfrom.IsOnAlarmArea = isOnAlarmArea;
        //    Transfrom.IsOnLocationArea = isOnLocationArea;
        //}

        public override string ToString()
        {
            return Name;
        }

        public List<PhysicalTopology> GetAllChildren(int? type)
        {
            var allChildren = new List<PhysicalTopology>();
            GetSubChildren(allChildren, this, type);
            return allChildren;
        }

        public void GetSubChildren(List<PhysicalTopology> list, PhysicalTopology node, int? type = null)
        {
            foreach (var child in node.Children)
            {
                if (type == null || type == (int)child.Type)
                {
                    list.Add(child);
                }
                GetSubChildren(list, child, type);
            }
        }

        /// <summary>
        /// 是否电厂园区
        /// </summary>
        /// <returns></returns>
        public bool IsPark()
        {
            return Name == "四会热电厂";
        }
    }   
}
