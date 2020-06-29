using BLL;
using BLL.Blls.LocationHistory;
using DbModel.LocationHistory.Door;
using Location.BLL.Tool;
using LocationServices.ObjToData;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TModel.FuncArgs;
using TModel.LocationHistory.AreaAndDev;
using TModel.Tools;

namespace LocationServices.Locations.Services
{
    public interface IDoorClickService
    {
        DataDoor GetPageByCondition(DoorSearchArgs args);
        List<DoorClick> GetListByCondition(DateTime dtBeginTime, DateTime dtEndTime, string doorIndexCode);

        PageInfo<DevEntranceGuardCardsHistroy> GetCardPageByCondition(DoorSearchArgsSH args);
    }

    public class DoorClickService : IDoorClickService
    {
        private Bll db;
        private DoorClickBll DoorClickSet;    //嘉明门禁

        public DoorClickService()
        {
            db = Bll.NewBllNoRelation();
            DoorClickSet = db.DoorClicks;
        }

        public DoorClickService(Bll bll)
        {
            this.db = bll;
            DoorClickSet = db.DoorClicks;
        }
        /*嘉明*/
        public List<DoorClick> GetListByJson()
        {

            List<DoorClick> doorClickList = new List<DoorClick>();
          
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + "Data\\DeviceData\\中山嘉明电厂\\门禁\\门禁事件.json";
                StreamReader sr = new StreamReader(path, Encoding.Default);
                string line;
                string jsonobj = "";
                while ((line = sr.ReadLine()) != null)
                {
                    jsonobj = jsonobj + line.ToString();
                }
                DoorEvents doorEvents = JsonConvert.DeserializeObject<DoorEvents>(jsonobj);
                doorClickList = doorEvents.data.list;
               
            }
            catch (Exception ex)
            {
                Log.Error("DoorClickService.GetListByJson:"+ex.ToString());
            }
            return doorClickList;
        }

        public DataDoor GetPageByCondition(DoorSearchArgs args)
        { 
            //时间要传格林尼治时间
            DataDoor data = new DataDoor();
            try
            {
                DoorSearchArgs arg = args;
                arg.eventType = "198914";
                DateTime now = DateTime.Now;
                if (args == null)
                {
                    arg = new DoorSearchArgs();
                    arg.startTime =now.AddMonths(-1).ToString("yyyy-MM-dd HH:mm:ss");
                    arg.endTime = now.ToString("yyyy-MM-dd HH:mm:ss");
                    arg.pageNo = 1;
                    arg.pageSize = 100;
                    arg.doorIndexCodes = null;
                    arg.personIds = null;
                    arg.eventType = "198914";
                }
                else
                {
                    if (string.IsNullOrEmpty( arg.startTime)|| string.IsNullOrEmpty(arg.endTime))
                    {
                        arg.startTime = now.AddMonths(-1).ToString("yyyy-MM-dd HH:mm:ss");
                        arg.endTime = now.ToString("yyyy-MM-dd HH:mm:ss");     
                    }
                    if (arg.pageSize==0)
                    {
                        arg.pageSize = 1000000;
                    }
                    if (arg.pageNo == 0)
                    {
                        arg.pageNo = 1;
                    }
                    if (string.IsNullOrEmpty(arg.eventType))
                    {
                        arg.eventType = "198914";
                    }
                }

                DateTime startTimeT;
                DateTime endTimeT;
                DateTime.TryParse(arg.startTime, out startTimeT);
                DateTime.TryParse(arg.endTime, out endTimeT);

                //对接部分：暂时注释
                ////DoorEvents doorEvents = getDoorEvents(arg.startTime.ToString("yyyy-MM-ddTHH:mm:sszzz"), arg.endTime.ToString("yyyy-MM-ddTHH:mm:sszzz"), arg.pageNo.ToString(), arg.pageSize.ToString(), arg.eventType, arg.personIds, arg.doorIndexCodes,arg.personName);
                DoorEvents doorEvents = getDoorEvents(startTimeT.ToString("yyyy-MM-ddTHH:mm:sszzz"), endTimeT.ToString("yyyy-MM-ddTHH:mm:sszzz"), arg.pageNo.ToString(), arg.pageSize.ToString(), arg.eventType, arg.personIds, arg.doorIndexCodes, arg.personName);
                if (doorEvents != null && doorEvents.data != null)
                {
                    data = doorEvents.data;
                }

                if (data.list == null)//从数据库获取
                {
                    //PageInfo<DoorClick> page = db.DoorClicks.GetPageByCondition(arg.startTime, arg.endTime, arg.eventType, arg.personIds, arg.doorIndexCodes,arg.personName, arg.pageNo, arg.pageSize);
                    PageInfo<DoorClick> page = db.DoorClicks.GetPageByCondition(startTimeT, endTimeT, arg.eventType, arg.personIds, arg.doorIndexCodes, arg.personName, arg.pageNo, arg.pageSize);
                    data.total = page.total;
                    data.totalPage = page.totalPage;
                    data.list = page.data;
                    data.pageNo = page.pageIndex;
                    data.pageSize = page.pageSize;
                }
            }
            catch (Exception ex)
            {
                Log.Error("DoorClickService.GetListByCondition:"+ex.ToString());
            }
          
            return data;
        }
        public List<DoorClick> GetListByCondition(DateTime dtBeginTime, DateTime dtEndTime, string doorIndexCode)
        {
            string[] doorIndexCodes = new string[] { doorIndexCode };
            DoorSearchArgs args = new DoorSearchArgs();
            DateTime now = new DateTime();
            if (dtBeginTime != null||dtEndTime!=null)
            {
                args.startTime = dtBeginTime.ToString("yyyy-MM-ddTHH:mm:sszzz");
                args.endTime = dtEndTime.ToString("yyyy-MM-ddTHH:mm:sszzz");
            }
            else
            {
                args.startTime = now.AddMonths(-1).ToString("yyyy-MM-ddTHH:mm:sszzz");
                args.endTime = now.ToString("yyyy-MM-ddTHH:mm:sszzz");
            }
            args.doorIndexCodes = doorIndexCodes;
            args.pageNo = 1;
            args.pageSize = 100000;
            args.eventType = "198914";
            return GetPageByCondition(args).list;
        }

        /// <summary>
        /// 获取门禁事件列表(海康接口)
        /// </summary>
        public DoorEvents getDoorEvents(string startTime, string endTime, string pageNo, string pageSize, string eventType/*必要(事件类型)*/, string[] personIds, string[] doorIndexCodes,string personName)
        {
             DoorEvents doorEvents = new DoorEvents();
            try
            {
                //注意时间转化
                DateTime dt = DateTime.Now;
                Log.Info("当前时间-普通格式：{0}", dt.ToString("yyyy-MM-dd HH:mm:ss"));
                Log.Info("当前时间-UTC格式：{0}", dt.ToString("yyyy-MM-ddTHH:mm:sszzz"));
                // "{\"startTime\": \"2020-05-16T13:00:00.000+08:00\",\"endTime\": \"2020-05-18T12:00:00.000+08:00\",\"pageNo\": \"1\",\"pageSize\": \"100\",\"eventType\": \"198914\"}";
                string where = "";
             
                if (!string.IsNullOrEmpty(startTime))
                {
                    where += "\"startTime\": \"" + startTime + "\",";
                }
                if (!string.IsNullOrEmpty(endTime))
                {
                    where += "\"endTime\": \"" + endTime + "\",";
                }
                if (!string.IsNullOrEmpty(pageNo))
                {
                    where += "\"pageNo\": \"" + pageNo + "\",";
                }
                if (!string.IsNullOrEmpty(pageSize))
                {
                    where += "\"pageSize\": \"" + pageSize + "\",";
                }
                if (!string.IsNullOrEmpty(eventType))
                {
                    where += "\"eventType\": \"" + eventType + "\",";
                }
                if (!string.IsNullOrEmpty(personName))
                {
                    personName = personName.Replace("/","").Replace("<","").Replace(">","").Replace("\"","").Replace("|","");
                    where += "\"personName\": \"" + personName + "\",";
                }

                //"doorIndexCodes": ["1f276203e5234bdca08f7d99e1097bba","3f9512ec067248dfa0679cf4a1634800"]
                if (personIds != null && personIds.Length != 0)
                {
                    string personIdss = "";
                    foreach (string personId in personIds)
                    {
                        personIdss += "\"" + personId + "\",";
                    }
                    personIdss = personIdss.Substring(0, personIdss.Length - 1);
                    where += "\"personIds\": [" + personIdss + "],";
                }
                where = where.Substring(0, where.Length - 1);
                string body = "{" + where + "}";
                string uri = "/artemis/api/acs/v1/door/events";
                Log.Info("DoorClickService.getDoorEvents: url=" + uri);
                Log.Info("DoorClickService.getDoorEvents: body=" + body);
                string obj = getString(body, uri);
                doorEvents = JsonConvert.DeserializeObject<DoorEvents>(obj);
            }
            catch (Exception ex)
            {
                Log.Error("DevService.getDoorEvents:" + ex.ToString());
            }
            return doorEvents;
        }

        public static string getString(string body, string uri)
        {
            try
            {
                HttpUtillib.SetPlatformInfo("21762820", "yvtbOgoYSPOh2fA1Kvbv", "10.146.33.21", 80, false);
                byte[] result = HttpUtillib.HttpPost(uri, body, 15);
                if (null == result)
                {
                    return null;
                }
                else
                {
                    string tmp = System.Text.Encoding.UTF8.GetString(result);
                    Log.Info("DoorClickService.getString: tmp="+tmp);
                    //
                    JObject obj = null;
                    try
                    {
                        obj = (JObject)JsonConvert.DeserializeObject(tmp);

                        // 说明是字符串，并且请求失败了
                    }
                    catch (Exception e)
                    {
                        // 转换成json对象异常说明响应是字节流
                        File.WriteAllBytes("D://test.jpeg", result);
                        Console.WriteLine("写入图片成功：D://test.jpeg\n");
                    }
                    return tmp;


                }
            }
            catch (Exception ex)
            {
                Log.Info("DoorClickService.getString:" + ex.ToString());
                return null;
            }
        }

        /*四会*/
        public PageInfo<DevEntranceGuardCardsHistroy> GetCardPageByCondition(DoorSearchArgsSH args)
        {
            PageInfo<DevEntranceGuardCardsHistroy> page = new PageInfo<DevEntranceGuardCardsHistroy>();
            try
            {
                if (args == null)
                {
                    args = new DoorSearchArgsSH();
                }
                page = db.DevEntranceGuardCardActions.getPageByCondition(args.device_id,args.startTime,args.endTime,args.pageNo,args.pageSize);
            }
            catch (Exception ex)
            {
                Log.Error("DoorClickService.GetCardPageByCondition:" + ex.ToString());
            }
            return page;
        }

    }
}
