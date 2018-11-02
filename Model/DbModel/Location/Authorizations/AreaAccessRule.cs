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

        public AccessType AccessType { get; set; }

        public int AreaId { get; set; }
    }

    public enum AccessType
    {
        EnterLeave,//可以进出
        Enter,//可以进入,不能出去
        Leave,//可以出去,不能进去
    }
}
