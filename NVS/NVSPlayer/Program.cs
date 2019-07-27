using PlaybackDemo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace NVSPlayer
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(params string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new NVSPlayerForm());
            Application.Run(new PlayBackForm(args));
        }
    }
}
