using NsqSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebNSQLib.RecvMessage;

namespace WebNSQLib
{
    public class RecvBaseCommunication
    {
        //视频告警或周界告警
        public RecvVideoOrPerimeterAlarm RvoPa = new RecvVideoOrPerimeterAlarm();

        //门禁告警
        public RecvAccessControlAlarm Raca = new RecvAccessControlAlarm();

        //消防告警
        public RecvFireAlarm Rfa = new RecvFireAlarm();

        public void StartConnectTopic()
        {
            try
            {
                ConnectRecvVideoOrPerimeterAlarmTopic();
                ConnectRecvAccessControlAlarmTopic();
                ConnectInitRecvFireAlarmTopic();

            }
            catch (Exception ex)
            {
                string strError = ex.Message;
            }

            while (true) { Thread.Sleep(6000); }

        }

        /// <summary>
        /// 视频或周界告警
        /// </summary>
        private void ConnectRecvVideoOrPerimeterAlarmTopic()
        {
            var consumer = new Consumer("ipms_hus", "test_channel");
            consumer.AddHandler(RvoPa);
            consumer.ConnectToNsqLookupd("172.16.100.22:4161");
            return;
        }


        /// <summary>
        /// 连接门禁告警NSQ Toppic
        /// </summary>
        private void ConnectRecvAccessControlAlarmTopic()
        {
            var consumer = new Consumer("honeywell", "channel-name");
            consumer.AddHandler(Raca);
            consumer.ConnectToNsqLookupd("172.16.100.22:4161");
            return;
        }

        /// <summary>
        /// 连接消防告警NSQ  topic
        /// </summary>
        private void ConnectInitRecvFireAlarmTopic()
        {
            var consumer = new Consumer("fire", "channel-name");
            consumer.AddHandler(Rfa);
            consumer.ConnectToNsqLookupd("172.16.100.22:4161");
            return;
        }


    }
}
