using System;
using System.Collections.Generic;

namespace Web.Sockets.Core.Commands.Cmds
{
    /// <summary>
    /// TIME 显示或设置时间。 
    /// 用法：TIME [时间] 
    /// </summary>
    class Time:ICommand
    {
        public string Name { get; set; }
        public Time()
        {
            //Name = "time";
        }
        public CommandResult Execute(List<string> args)
        {
            CommandResult r = new CommandResult();
            r.Successed = true;
            r.Message = DateTime.Now.ToShortTimeString();
            return r;
        }
    }
}
