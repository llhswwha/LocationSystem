using System;

namespace Location.IModel.FuncArgs
{
    public interface IAlarmSearchArg
    {
        string Start { get; set; }
        string End { get; set; }

        /// <summary>
        /// 告警等级
        /// </summary>
        int Level { get; set; }
    }
}
