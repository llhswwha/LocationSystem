using Base.Common.Threads;
using BLL;
using Location.BLL.Tool;
using Location.TModel.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationServer.Threads
{
    /// <summary>
    /// 清除历史数据
    /// </summary>
    public class DeleteHistoryDataThread : IntervalTimerThread
    {
        public DeleteHistoryDataThread()  :base(
                 TimeSpan.FromHours(1)
                 , TimeSpan.FromSeconds(1))
        {
        }

        public override bool TickFunction()
        {
            try
            {
                DateTime now = DateTime.Now;
                if (now.Hour == 1)//凌晨1点至两点执行一次
                {
                    List<DeleteTable> delList = new List<DeleteTable>();
                    //需要清除数据在此添加
                    delList.Add(new DeleteTable(6, "OperateTime","DateTime", "deventranceguardcardactions"));  //门禁操作历史
                    delList.Add(new DeleteTable(6, "time","long", "devmonitornodehistories"));
                    delList.Add(new DeleteTable(6, "dtCreateTime", "DateTime", "inspectiontrackhistories"));
                    delList.Add(new DeleteTable(12, "AlarmTime", "DateTime", "locationalarmhistories"));
                    delList.Add(new DeleteTable(6, "createTime", "DateTime", "operationtickethistoryshes"));
                    delList.Add(new DeleteTable(6, "createTime", "DateTime", "worktickethistoryshes"));
                    delList.Add(new DeleteTable(6, "DateTimeStamp", "long", "positions"));

                    Bll db = Bll.NewBllNoRelation();
                    foreach (DeleteTable del in delList)
                    {
                        DateTime datetime = now.AddMonths(-del.time);
                        string sql = "";
                        if (del.type == "long")
                        {
                            long time = TimeConvert.ToStamp(datetime);  //这里转完，是13位的数据
                            switch (del.tableName)
                            {
                                case "devmonitornodehistories":  
                                    time = time / 1000; //数据库这张表，时间戳是10位的
                                    break;
                            }
                            sql = string.Format("delete from {0} where {1}<'{2}';", del.tableName, del.column,time);
                        }
                        else if (del.type == "DateTime")
                        {
                            sql = string.Format("delete from {0} where {1}<'{2}';", del.tableName, del.column, datetime);
                        }                       
                        string result = db.WorkTicketHistorySHes.AddorEditBySql(sql);
                        Log.Info(string.Format(@"清除表：{0}  {1}个月前数据,结果：{2}",del.tableName,del.time,result));
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("DeleteHistoryDataThread:"+ex.ToString());
            }
            return true;
        }

        protected override void DoBeforeWhile()
        {
            throw new NotImplementedException();
        }


    }


    public class DeleteTable
    {
        /// <summary>
        /// 删除时间（该时间表示删除多少个月前的数据） 比如：time=6  表示6个月前的数据
        /// </summary>
        public int time { get; set; }
        /// <summary>
        /// 条件字段
        /// </summary>
        public string column { get; set; }
        /// <summary>
        /// 字段类型
        /// </summary>
        public string type { get; set; }
        public string tableName { get; set; }

        public DeleteTable(int time,string column, string type,string tableName)
        {
            this.time = time;
            this.column = column;
            this.type = type;
            this.tableName = tableName;
        }

    }

}
