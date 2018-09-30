using System;
using Location.IModel.FuncArgs;

namespace Location.Model.FuncArgs
{
    /// <summary>
    /// 告警查询参数
    /// </summary>
    public class AlarmSearchArg: IAlarmSearchArg
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        /// <summary>
        /// 告警等级
        /// </summary>
        public int Level { get; set; }

        public string Keyword { get; set; }
    }
}
