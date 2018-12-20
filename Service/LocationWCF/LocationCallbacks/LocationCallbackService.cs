using System;
using System.Collections.Generic;
using System.ServiceModel;
using Location.TModel.Location.Alarm;

namespace LocationServices.LocationCallbacks
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class LocationCallbackService : ILocationAlarmService
        //, ILocationInfoService
    {
        static List<ILocationAlarmServiceCallback> m_Callbacks = new List<ILocationAlarmServiceCallback>();

        static ILocationInfoServiceCallback m_InfoCallbacksRsetful;

 //       static Dictionary<int, ILocationInfoServiceCallback> m_InfoCallbacksClient = new Dictionary<int, ILocationInfoServiceCallback>();

        static int m_nCount = 0;

        /*****************用于告警**************************/

        public void Connect()
        {
            //ILocationAlarmServiceCallback callback = OperationContext.Current.GetCallbackChannel<ILocationAlarmServiceCallback>();
            //if (m_Callbacks.Contains(callback) == false)
            //{
            //    m_Callbacks.Add(callback);
            //}
        }

        public void DisConnect()
        {
            //ILocationAlarmServiceCallback callback = OperationContext.Current.GetCallbackChannel<ILocationAlarmServiceCallback>();
            //if (m_Callbacks.Contains(callback))
            //{
            //    m_Callbacks.Remove(callback);
            //}
            //else
            //{
            //    throw new InvalidOperationException("Cannot find callback");
            //}
        }

        public static void ActiveAlarmReport(List<LocationAlarm> localAlarms)
        {
            Action<ILocationAlarmServiceCallback> invoke = callback => callback.AlarmInfo(localAlarms);
            m_Callbacks.ForEach(invoke);
        }

        ////通知用户服务已经停止
        //public static void NotifyServiceStop()
        //{
        //    List<LocationAlarm> msgItems = new List<LocationAlarm>();
        //    msgItems.Add(new LocationAlarm() { Id = 0,Content="告警" });
        //    ActiveAlarmReport(msgItems);
        //}

        public void AddAlarmInfo(string AlarmInfo)
        {
            List<LocationAlarm> msgItems = new List<LocationAlarm>();
            //msgItems.Add(new LocationAlarm(1, AlarmInfo));
            ActiveAlarmReport(msgItems);
        }

        /*****************用于获取Rsetful数据**************************/
        //获取信息的回调中，客户端不需要注册和销毁
        public void InfoConnect()
        {
            ILocationInfoServiceCallback callback = OperationContext.Current.GetCallbackChannel<ILocationInfoServiceCallback>();
            if (callback != null)
            {
                m_InfoCallbacksRsetful = callback;
            }
        }

        public void DisInfoConnect()
        {
            ILocationInfoServiceCallback callback = OperationContext.Current.GetCallbackChannel<ILocationInfoServiceCallback>();
            if (callback != null && callback == m_InfoCallbacksRsetful)
            {
                m_InfoCallbacksRsetful = null;
            }
        }

        public List<string> GetInfo(int i)
        {
            List<string> DevInfos = new List<string>();

            if (m_InfoCallbacksRsetful != null)
            {
                DevInfos = m_InfoCallbacksRsetful.GetInfoFromRsetful(i);
               
            }

            return DevInfos;
        }

    }
}
