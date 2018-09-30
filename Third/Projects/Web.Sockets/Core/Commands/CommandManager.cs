using System;
using System.Collections.Generic;
using Web.Sockets.Core.Others;

namespace Web.Sockets.Core.Commands
{
    public static class CommandManager
    {
        static CommandManager()
        {
            //AddCommand(new Dir());
            //AddCommand(new ChangeDir());
            //AddCommand(new Move());
            //AddCommand(new Date());
            //AddCommand(new Time());
        }

        public static void AddCommand(ICommand cmd)
        {
            try
            {
                string name = cmd.Name;
                if (string.IsNullOrEmpty(cmd.Name))
                {
                    name = cmd.GetType().Name.ToLower();
                }
                Commands.Add(name, cmd);
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error At CommandManager.AddCommand:" + ex.Message);
                Log.error(ex);
            }
        }

        static Dictionary<string, ICommand> Commands = new Dictionary<string,ICommand>();
        public static CommandResult Execute(byte[] data)
        {
            try
            {
                string msg = SocketHelper.GetString(data);
                string[] parts = msg.Split(' ');
                string name = parts[0].ToLower();
                if (Commands.ContainsKey(name))
                {
                    List<string> args = new List<string>();
                    for (int i = 1; i < parts.Length; i++)
                    {
                        args.Add(parts[i]);
                    }

                    ICommand command = Commands[name];
                    return command.Execute(args);
                }
                
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error At CommandManager.Execute:" + ex.Message);
                Log.error(ex);
            }
            return null;
        }
    }
}
