using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DbModel.Location.AreaAndDev;
using DbModel.Location.Work;
using Location.IModel;

namespace DbModel.Tools.InitInfos
{
    public class AuthorizationArea 
        : ITreeNode<AuthorizationArea>
    {
        [XmlAttribute]
        public int Id { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlIgnore]
        public int? ParentId { get; set; }

        public List<AreaAuthorization> Items { get; set; }

        public List<AreaAuthorizationRecord> Records { get; set; }

        public List<AuthorizationArea> Children { get; set; }

        public AuthorizationArea()
        {
            Items = new List<AreaAuthorization>();
            Records = new List<AreaAuthorizationRecord>();
            Children = new List<AuthorizationArea>();
        }

        public List<AuthorizationArea> GetAllChildren(int? type)
        {
            var allChildren = new List<AuthorizationArea>();
            GetSubChildren(allChildren, this, type);
            return allChildren;
        }

        public void GetSubChildren(List<AuthorizationArea> list, AuthorizationArea node, int? type = null)
        {
            foreach (var child in node.Children)
            {
                //if (type == null || type == (int)child.Type)
                {
                    list.Add(child);
                }
                GetSubChildren(list, child, type);
            }
        }


    }

    public class XmlFile
    {
        [XmlAttribute]
        public int Id { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlIgnore]
        public int? ParentId { get; set; }

        public List<XmlFile> Children { get; set; }

        public List<AreaAuthorization> Items { get; set; }

        //public List<AreaAuthorizationRecord> Records { get; set; }

        public XmlFile()
        {
            Children = new List<XmlFile>();
        }
    }

}
