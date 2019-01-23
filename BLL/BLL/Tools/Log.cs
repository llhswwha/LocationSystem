using log4net.Core;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Location.BLL.Tool
{
    public  static class Log
    {


       public   static log4net.ILog Logger=log4net.LogManager.GetLogger("Logger");

        private static bool logWatching = true;
        private static log4net.Appender.MemoryAppender logger;
        private static Thread logWatcher;

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

        //static Log()
        //{

        //}

        public static void StartWatch()
        {
            if (logger == null)
            {
                logger = new log4net.Appender.MemoryAppender();
                log4net.Config.BasicConfigurator.Configure(logger);
            }

            logWatcher = new Thread(new ThreadStart(LogWatcher));
            logWatcher.IsBackground = true;
            logWatcher.Start();
        }


        public static void StopWatch()
        {
            if (logWatcher != null)
            {
                logWatcher.Abort();
                logWatcher = null;
            }
        }

        private static void LogWatcher()
        {
            while (logWatching)
            {
                LoggingEvent[] events = logger.GetEvents();
                if (events != null && events.Length > 0)
                {
                    logger.Clear();
                    foreach (LoggingEvent ev in events)
                    {
                        //2018-10-27 12:46:53,954 [10] INFO  Logger - 0 App_OnStartup
                        //string line =ev.ToString();
                        //%d{yyyy-MM-dd HH:mm:ss,fff} %-5level [%c:%line] - %message%newline
                        //%d [%t] %-5p %c - %m%n
                        string line = string.Format("{0} [{1}] {2} {3} - {4} {5}",ev.TimeStamp,ev.ThreadName,ev.Level,ev.LoggerName,ev.MessageObject,ev.ExceptionObject);
                        //string line = ev.LoggerName + ": " + ev.RenderedMessage + "\r\n";
                        if (NewLogEvent != null)
                        {
                            NewLogEvent(line);
                        }
                    }
                }
                Thread.Sleep(250);
            }
        }

        public static event Action<string> NewLogEvent;

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

        public static void AppStart()
        {
            Info("");
            Info("");
            Info("====================================");
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