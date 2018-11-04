using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.Location.Authorizations
{
    /// <summary>
    /// 区域进入规则
    /// </summary>
    public class AreaAccessRule
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public AreaAccessType AccessType { get; set; }

        public AreaRangeType RangeType { get; set; }

        public List<int> AreaId { get; set; }

        public List<int> CardRoleId { get; set; }
    }

    public enum AreaRangeType
    {   
        All,//特殊，全部区域
        WithParent,//从根节点到自身节点
        Single,//只有自身   
        WithChildren,//自身和子节点（递归下去）
        AllRelative,//父节点、自身、子节点（递归下去）
    }

    /// <summary>
    /// 进出类型
    /// </summary>
    public enum AreaAccessType
    {
        EnterLeave,//可以进出
        Enter,//可以进入,不能出去
        Leave,//可以出去,不能进去
        None,//不能进去不能出去
    }
}
