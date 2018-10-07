using Location.TModel.Location.Alarm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using WCFServiceForWPF.LocationCallbackServices;

namespace WCFClientLib
{
    public class LocationCallbackClient:WCFClient
        //,ILocationAlarmServiceCallback
    {
        public LocationAlarmServiceClient InnerClient { get; set; }

        public LocationCallbackClient(string host, string port):base(host,port,WCFClientHostType.Self)
        {
           
        }

        protected void SetConnectInfo()
        {
            Url =
    string.Format("net.tcp://{0}:{1}/LocationServices/LocationCallbacks/LocationCallbackService",
        Host, Port);
            NetTcpBinding wsBinding = new NetTcpBinding();
            wsBinding.MaxReceivedMessageSize = int.MaxValue;
            Binding = wsBinding;
            EndpointAddress = new EndpointAddress(Url);

            InnerClient = new LocationAlarmServiceClient(
                //new InstanceContext(this), 
                Binding, EndpointAddress);
        }

        public bool Connect()
        {
            try
            {
                SetConnectInfo();
                InnerClient.Connect();
                return true;
            }
            catch (Exception ex)
            {
                Exception = ex;
                return false;
            }
        }

        public bool Disconnect()
        {
            try
            {
                if (InnerClient != null)
                {
                    InnerClient.DisConnect();
                }
                return true;
            }
            catch (Exception ex)
            {
                Exception = ex;
                return false;
            }
        }

        public event Action<LocationAlarm[]> LocAlarmsReceved;

        protected void OnLocAlarmsReceved(LocationAlarm[] locAlarms)
        {
            if (LocAlarmsReceved != null)
            {
                LocAlarmsReceved(locAlarms);
            }
        }

        public void AlarmInfo(LocationAlarm[] locAlarms)
        {
            OnLocAlarmsReceved(locAlarms);

            //string txt = "";
            //foreach (var alarm in locAlarms)
            //{
            //    txt += alarm.AlarmType + "|" + alarm.Content + "\n";
            //}
            ////Tb3DPosResult.Text = GetLogText(txt);
            //MessageBox.Show("收到告警:" + txt);
        }
    }
}
