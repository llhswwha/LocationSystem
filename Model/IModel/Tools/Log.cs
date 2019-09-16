using System;

namespace DbModel.Tools
{
    public static class LogEvent
    {
        public class LogEventInfo
        {
            public string Level = "Info";

            public string Msg = "";

            public string Tag = "";

            public LogEventInfo(string msg, string level, string tag)
            {
                this.Msg = msg;
                this.Level = level;
                this.Tag = tag;
            }

            public LogEventInfo()
            {

            }

            public override string ToString()
            {
                return String.Format("[{0}]{1} {2}", Level, Tag,Msg);
            }
        }

        public static event Action<LogEventInfo> InfoEvent;

        public static void Info(string msg)
        {
            try
            {
                LogEventInfo info=new LogEventInfo(msg,"Info","");
                if (InfoEvent != null)
                {
                    InfoEvent(info);
                }
            }catch(Exception e)
            {

            }            
        }

        public static void Info(string tag,string msg)
        {
            try
            {
                LogEventInfo info = new LogEventInfo(msg, "Info", tag);
                if (InfoEvent != null)
                {
                    InfoEvent(info);
                }
            }
            catch (Exception e)
            {

            }
        }

        public static void Error(Exception ex)
        {
            try
            {
                LogEventInfo info = new LogEventInfo(ex.ToString(), "Error", "");
                //Log.Info(msg);
                if (InfoEvent != null)
                {
                    InfoEvent(info);
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}
