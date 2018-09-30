using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkedNet
{
    class Program
    {
        static void Main(string[] args)
        {
            Marked marked = new Marked();
            string path = AppDomain.CurrentDomain.BaseDirectory + "api.md";
            string txt = File.ReadAllText(path);
            string result=marked.Parse(txt);
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "result.html", result);
            Console.WriteLine(result);
            Console.Read();
        }
    }
}
