using System;
using System.Collections.Generic;
using System.IO;

namespace Web.Sockets.Core.Commands.Cmds
{
    /// <summary>
    /// DIR 显示文件和文件夹（目录）。
    /// 用法：DIR [文件名] [选项] 
    /// 它有很多选项，如/A表示显示所有文件（即包括带隐含和系统属性的文件）,/S表示也显示子文件夹中的文件，/P表示分屏显示，/B表示只显示文件名，等等。 
    /// 如 DIR A*.EXE /A /P 
    /// 此命令分屏显示当前文件夹下所有以A开头后缀为EXE的文件(夹）。
    /// </summary>
    class Dir:ICommand
    {
        public string Name { get; set; }

        public Dir()
        {
            Name = "dir";
        }

        public CommandResult Execute(List<string> args)
        {
            CommandResult result = new CommandResult();
            DirectoryInfo di = null;
            if (args.Count != 0)
            {
                di = new DirectoryInfo(args[0]);
            }
            else
            {
                di = new DirectoryInfo(".\\");
            }
            result.Message = GetDirInfo(di);
            result.Successed = true;
            return result;
        }

        public static string GetDirInfo(DirectoryInfo di)
        {
            string info = "Directories:\n";
            DirectoryInfo[] subDirs = di.GetDirectories();
            foreach (DirectoryInfo subDir in subDirs)
            {
                info += subDir.FullName + "\n";
            }
            info += "Files:\n";
            FileInfo[] files = di.GetFiles();
            foreach (FileInfo file in files)
            {
                info += file.Name + "\n";
            }
            return info;
        }
    }
}
