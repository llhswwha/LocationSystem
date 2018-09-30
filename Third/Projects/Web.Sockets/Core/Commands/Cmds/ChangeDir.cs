using System;
using System.Collections.Generic;

namespace Web.Sockets.Core.Commands.Cmds
{
    /// <summary>
    /// CD或CHDIR 改变当前文件夹。 
    /// 用法：CD [文件夹名] 
    /// 若无文件夹名则显示当前路径。 
    /// </summary>
    class ChangeDir : ICommand
    {
        public string Name { get; set; }
        public ChangeDir()
        {
            Name = "cd";
        }
        public CommandResult Execute(List<string> args)
        {
            return null;
        }
    }
}
