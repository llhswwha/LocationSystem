using System.Collections.Generic;
namespace Web.Sockets.Core.Commands
{
    public interface ICommand
    {
        string Name { get; set; }
        //List<string> Args { get; set; }
        CommandResult Execute(List<string> args);
    }
}
