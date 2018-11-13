using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using DbModel.Tools;
using Location.IModel;
using Location.TModel.Location.AreaAndDev;
using Location.TModel.Location.Person;

namespace TModel.Location.Nodes
{
    /// <summary>
    /// 物理逻辑拓扑
    /// </summary>
    [DataContract]
    [Serializable]
    public class AreaNode : ITreeNodeEx<AreaNode, DevNode>
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int? ParentId { get; set; }

        //[DataMember]
        public virtual AreaNode Parent { get; set; }

        [DataMember]
        public AreaTypes Type { get; set; }

        //[Display(Name = "标签")]
        [DataMember]
        public string Tag { get; set; }

        [DataMember]
        public List<AreaNode> Children { get; set; }

        /// <summary>
        /// 叶子节点：区域中的设备
        /// </summary>
        [DataMember]
        public List<DevNode> LeafNodes { get; set; }

        /// <summary>
        /// 全部设备数量（包括子节点中的）
        /// </summary>
        [DataMember]
        public int TotalDevCount { get; set; }

        /// <summary>
        /// 区域内人员
        /// </summary>
        [DataMember]
        public List<PersonNode> Persons { get; set; }

        /// <summary>
        /// 全部人员数量（包括子节点中的）
        /// </summary>
        [DataMember]
        public int TotalPersonCount { get; set; }

        public bool IsSelftEmpty()
        {
            if (LeafNodes != null && LeafNodes.Count > 0)
            {
                return false;
            }
            if (Persons != null && Persons.Count > 0)
            {
                return false;
            }
            if (Children != null && Children.Count > 0)
            {
                return false;
            }
            return true;
        }

        public void AddPerson(PersonNode p)
        {
            if (Persons == null)
            {
                Persons = new List<PersonNode>();
            }
            Persons.Add(p);
        }

        [DataMember]
        public string KKS { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public List<AreaNode> GetAllChildren(int? type)
        {
            var allChildren = new List<AreaNode>();
            GetSubChildren(allChildren, this, type);
            return allChildren;
        }

        public void GetSubChildren(List<AreaNode> list, AreaNode node, int? type = null)
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

        public List<PersonNode> GetAllPerson()
        {
            var allChildren = new List<PersonNode>();
            GetSubPerson(allChildren, this);
            return allChildren;
        }

        public void GetSubPerson(List<PersonNode> list, AreaNode node)
        {
            if (node.Persons != null)
                foreach (var person in node.Persons)
                {
                    list.Add(person);
                }

            if (node.Children!=null)
                foreach (var child in node.Children)
                {
                    GetSubPerson(list, child);
                }
        }
    }
}
