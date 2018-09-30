using System;
using System.Collections.Generic;
using System.ServiceModel;
using Location.Model;

namespace LocationServices.LocationAlarms
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class LocationAlarmService: ILocationAlarmService
    {
        protected static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(LocationAlarmService));
        public delegate void CallbackDelegate<T>(T t);
        //客户端的报警消息接收事件
        public static CallbackDelegate<List<LocationAlarm>> MessageReceived;
        //订阅者
        public static List<AlarmSubscriber> Subscribers = new List<AlarmSubscriber>();

        //用户订阅报警，Alarms代表要订阅的报警类型
        public void Subscribe(int UserId, List<int> Alarms)
        {
            ILocationAlarmServiceCallback callback = OperationContext.Current.GetCallbackChannel<ILocationAlarmServiceCallback>();

            User u = GetUser(UserId);
            AlarmSubscriber subscriber = GetSubscirber(UserId);
            if (subscriber == null)
            {
                subscriber = new AlarmSubscriber();
                subscriber.User = u;
                Subscribers.Add(subscriber);
                logger.Info("客户端" + UserId + "注册");
            }
            subscriber.AlarmTypeList = Alarms; //更新订阅
            subscriber.ClientCallback = callback;
            //绑定退出事件，在客户端退出时，注销客户端的订阅
            ICommunicationObject obj = (ICommunicationObject)callback;
            obj.Closed += new EventHandler(GpsEventService_Closed);
            obj.Closing += new EventHandler(GpsEventService_Closing);
        }

        private AlarmSubscriber GetSubscirber(int UserId)
        {
            foreach (AlarmSubscriber sub in Subscribers)
            {
                if (sub.User.Id == UserId)
                    return sub;
            }
            return null;
        }

        private User GetUser(int UserId)
        {
            return new User(UserId);
        }

        void GpsEventService_Closing(object sender, EventArgs e)
        {
            logger.Info("客户端关闭退出...");
        }

        void GpsEventService_Closed(object sender, EventArgs e)
        {
            ILocationAlarmServiceCallback callback = (ILocationAlarmServiceCallback)sender;
            Subscribers.ForEach(delegate (AlarmSubscriber subscriber)
            {
                if (subscriber.ClientCallback == callback)
                {
                    Subscribers.Remove(subscriber);
                    logger.Info("用户" + subscriber.User.Id + "Closed Client Removed!");
                }

            });

        }
        //客户端断开
        public void Unsubscribe(int UserId)
        {
            ILocationAlarmServiceCallback callback = OperationContext.Current.GetCallbackChannel<ILocationAlarmServiceCallback>();

            Subscribers.ForEach(delegate (AlarmSubscriber subscriber)
            {
                if (subscriber.User.Id == UserId)
                {
                    Subscribers.Remove(subscriber);
                    logger.Info("用户" + subscriber.User.Id + "注销 Client Removed!");
                }

            });
        }

        //向客户端推送报警数据
        public static void SendAlarmMessage(List<LocationAlarm> alarmItems)
        {
            //没有要推送的报警数据
            if (alarmItems.Count == 0)
                return;

            Subscribers.ForEach(delegate (AlarmSubscriber subscriber)
            {
                ICommunicationObject callback = (ICommunicationObject)subscriber.ClientCallback;
                if (((ICommunicationObject)callback).State == CommunicationState.Opened)
                {
                    try
                    {
                        //此处需要加上权限判断、订阅判断等
                        subscriber.ClientCallback.OnMessageReceived(alarmItems);
                    }
                    catch (Exception ex)
                    {
                        Subscribers.Remove(subscriber);
                        logger.Error("用户" + subscriber.User.Id + "出错:" + ex.Message);
                        logger.Error(ex.StackTrace);
                    }
                }
                else
                {
                    Subscribers.Remove(subscriber);
                    logger.Info("用户" + subscriber.User.Id + "Closed Client Removed!");
                }
            });
        }

        //通知用户服务已经停止
        public static void NotifyServiceStop()
        {
            List<LocationAlarm> msgItems = new List<LocationAlarm>();
            msgItems.Add(new LocationAlarm(0, "Stop"));
            SendAlarmMessage(msgItems);
        }
    }
}
