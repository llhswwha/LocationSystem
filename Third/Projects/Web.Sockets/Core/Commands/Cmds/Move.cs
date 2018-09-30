using System;
using System.Collections.Generic;

namespace Web.Sockets.Core.Commands.Cmds
{
    class Move:ICommand
    {
        public string Name { get; set; }
        public Move()
        {
            Name = "move";
        }
        public CommandResult Execute(List<string> args)
        {
            return null;
        }
    }
}
