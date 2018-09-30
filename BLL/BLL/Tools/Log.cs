using System;
using System.Collections.Generic;

namespace Location.BLL.Tool
{
    public  static class Log
    {


       public   static log4net.ILog Logger=log4net.LogManager.GetLogger("Logger");

        #region LogInfoStart->End
        public class LogInfo
        {
            public string Tag;
            public DateTime Time;
            public bool IsGroup;
        }
        static Dictionary<string, LogInfo> infos=new Dictionary<string, LogInfo>();

        public static int TabCount = 0;

        public static string GetTabString()
        {
            string txt = "" + TabCount + " ";
            for (int i = 0; i < TabCount; i++)
            {
                txt += ">>";
            }
            return txt;
        }

        public static string GetBefore()
        {
            return GetTabString();
        }

        public static string GetAfter()
        {
            return "";
        }

        public static void InfoStart(string tag,bool isGroup=false)
        {
            LogInfo info=new LogInfo() {Tag = tag,Time = DateTime.Now ,IsGroup = isGroup};
            if (infos.ContainsKey(tag))
            {
                infos[tag] = info;
            }
            else
            {
                infos.Add(tag, info);
            }
            
            if (info.IsGroup)
            {
                TabCount++;
            }
            Logger.Info(GetBefore()+tag +" Start "+ GetAfter());
        }

        public static void InfoEnd(string tag)
        {
            if (infos.ContainsKey(tag))
            {
                LogInfo info = infos[tag];
                TimeSpan timeSpan = DateTime.Now - info.Time;

                Logger.Info(GetBefore() + tag + " End Time:" + timeSpan + GetAfter());

                if (info.IsGroup)
                {
                    TabCount--;
                }
            }
            
        }
        #endregion

        public static void Debug(object message)
        {
            Logger.Debug(GetBefore()+message + GetAfter());
        }

        public static void Debug(object message, Exception exception)
        {
            Logger.Debug(GetBefore() + message + GetAfter(), exception);
        }

        public static void DebugFormat(string format, params object[] args)
        {
            Logger.DebugFormat(GetBefore() + format + GetAfter(), args);
        }

        public static void DebugFormat(string format, object arg0)
        {
            Logger.DebugFormat(GetBefore() + format + GetAfter(), arg0);
        }

        public static void DebugFormat(string format, object arg0, object arg1)
        {
            Logger.DebugFormat(GetBefore() + format + GetAfter(), arg0, arg1);
        }

        public static void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            Logger.DebugFormat(GetBefore() + format + GetAfter(), arg0, arg1, arg2);
        }

        public static void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            Logger.DebugFormat(GetBefore() + format + GetAfter(), format, args);
        }

        public static void Error(object message)
        {
            Logger.Error(GetBefore() + message + GetAfter());
        }

        public static void Error(object message, Exception exception)
        {
            Logger.Error(GetBefore() + message + GetAfter(), exception);
        }

        public static void ErrorFormat(string format, params object[] args)
        {
            Logger.ErrorFormat(GetBefore() + format + GetAfter(), args);
        }

        public static void ErrorFormat(string format, object arg0)
        {
            Logger.ErrorFormat(GetBefore() + format + GetAfter(), arg0);
        }

        public static void ErrorFormat(string format, object arg0, object arg1)
        {
            Logger.ErrorFormat(GetBefore() + format + GetAfter(), arg0, arg1);
        }

        public static void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            Logger.ErrorFormat(GetBefore() + format + GetAfter(), arg0, arg1, arg2);
        }

        public static void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            Logger.ErrorFormat(GetBefore() + format + GetAfter(), format, args);
        }

        public static void Fatal(object message)
        {
            Logger.Fatal(GetBefore() + message + GetAfter());
        }

        public static void Fatal(object message, Exception exception)
        {
            Logger.Fatal(GetBefore() + message + GetAfter(), exception);
        }

        public static void FatalFormat(string format, params object[] args)
        {
            Logger.FatalFormat(GetBefore() + format + GetAfter(), args);
        }

        public static void FatalFormat(string format, object arg0)
        {
            Logger.FatalFormat(GetBefore() + format + GetAfter(), arg0);
        }

        public static void FatalFormat(string format, object arg0, object arg1)
        {
            Logger.FatalFormat(GetBefore() + format + GetAfter(), arg0, arg1);
        }

        public static void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            Logger.FatalFormat(GetBefore() + format + GetAfter(), arg0, arg1, arg2);
        }

        public static void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            Logger.FatalFormat(GetBefore() + format + GetAfter(), format, args);
        }

        public static void Info(object message)
        {
            Logger.Info(GetBefore() + message + GetAfter());
        }

        public static void Info(object message, Exception exception)
        {
            Logger.Info(GetBefore() + message + GetAfter(), exception);
        }

        public static void InfoFormat(string format, params object[] args)
        {
            Logger.InfoFormat(GetBefore() + format + GetAfter(), args);
        }

        public static void InfoFormat(string format, object arg0)
        {
            Logger.InfoFormat(GetBefore() + format + GetAfter(), arg0);
        }

        public static void InfoFormat(string format, object arg0, object arg1)
        {
            Logger.InfoFormat(GetBefore() + format + GetAfter(), arg0, arg1);
        }

        public static void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            Logger.InfoFormat(GetBefore() + format + GetAfter(), arg0, arg1, arg2);
        }

        public static void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            Logger.InfoFormat(GetBefore() + format + GetAfter(), format, args);
        }

        public static void Warn(object message)
        {
            Logger.Warn(GetBefore() + message + GetAfter());
        }

        public static void Warn(object message, Exception exception)
        {
            Logger.Warn(GetBefore() + message + GetAfter(), exception);
        }

        public static void WarnFormat(string format, params object[] args)
        {
            Logger.WarnFormat(GetBefore() + format + GetAfter(), args);
        }

        public static void WarnFormat(string format, object arg0)
        {
            Logger.WarnFormat(GetBefore() + format + GetAfter(), arg0);
        }

        public static void WarnFormat(string format, object arg0, object arg1)
        {
            Logger.WarnFormat(GetBefore() + format + GetAfter(), arg0, arg1);
        }

        public static void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            Logger.WarnFormat(GetBefore() + format + GetAfter(), arg0, arg1, arg2);
        }

        public static void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            Logger.WarnFormat(GetBefore() + format + GetAfter(), format, args);
        }
    }
}