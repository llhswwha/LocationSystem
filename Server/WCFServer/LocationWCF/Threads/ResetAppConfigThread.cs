using Base.Common.Threads;
using BLL;
using DbModel;
using EngineClient;
using LocationServer.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebNSQLib;

namespace LocationServer.Threads
{
    /// <summary>
    /// 运行过程中修改参数
    /// </summary>
    public class ResetAppConfigThread : IntervalTimerThread
    {
        public ResetAppConfigThread() : base(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(30))
        {
            LoadAppConfig(true);//主线程中，要在后面的功能代码前执行
        }

        public override bool TickFunction()
        {
            LoadAppConfig(false);
            return true;
        }

        public static void LoadAppConfig(bool isFirst)
        {
            AppContext.DebugMode = ConfigurationHelper.GetBoolValue("DebugMode");
            AppContext.AutoStartServer = ConfigurationHelper.GetIntValue("AutoStartServer") == 0;
            AppContext.WritePositionLog = ConfigurationHelper.GetBoolValue("WritePositionLog");
            AppContext.PositionMoveStateWaitTime = ConfigurationHelper.GetDoubleValue("PositionMoveStateWaitTime");
            AppContext.PositionMoveStateOfflineTime = ConfigurationHelper.GetDoubleValue("PositionMoveStateOfflineTime");
            AppContext.LowPowerFlag = ConfigurationHelper.GetIntValue("LowPowerFlag");

            AppContext.UrlMaxLength = ConfigurationHelper.GetIntValue("UrlMaxLength");


            AppContext.ParkName = ConfigurationHelper.GetValue("ParkName");

            if(isFirst)
                AppContext.DatacaseWebApiUrl = ConfigurationHelper.GetValue("DatacaseWebApiUrl");
            AppContext.DatacaseWebApiPort = ConfigurationHelper.GetValue("DatacaseWebApiPort");

            AppContext.SaveWebApiJson= ConfigurationHelper.GetBoolValue("SaveWebApiJson");
            AppContext.ShowUnLocatedAreaPoint = ConfigurationHelper.GetBoolValue("ShowUnLocatedAreaPoint");

            AppContext.LogTextBoxMaxLength = ConfigurationHelper.GetIntValue("LogTextBoxMaxLength", 10000);

            AppContext.MoveMaxSpeed = ConfigurationHelper.GetDoubleValue("MoveMaxSpeed", 20);
            AppContext.FilterTodayWhenStart = ConfigurationHelper.GetBoolValue("FilterTodayWhenStart");
            AppContext.FilterMoreThanMaxSpeedTimer = ConfigurationHelper.GetValue("FilterMoreThanMaxSpeedTimer", "04:00");

            AppContext.EnableHistoryBuffer = ConfigurationHelper.GetBoolValue("EnableHistoryBuffer");
            AppContext.DeleteRepeatPositionsWhenLoad = ConfigurationHelper.GetBoolValue("DeleteRepeatPositionsWhenLoad");
            AppContext.HistoryBufferLoadMode = ConfigurationHelper.GetIntValue("HistoryBufferLoadMode");

            AppContext.AnchorScanInterval = ConfigurationHelper.GetIntValue("AnchorScanInterval");
            AppContext.AnchorScanResetCount = ConfigurationHelper.GetIntValue("AnchorScanResetCount",60);
            AppContext.AnchorScanSendMode= ConfigurationHelper.GetIntValue("AnchorScanSendMode");

            AppContext.RepeatDevInfoCheckInterval = ConfigurationHelper.GetIntValue("RepeatDevInfoCheckInterval");
            

            LocationContext.LoadOffset(ConfigurationHelper.GetValue("LocationOffset"));
            LocationContext.LoadInitOffset(ConfigurationHelper.GetValue("InitTopoOffset"));
            LocationContext.Power = ConfigurationHelper.GetIntValue("InitTopoPower");

            EngineClientSetting.LocalIp = ConfigurationHelper.GetValue("Ip");
            EngineClientSetting.EngineIp = ConfigurationHelper.GetValue("EngineIp");
            EngineClientSetting.AutoStart = ConfigurationHelper.GetBoolValue("AutoConnectEngine");
            AppContext.PosEngineKeepAliveInterval = ConfigurationHelper.GetIntValue("PosEngineKeepAliveInterval", 1000);


            //SystemSetting setting = new SystemSetting();
            //XmlSerializeHelper.Save(setting,AppDomain.CurrentDomain.BaseDirectory + "\\default.xml");

            //WebApiHelper.IsSaveJsonToFile = ConfigurationHelper.GetBoolValue("IsSaveJsonToFile");

            RealAlarm.NsqLookupdUrl = ConfigurationHelper.GetValue("NsqLookupdUrl");
            RealAlarm.NsqLookupdTopic = ConfigurationHelper.GetValue("NsqLookupdTopic");
            RealAlarm.NsqLookupdChannel = ConfigurationHelper.GetValue("NsqLookupdChannel");

            DbModel.AppSetting.AddHisPositionInterval = ConfigurationHelper.GetIntValue("AddHisPositionInterval", 30) * 1000;//单位是s，

            AppContext.ActiveFlag = Convert.ToInt32(ConfigurationHelper.GetValue("ActiveFlag"));
            AppContext.FaintFlag = Convert.ToInt32(ConfigurationHelper.GetValue("FaintFlag"));
            AppContext.FaintRange =ConfigurationHelper.GetValue("FaintRange");
            AppContext.FaintEffectiveTime = int.Parse(ConfigurationHelper.GetValue("FaintEffectiveTime"));
            AppContext.FaintTimeInterval = int.Parse(ConfigurationHelper.GetValue("FaintTimeInterval"));

            AppSetting.PositionPower = ConfigurationHelper.GetIntValue("PositionPower",1);

            AppSetting.EnableMigration= ConfigurationHelper.GetBoolValue("EnableMigration");
            AppSetting.EnableHistoryMigration= ConfigurationHelper.GetBoolValue("EnableHistoryMigration");
        }

        protected override void DoBeforeWhile()
        {
            
        }
    }
}
