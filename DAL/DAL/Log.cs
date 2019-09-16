using log4net.Core;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Location.BLL.Tool
{
    public static class LogTags
    {
        /// <summary>
        /// 获取数据库数据
        /// </summary>
        public static string DbGet = "[DbGet]";

        /// <summary>
        /// 客户端发送消息(WCF)
        /// </summary>
        public static string WCF = "[WCF]";

        /// <summary>
        /// 客户端发送消息(WebApi)
        /// </summary>
        public static string WebApi = "[WebApi]";

        /// <summary>
        /// 服务端消息(Server)
        /// </summary>
        public static string Server = "[Server]";

        /// <summary>
        /// 数据初始化
        /// </summary>
        public static string DbInit = "[DbInit]";

        /// <summary>
        /// 极视角
        /// </summary>
        public static string ExtremeVision = "[ExtremeVision]";

        /// <summary>
        /// 光谱基础数据平台
        /// </summary>
        public static string BaseData = "[BaseData]";

        /// <summary>
        /// 定位引擎
        /// </summary>
        public static string Engine = "[Engine]";

        /// <summary>
        /// 定位数据写入数据库
        /// </summary>
        public static string Engine2Db = "[Engine2Db]";

        /// <summary>
        /// KKS
        /// </summary>
        public static string KKS = "[KKS]";

        /// <summary>
        /// 告警事件测试
        /// </summary>
        public static string EventTest = "[EventTest]";

        /// <summary>
        /// 历史定位数据获取
        /// </summary>
        public static string HisPos = "[HisPos]";

        /// <summary>
        /// 历史统计数据缓存
        /// </summary>
        public static string HisPosBuffer = "[HisPosBuffer]";

        /// <summary>
        /// EF调试内容
        /// </summary>
        public static string EF = "[EF]";
    }

    public  static class Log
    {


       public   static log4net.ILog Logger=log4net.LogManager.GetLogger("Logger");

        private static bool logWatching = true;
        private static log4net.Appender.MemoryAppender logger;
        private static Thread logWatcher;

        #region LogInfoStart->End
        private class LogGroup
        {
            public string Tag;
            public string Flag;
            public DateTime Time;
            public bool IsGroup;
        }
        static Dictionary<string, LogGroup> infos=new Dictionary<string, LogGroup>();

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

        private static string GetPrefix()
        {
            return GetTabString();
        }

        private static string GetSuffix()
        {
            return "";
        }

        private static void AddInfo(LogGroup info)
        {
            string flag = info.Flag;
            if (infos.ContainsKey(flag))
            {
                infos[flag] = info;
            }
            else
            {
                infos.Add(flag, info);
            }

            if (info.IsGroup)
            {
                TabCount++;
            }
        }

        public static void InfoStart(string tag,string flag,bool isGroup=false)
        {
            try
            {
                LogGroup info = new LogGroup() { Tag=tag,Flag = flag, Time = DateTime.Now, IsGroup = isGroup };
                AddInfo(info);
                Info(tag, flag + " Start ");
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }

        public static void InfoEnd(string flag)
        {
            try
            {
                if (infos.ContainsKey(flag))
                {
                    LogGroup info = infos[flag];
                    TimeSpan timeSpan = DateTime.Now - info.Time;

                    Info(info.Tag, flag + " End Time:" + timeSpan);

                    if (info.IsGroup)
                    {
                        TabCount--;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }
        #endregion

        //static Log()
        //{

        //}

        public static int LogCount = 0;

        public static void StartWatch()
        {
            LogCount++;
            if (logWatcher == null)
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
        }


        public static void StopWatch()
        {
            LogCount--;
            if (LogCount <= 0)
            {
                if (logWatcher != null)
                {
                    logWatcher.Abort();
                    logWatcher = null;
                }
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
                        string line = string.Format("{0} [{1}] {2} {3} - {4} {5}",ev.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss,fff"),ev.ThreadName,ev.Level,ev.LoggerName,ev.MessageObject,ev.ExceptionObject);
                        //string line = ev.LoggerName + ": " + ev.RenderedMessage + "\r\n";

                        string tag = "[None]";
                        string msg = (ev.MessageObject + "");
                        int index = msg.IndexOf('|');
                        if (index > 0)
                        {
                            tag = msg.Substring(0, index);
                        }
                        
                        if (NewLogEvent != null)
                        {
                            LogInfo info = new LogInfo();
                            info.Tag = tag;
                            info.Log = line;
                            NewLogEvent(info);
                        }
                    }
                }
                Thread.Sleep(250);
            }
        }

        public static event Action<LogInfo> NewLogEvent;

        public static void Debug(object message)
        {
            Logger.Debug(GetPrefix()+message + GetSuffix());
        }

        public static void Debug(object message, Exception exception)
        {
            Logger.Debug(GetPrefix() + message + GetSuffix(), exception);
        }

        public static void DebugFormat(string format, params object[] args)
        {
            Logger.DebugFormat(GetPrefix() + format + GetSuffix(), args);
        }

        public static void DebugFormat(string format, object arg0)
        {
            Logger.DebugFormat(GetPrefix() + format + GetSuffix(), arg0);
        }

        public static void DebugFormat(string format, object arg0, object arg1)
        {
            Logger.DebugFormat(GetPrefix() + format + GetSuffix(), arg0, arg1);
        }

        public static void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            Logger.DebugFormat(GetPrefix() + format + GetSuffix(), arg0, arg1, arg2);
        }

        public static void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            Logger.DebugFormat(GetPrefix() + format + GetSuffix(), format, args);
        }

        public static void Error(object message)
        {
            Logger.Error(GetPrefix() + message + GetSuffix());
        }

        public static void Error(object message, Exception exception)
        {
            Logger.Error(GetPrefix() + message + GetSuffix(), exception);
        }

        public static void ErrorFormat(string format, params object[] args)
        {
            Logger.ErrorFormat(GetPrefix() + format + GetSuffix(), args);
        }

        public static void ErrorFormat(string format, object arg0)
        {
            Logger.ErrorFormat(GetPrefix() + format + GetSuffix(), arg0);
        }

        public static void ErrorFormat(string format, object arg0, object arg1)
        {
            Logger.ErrorFormat(GetPrefix() + format + GetSuffix(), arg0, arg1);
        }

        public static void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            Logger.ErrorFormat(GetPrefix() + format + GetSuffix(), arg0, arg1, arg2);
        }

        public static void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            Logger.ErrorFormat(GetPrefix() + format + GetSuffix(), format, args);
        }

        public static void Fatal(object message)
        {
            Logger.Fatal(GetPrefix() + message + GetSuffix());
        }

        public static void Fatal(object message, Exception exception)
        {
            Logger.Fatal(GetPrefix() + message + GetSuffix(), exception);
        }

        public static void FatalFormat(string format, params object[] args)
        {
            Logger.FatalFormat(GetPrefix() + format + GetSuffix(), args);
        }

        public static void FatalFormat(string format, object arg0)
        {
            Logger.FatalFormat(GetPrefix() + format + GetSuffix(), arg0);
        }

        public static void FatalFormat(string format, object arg0, object arg1)
        {
            Logger.FatalFormat(GetPrefix() + format + GetSuffix(), arg0, arg1);
        }

        public static void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            Logger.FatalFormat(GetPrefix() + format + GetSuffix(), arg0, arg1, arg2);
        }

        public static void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            Logger.FatalFormat(GetPrefix() + format + GetSuffix(), format, args);
        }

        public static void AppStart()
        {
            Info("");
            Info("");
            Info("====================================");
        }

        //public static List<string> Tags = new List<string>();

        public static void Info(string tag, object message)
        {
            //if (!Tags.Contains(tag))
            //{
            //    Tags.Add(tag);
            //}
            string log = tag + "|" + GetPrefix() + message + GetSuffix();
            Logger.Info(log);
        }

        public static void Error(string tag, string message)
        {
            string log = tag + "|" + GetPrefix() + message + GetSuffix();
            Logger.Error(log);
        }
        public static void Error(string tag,string funcName, string message)
        {
            string log = tag + "|" +funcName+">>"+ GetPrefix() + message + GetSuffix();
            Logger.Error(log);
        }

        public static void Info(object message)
        {
            string log = GetPrefix() + message + GetSuffix();
            Logger.Info(log);
        }

        public static void Info(object message, Exception exception)
        {
            Logger.Info(GetPrefix() + message + GetSuffix(), exception);
        }

        public static void InfoFormat(string format, params object[] args)
        {
            Logger.InfoFormat(GetPrefix() + format + GetSuffix(), args);
        }

        public static void InfoFormat(string format, object arg0)
        {
            Logger.InfoFormat(GetPrefix() + format + GetSuffix(), arg0);
        }

        public static void InfoFormat(string format, object arg0, object arg1)
        {
            Logger.InfoFormat(GetPrefix() + format + GetSuffix(), arg0, arg1);
        }

        public static void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            Logger.InfoFormat(GetPrefix() + format + GetSuffix(), arg0, arg1, arg2);
        }

        public static void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            Logger.InfoFormat(GetPrefix() + format + GetSuffix(), format, args);
        }

        public static void Warn(object message)
        {
            Logger.Warn(GetPrefix() + message + GetSuffix());
        }

        public static void Warn(object message, Exception exception)
        {
            Logger.Warn(GetPrefix() + message + GetSuffix(), exception);
        }

        public static void WarnFormat(string format, params object[] args)
        {
            Logger.WarnFormat(GetPrefix() + format + GetSuffix(), args);
        }

        public static void WarnFormat(string format, object arg0)
        {
            Logger.WarnFormat(GetPrefix() + format + GetSuffix(), arg0);
        }

        public static void WarnFormat(string format, object arg0, object arg1)
        {
            Logger.WarnFormat(GetPrefix() + format + GetSuffix(), arg0, arg1);
        }

        public static void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            Logger.WarnFormat(GetPrefix() + format + GetSuffix(), arg0, arg1, arg2);
        }

        public static void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            Logger.WarnFormat(GetPrefix() + format + GetSuffix(), format, args);
        }
    }

    public class LogInfo
    {
        private string _tag = "";
        public string Tag
        {
            get
            {
                return _tag;
            }
            set
            {
                _tag = value;
                //if (_tag.Length > 30)//调试
                //{

                //}
            }
        }

        public string Log;
    }
}