using BLL;
using DbModel.Location.Alarm;
using Location.BLL.Tool;
using Location.TModel.Tools;
using LocationServices.Converters;
using SignalRService.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WebNSQLib;

namespace LocationServer.Windows.Simple
{
    /// <summary>
    /// AlarmList.xaml 的交互逻辑
    /// </summary>
    public partial class AlarmList : Window
    {
        public AlarmList()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GetPageDate();
        }


        /// <summary>
        /// 清除告警并更新列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Click(object sender, RoutedEventArgs e)
        {

          int   day = Convert.ToInt32(TxtDay.Text);
            bool result=RemoveList(day);
            if(result)
            {
                GetPageDate();
            }
            
        }
        /// <summary>
        /// 页面数据显示
        /// </summary>
        private void GetPageDate()
        {
            try
            {
                Bll db = Bll.NewBllNoRelation();
                List<DevAlarm> list = db.DevAlarms.ToList();
                DataGridAlarmList.ItemsSource = list;
                TxtDevNum.Text = list.Count().ToString();
            }
            catch (Exception ex)
            {
                Log.Info("GetDevAlarms:"+ex.ToString());
            }
        }

        private bool RemoveList(int day)
        {
            try
            {
                //清除列表
                Bll db = Bll.NewBllNoRelation();
                DateTime nowTime = DateTime.Now;
                DateTime starttime = DateTime.Now.AddDays(-day);
                var starttimeStamp = TimeConvert.ToStamp(starttime);
                var query = db.DevAlarms.DbSet.Where(i => i.AlarmTimeStamp < starttimeStamp);
                if (query.Count() > 0)
                {
                    query.DeleteFromQuery();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Info("RemoveDevAlarms:"+ex.ToString());
                return false;
            }
            
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if(alarmReceiveThread!= null)
            {
                alarmReceiveThread.Abort();
            }
        }


        Thread alarmReceiveThread;
        private void BtnStartReceive_Click(object sender, RoutedEventArgs e)
        {
            RealAlarm ra = new RealAlarm(Mh_DevAlarmReceived);
            if (alarmReceiveThread == null)
            {
                alarmReceiveThread = new Thread(ra.ReceiveRealAlarmInfo);
                alarmReceiveThread.IsBackground = true;
                alarmReceiveThread.Start();
            }
        }

        private void Mh_DevAlarmReceived(DbModel.Location.Alarm.DevAlarm obj)
        {
            AlarmHub.SendDeviceAlarms(obj.ToTModel());
        }
    }
}
