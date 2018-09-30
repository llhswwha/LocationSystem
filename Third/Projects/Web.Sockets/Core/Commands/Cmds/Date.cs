using System;
using System.Collections.Generic;

namespace Web.Sockets.Core.Commands.Cmds
{
    /// <summary>
    /// DATE 显示或设置日期。 
    ///用法：DATE [日期] 
    /// </summary>
    class Date:ICommand
    {
        public string Name { get; set; }
        public Date()
        {
            Name = "date";
        }
        public CommandResult Execute(List<string> args)
        {
            CommandResult r=new CommandResult();
            r.Successed = true;
            r.Message = DateTime.Now.ToShortDateString();
            return r;
        }
    }
}
