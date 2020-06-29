using Base.Common.Threads;
using BLL;
using DbModel.Location.AreaAndDev;
using Location.BLL.Tool;
using Location.TModel.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiLib;
using WebApiLib.Clients.OpcCliect;

namespace LocationServer.Threads
{
    public class DevMonitorNodeThread : IntervalTimerThread
    {
        private int isEnd = 1;   //方法是否执行结束
        public DevMonitorNodeThread() 
            : base(TimeSpan.FromSeconds(5)//10秒检查一次
                                         //TimeSpan.FromDays(1)//一天检查一次
                 , TimeSpan.FromSeconds(5))
        {

        }

        public override bool TickFunction()
        {
            if (isEnd == 1)//执行完成
            {
                isEnd = 0;

                try
                {
                    Bll db = Bll.NewBllNoRelation();
                    
                    List<DevMonitorNode> allList = db.DevMonitorNodes.ToList();
                    #region  保存
                    /*
                    int count = allList.Count;
                    //一次取30条
                    int pageCount = 0;
                    if (count % 30 > 0)
                    {
                       pageCount = count/30 + 1;
                    }
                    else
                    {
                        pageCount = count / 30;
                    }
                    //Log.Info("pageCount:" + pageCount);
                    for (int i = 0; i < pageCount; i++)
                    {
                        List<DevMonitorNode> list = allList.GetRange(i,30);
                       // List<DevMonitorNode> list = allList.FindAll(p=>p.TagName.Contains("30LCA42AA101C"));
                        //每次获取50条
                        List<SisData> sisList = getSisList(list);
                        if (sisList == null || sisList.Count == 0)
                        {
                            //Log.Error("DevMonitorNodeThread:opc");
                            #region
                            //try
                            //{
                            //    string opcServerIp = AppContext.OPCServerIP;
                            //    OPCReadAuto opc = new OPCReadAuto(opcServerIp);
                            //    if (opc.IsConnected)
                            //    {
                            //        foreach (DevMonitorNode node in nodeList)
                            //        {
                            //            string tagName = node.TagName;
                            //            string tagNameValue = opc.getOPC(tagName);
                            //            node.Value = tagNameValue;
                            //            node.Time = TimeConvert.ToStamp(DateTime.Now);
                            //        }
                            //        db.DevMonitorNodes.EditRange(nodeList);
                            //    }

                            //}
                            //catch (Exception ex)
                            //{
                            //    return false;
                            //}
                            #endregion
                        }
                        else
                        {
                            string saveSql = "";
                            Dictionary<string, SisData> dic = sisList.ToDictionary(key => key.Name.Trim(), value => value);
                            Log.Info("DevMonitorNodeThread: dic.count="+dic.Count);
                            int dicCount = 0;
                            foreach (DevMonitorNode dev in list)
                            {
                                if (dic.ContainsKey(dev.TagName.Trim()))
                                {
                                    SisData dataT = dic[dev.TagName.Trim()];
                                    dev.Unit = dataT.Unit;
                                    dev.Describe = dataT.Desc;
                                    dev.Value = dataT.Value;
                                    saveSql = string.Format(@"update devmonitornodes set  `Describe`='{0}',`Value`='{1}',Unit='{2}'   where Id={3};", dev.Describe.Trim(), dev.Value, dev.Unit, dev.Id);
                                    string result = db.DevMonitorNodes.AddorEditBySql(saveSql);
                                    Log.Info("sql:"+result);
                                    Log.Info("DevMonitorNodeThread: 更新sis数据,i=" + i + "结果:" + result);
                                    //saveSql += string.Format(@"update devmonitornodes set  `Describe`='{0}',`Value`='{1}',Unit='{2}'   where Id={3};",dev.Describe.Trim(),dev.Value,dev.Unit,dev.Id);
                                    dicCount++;
                                }
                            }
                            //Log.Info("dicCount:" + dicCount);
                           // string result= db.DevMonitorNodes.AddorEditBySql(saveSql);
                            //bool result = db.DevMonitorNodes.EditRange(list);
                            //Log.Info("DevMonitorNodeThread: 更新sis数据,i="+i+"结果:"+result);
                        }
                        
                    }
                    */
                    #endregion
                    #region  单条保存
                    JsonSerializerSettings setting = new JsonSerializerSettings();
                    setting.NullValueHandling = NullValueHandling.Ignore;
                    for (int i = 0; i < allList.Count; i++)
                    {
                        DevMonitorNode dev = allList[i];
                        string tags = dev.TagName.Trim();
                        tags = tags.Replace(" ", "%20").Replace("#", "%23").Replace("+", "%2B").Replace("/", "%2F");
                        string result = WebApiHelper.GetString("http://10.146.33.9:20080/MIS/GetRtMonTagInfosByNames?tagNames=" + tags);
                        List<SisData> sisList = JsonConvert.DeserializeObject<List<SisData>>(result, setting);//只有一条
                        SisData sis = sisList[0];
                       string saveSql = string.Format(@"update devmonitornodes set  `Describe`='{0}',`Value`='{1}',Unit='{2}'   where Id={3};", dev.Describe, sis.Value, sis.Unit, dev.Id);
                        Log.Info("DevMonitorNodeThread:id:" + dev.Id + ",tagName:" + dev.TagName + ",value:" + sis.Value + ",unit:" + sis.Unit);
                        string saveResult = db.DevMonitorNodes.AddorEditBySql(saveSql);
                       
                    }
                        #endregion



                    }
                catch (Exception ex)
                {
                    Log.Error("DevMonitorNodeThread:"+ex.ToString());
                }
                isEnd = 1;
            }
            return true;
        }

        public static List<SisData> getSisList(List<DevMonitorNode> nodeList)
        {
            try
            {
                string tags = "";
                foreach (DevMonitorNode node in nodeList)
                {
                    tags += node.TagName + ",";
                }
                tags = tags.Substring(0, tags.Length - 1);
                //从网址获取（根据sis系统页面网页获取到的地址）
                tags=tags.Replace(" ", "%20").Replace("#", "%23").Replace("+", "%2B").Replace("/", "%2F");
                 string result = WebApiHelper.GetString("http://10.146.33.9:20080/MIS/GetRtMonTagInfosByNames?tagNames=" + tags);
                //string result = "[{\"Id\": 832, \"Name\": \"30LCA42AA101C\", \"Type\": 0, \"Desc\": \"凝结水再开式循环调节阀控制信号\", \"Unit\": \"%\", \"ExDesc\": \"\", \"State\": 0,\"ControlSystemName\": null,\"Value\": 100.0, \"ROLEType\": 0,\"Key\": null}]";
                Log.Info("DevMonitorNodeThread.getSisList.result" + result);
                //List <SisData> sisList = WebApiHelper.GetEntity<List<SisData>>("http://10.146.33.9:20080/MIS/GetRtMonTagInfosByNames?tagNames=" + tags);
                JsonSerializerSettings setting = new JsonSerializerSettings();
                setting.NullValueHandling = NullValueHandling.Ignore;
                List<SisData> sisList = JsonConvert.DeserializeObject<List<SisData>>(result,setting);
                return sisList;
            }
            catch (Exception ex)
            {
                Log.Error("getSisListInThread:" + ex.ToString());
                return null;
            }
        }

        protected override void DoBeforeWhile()
        {
           
        }
    }
}
