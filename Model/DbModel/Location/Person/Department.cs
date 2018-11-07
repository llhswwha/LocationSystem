using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Location.IModel;
using Location.IModel.Locations;
using Location.TModel.Tools;
using DbModel.Tools;

namespace DbModel.Location.Person
{
    /// <summary>
    /// 部门信息
    /// </summary>
    public class Department : ITreeNodeEx<Department, Personnel>, IDepartment
    {
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
        public string Name { get; set; }

        /// <summary>
        /// 上级部门
        /// </summary>
        [DataMember]
        [Display(Name = "上级部门")]
        public int? ParentId { get; set; }
        public virtual Department Parent { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [DataMember]
        [Display(Name = "排序")]
        public int ShowOrder { get; set; }

        /// <summary>
        /// 类型，0本厂，1外委单位
        /// </summary>
        [DataMember]
        [Display(Name = "类型")]
        public DepartType Type { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        [DataMember]
        [Display(Name = "说明")]
        public string Description { get; set; }

        /// <summary>
        /// 子部门
        /// </summary>
        [DataMember]
        [ForeignKey("ParentId")]
        public virtual List<Department> Children { get; set; }

        /// <summary>
        /// 人员信息
        /// </summary>
        [DataMember]
        [ForeignKey("ParentId")]
        public virtual List<Personnel> LeafNodes { get; set; }

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
