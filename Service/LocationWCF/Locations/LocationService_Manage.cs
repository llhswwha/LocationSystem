using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using BLL;
using DbModel.Location.AreaAndDev;
using DbModel.Tools;
using Location.BLL.ServiceHelpers;
using Location.Model.DataObjects.ObjectAddList;
using Location.TModel.FuncArgs;
using Location.TModel.Location;
using Location.TModel.Location.AreaAndDev;
using Location.TModel.Location.Data;
using Location.TModel.Location.Obsolete;
using Location.TModel.Location.Alarm;
using Location.TModel.Location.Person;
using Location.TModel.LocationHistory.Data;
using LocationServices.Converters;
using LocationServices.Tools;
using LocationWCFService;
using LocationWCFService.ServiceHelper;
using ConfigArg = Location.TModel.Location.AreaAndDev.ConfigArg;
using DevInfo = Location.TModel.Location.AreaAndDev.DevInfo;
using KKSCode = Location.TModel.Location.AreaAndDev.KKSCode;
using Post = Location.TModel.Location.AreaAndDev.Post;
using Dev_DoorAccess = Location.TModel.Location.AreaAndDev.Dev_DoorAccess;
using TModel.BaseData;
using TModel.Location.Manage;
using System.Configuration;
using LocationServer.Tools;
namespace LocationServices.Locations
{
    //人员相关的接口
    public partial class LocationService : ILocationService, IDisposable
    {
        public LoginInfo Login(LoginInfo info)
        {
           
            info.Session = Guid.NewGuid().ToString();
            ShowLog(">>>>> Login !!!!!!!!!!!!!!!!!!!! :"+ info.Session);
            return info;
        }

        public LoginInfo Logout(LoginInfo info)
        {
            ShowLog(">>>>> Login !!!!!!!!!!!!!!!!!!!! :"+ info.Session);
            info.Session = "";
            return info;
        }

        public LoginInfo KeepLive(LoginInfo info)
        {
            return info;
        }
        

        public VersionInfo GetVersionInfo()
        {
            VersionInfo version = new VersionInfo();
            version.Version = ConfigurationManager.AppSettings["VersionCode"];
            version.LocationURL = ConfigurationManager.AppSettings["LocationPackageURL"];
            return version;
        }

        public void DebugMessage(string msg)
        {
            ShowLog(msg);
        }
    }
}
