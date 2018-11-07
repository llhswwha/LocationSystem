using System.Collections.Generic;
using System.Runtime.Serialization;
using Location.IModel;
using Location.IModel.Locations;
using Location.TModel.Tools;
using System;

namespace Location.TModel.Location.Person
{
    /// <summary>
    /// 部门信息
    /// </summary>
    [DataContract] [Serializable]
    public class Department 
        : ITreeNodeEx<Department, Personnel>, IDepartment
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [DataMember]
        //[Display(Name = "主键Id")]
        public int Id { get; set; }

        /// <summary>
        /// 对接Id
        /// </summary>
        [DataMember]
        //[Display(Name = "对接Id")]
        public int? Abutment_Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        //[Display(Name = "名称")]
        public string Name { get; set; }

        private Department _parent;

        //[NotMapped]
        public Department Parent
        {
            set
            {
                _parent = value;
                if (value == null)
                {
                    ParentId = null;
                }
                else
                {
                    ParentId = value.Id;
                }
            }
            get { return _parent; }
        }

        /// <summary>
        /// 父ID
        /// </summary>
        [DataMember]
        //[Display(Name = "父ID")]
        public int? ParentId { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [DataMember]
        //[Display(Name = "排序")]
        public int ShowOrder { get; set; }

        /// <summary>
        /// 类型，0本厂，1外委单位
        /// </summary>
        [DataMember]
        //[Display(Name = "类型")]
        public int Type { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        [DataMember]
        //[Display(Name = "说明")]
        public string Description { get; set; }

        /// <summary>
        /// 子部门
        /// </summary>
        [DataMember]
        public List<Department> Children { get; set; }

        /// <summary>
        /// 人员信息
        /// </summary>
        [DataMember]
        public List<Personnel> LeafNodes { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public Department Clone()
        {
            return this.CloneObjectByBinary();
        }

        public List<Department> GetAllChildren(int? type)
        {
            var allChildren = new List<Department>();
            GetSubChildren(allChildren, this, type);
            return allChildren;
        }

        public void GetSubChildren(List<Department> list, Department node, int? type = null)
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
    }
}
