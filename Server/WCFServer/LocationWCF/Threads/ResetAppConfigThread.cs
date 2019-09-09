using Base.Common.Threads;
using BLL;
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
            LoadAppConfig();
        }

        public override bool TickFunction()
        {
            LoadAppConfig();
            return true;
        }

        public static void LoadAppConfig()
        {
            AppContext.AutoStartServer = ConfigurationHelper.GetIntValue("AutoStartServer") == 0;
            AppContext.WritePositionLog = ConfigurationHelper.GetBoolValue("WritePositionLog");
            AppContext.PositionMoveStateWaitTime = ConfigurationHelper.GetDoubleValue("PositionMoveStateWaitTime");
            AppContext.PositionMoveStateOfflineTime = ConfigurationHelper.GetDoubleValue("PositionMoveStateOfflineTime");
            AppContext.LowPowerFlag = ConfigurationHelper.GetIntValue("LowPowerFlag");

            AppContext.UrlMaxLength = ConfigurationHelper.GetIntValue("UrlMaxLength");


            AppContext.ParkName = ConfigurationHelper.GetValue("ParkName");
            AppContext.DatacaseWebApiUrl = ConfigurationHelper.GetValue("DatacaseWebApiUrl");
            AppContext.ShowUnLocatedAreaPoint = ConfigurationHelper.GetBoolValue("ShowUnLocatedAreaPoint");

            AppContext.LogTextBoxMaxLength = ConfigurationHelper.GetIntValue("LogTextBoxMaxLength", 10000);

            AppContext.MoveMaxSpeed = ConfigurationHelper.GetDoubleValue("MoveMaxSpeed", 20);
            AppContext.FilterTodayWhenStart = ConfigurationHelper.GetBoolValue("FilterTodayWhenStart");
            AppContext.FilterMoreThanMaxSpeedTimer = ConfigurationHelper.GetValue("FilterMoreThanMaxSpeedTimer", "04:00");


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
        }
    }
}
