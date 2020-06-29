using Base.Common.Threads;
using BLL;
using DbModel.Others;
using Location.BLL.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiLib;
using WebApiLib.Clients.OpcCliect;

namespace LocationServer.Threads
{
    /// <summary>
    /// 保存测点
    /// </summary>
    public class PointHistoryThread : IntervalTimerThread
    {
        public PointHistoryThread() : 
            base(
            TimeSpan.FromSeconds(10), 
            TimeSpan.FromSeconds(1)
                )
        {
        }

        public override bool TickFunction()
        {
          
            string tags = "NCS_34_AI67,NCS_34_AI71,2NCS_AI190,2NCS_AI198,2NCS_AI230,2NCS_AI238,2NCS_AI266,2NCS_AI274";
            try
            {
                DateTime nowTime = DateTime.Now;
                if (nowTime.Hour == 0) //当天的凌晨0点（60秒内）
                {
                    List<SisData> sisList = WebApiHelper.GetEntity<List<SisData>>("http://10.146.33.9:20080/MIS/GetRtMonTagInfosByNames?tagNames=" + tags);
                    List<PointHistory> pointList = new List<PointHistory>();
                    Bll db = Bll.NewBllNoRelation();
                    if (sisList != null && sisList.Count > 0)
                    {
                        foreach (SisData date in sisList)
                        {
                            PointHistory pointHis = new PointHistory();
                            pointHis.point = date.Name;
                            pointHis.value = date.Value;
                            pointHis.createTime = DateTime.Now;
                            pointList.Add(pointHis);
                        }

                      bool result =  db.PointHistories.AddRange(pointList);
                        Log.Info("PointHistoryThread: 保存测点数据,时间：" + DateTime.Now+",结果："+result);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("PointHistoryThread:"+ex.ToString());
            }
            return true;
        }

        protected override void DoBeforeWhile()
        {
            
        }
    }
}
