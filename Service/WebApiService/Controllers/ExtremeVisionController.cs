using Location.BLL.Tool;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SignalRService.Hubs;
using System;
using System.IO;
using System.Web.Http;
using WebApiCommunication.ExtremeVision;

namespace WebApiService.Controllers
{
    [RoutePrefix("api/ExtremeVision")]
    public class ExtremeVisionController : ApiController
    {
        [Route("callback")]
        [HttpPost]
        public string Callback(CameraAlarmInfo info)
        {
            try
            {
                string json = JsonConvert.SerializeObject(info);

                Log.Info(LogTags.ExtremeVision, string.Format("ExtremeVisionController.Callback({0})", Request.GetClientIpAddress()));
                Log.Info(LogTags.ExtremeVision, json);
                DateTime now = DateTime.Now;
                string path = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\" + now.ToString("yyyy_mm_dd_HH_MM_ss_fff") + ".json";
                File.WriteAllText(path, json);

                CameraAlarmHub.SendInfo(info);
                return info+"";
            }
            catch (Exception ex)
            {
                Log.Error(LogTags.ExtremeVision, "ExtremeVisionController.Callback:"+ex.Message);
                return "error:"+ex;
            }
        }


        [Route("test")]
        public string Test(JObject obj)
        {
            try
            {
                return obj + "";
            }
            catch (Exception ex)
            {
                return "error:"+ex;
            }
        }

        [Route("get")]
        public string Get()
        {
            try
            {
                return "test1";
            }
            catch (Exception ex)
            {
                return "test2";
            }
        }

        //[Route("getInfo")]
        //public string GetInfo(CameraAlarmInfo info)
        //{
        //    try
        //    {
        //        if (info == null) return "info is null";
        //        //return "test1";
        //        string json = JsonConvert.SerializeObject(info);

        //        Log.Info(LogTags.ExtremeVision, string.Format("ExtremeVisionController.GetInfo({0})", Request.GetClientIpAddress()));
        //        Log.Info(LogTags.ExtremeVision, json);
        //        DateTime now = DateTime.Now;
        //        string path = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\" + now.ToString("yyyy_mm_dd_HH_MM_ss_fff") + ".json";
        //        File.WriteAllText(path, json);

        //        CameraAlarmHub.SendInfo(info);
        //        return info + "";
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //}
    }
}
