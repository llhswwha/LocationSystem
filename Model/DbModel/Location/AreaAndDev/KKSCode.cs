using System.ComponentModel.DataAnnotations;
using Location.IModel;
using System.Runtime.Serialization;
using DbModel.Tools;
using Location.TModel.Tools;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbModel.Location.AreaAndDev
{
    /// <summary>
    /// KKS编码信息
    /// </summary>
    public class KKSCode : IKKSCode
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [DataMember]
        [Display(Name = "主键Id")]
        public int Id { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [DataMember]
        [Display(Name = "序号")]
        [MaxLength(8)]
        [Required]
        public string Serial { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        [DataMember]
        [Display(Name = "设备名称")]
        [MaxLength(128)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 工艺相关标识
        /// </summary>
        [DataMember]
        [Display(Name = "工艺相关标识")]
        [MaxLength(128)]
        [Required]
        public string RawCode { get; set; }

        /// <summary>
        /// 工艺相关标识
        /// </summary>
        [DataMember]
        [Display(Name = "工艺相关标识")]
        [MaxLength(128)]
        [Required]
        public string Code { get; set; }

        /// <summary>
        /// 上级工艺相关标识
        /// </summary>
        [DataMember]
        [Display(Name = "上级工艺相关标识")]
        [MaxLength(128)]
        public string ParentCode { get; set; }

        /// <summary>
        /// 设计院编码
        /// </summary>
        [DataMember]
        [Display(Name = "设计院编码")]
        [MaxLength(32)]
        public string DesinCode { get; set; }

        /// <summary>
        /// 主类
        /// </summary>
        [DataMember]
        [Display(Name = "主类")]
        [MaxLength(16)]
        [Required]
        public string MainType { get; set; }

        /// <summary>
        /// 子类
        /// </summary>
        [DataMember]
        [Display(Name = "子类")]
        [MaxLength(32)]
        [Required]
        public string SubType { get; set; }

        /// <summary>
        /// 所属系统
        /// </summary>
        [DataMember]
        [Display(Name = "所属系统")]
        [MaxLength(32)]
        [Required]
        public string System { get; set; }

        [NotMapped]
        public KKSCode Parent { get; set; }

        public List<KKSCode> GetAncestors()
        {
            List<KKSCode> ancestors = new List<KKSCode>();
            KKSCode p = Parent;
            while (p!=null)
            {
                ancestors.Add(p);
                p = p.Parent;
            }
            return ancestors;
        }

        public KKSCode GetAncestor(string type)
        {
            KKSCode p = Parent;
            while (p != null && p.MainType != type)
            {
                p = p.Parent;
            }
            return p;
        }

        public void SetSystem(KKSCode sys, KKSCode subSys)
        {
            if (System == "")
            {
                if (subSys != null && subSys!=this)
                {
                    System = subSys.Name;
                }
                else if (sys != null)
                {
                    System = sys.Name;
                }
            }
        }

        public void SetParent(KKSCode parent)
        {
            if (parent == null) return;
            Parent = parent;
            ParentCode = parent.Code;
            Parent.AddChild(this);
        }

        [NotMapped]
        public List<KKSCode> Children { get; set; }

        public void AddChild(KKSCode code)
        {
            if (Children == null)
            {
                Children = new List<KKSCode>();
            }
            Children.Add(code);
            code.Parent = this;
        }

        public KKSCode Clone()
        {
            return this.CloneObjectByBinary();
        }

        public override string ToString()
        {
            return string.Format("[{2}]{0},{1},{3}",Name,Code,MainType,ParentCode);
        }

        public KKSCode()
        {
            Serial = "";
            Name = "";
            RawCode = "";
            Code = "";
            ParentCode = "";
            DesinCode = "";
            MainType = "";
            SubType = "";
            System = "";
        }

        public static Dictionary<string, KKSCode> ToDict(List<KKSCode> list)
        {
            List<KKSCode> error = new List<KKSCode>();
            Dictionary<string, KKSCode> dic = new Dictionary<string, KKSCode>();
            foreach (KKSCode code in list)
            {
                if (dic.ContainsKey(code.Code))
                {
                    error.Add(code);
                }
                else
                {
                    dic.Add(code.Code, code);
                }
            }
            return dic;
        }
    }
}
