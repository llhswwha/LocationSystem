using System;

namespace Location.IModel.FuncArgs
{
    public interface IAlarmSearchArg
    {
        DateTime Start { get; set; }
        DateTime End { get; set; }

        /// <summary>
        /// 告警等级
        /// </summary>
        int Level { get; set; }
    }
}
