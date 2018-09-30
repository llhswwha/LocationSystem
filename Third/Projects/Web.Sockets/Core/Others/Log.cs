using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Web.Sockets.Core.Others
{
    public static class Log
    {
        public static void error(Exception ex)
        {
            try
            {
                Console.WriteLine("[{0}]: Error At {1}\n{2}", DateTime.Now, GetCurrentFuncStack(), ex);
            }
            catch
            {

            }
        }

        /// <summary>
        /// 获取当前函数的调用信息,调用队列
        /// </summary>
        /// <param name="start"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static string GetCurrentFuncStack(int start = 2, int max = 4)
        {
            string func = "";
            for (int i = start; max > 0; max--, i++)
            {
                var stack = new StackFrame(i);
                MethodBase method = stack.GetMethod();
                if (method != null)
                {
                    string name = method.Name;
                    if (i == start)
                        func = name;
                    else
                        func = name + "/" + func;
                }
            }
            return "(" + func + ")";
        }
    }
}
