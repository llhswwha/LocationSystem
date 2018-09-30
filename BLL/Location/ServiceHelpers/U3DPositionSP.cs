using Location.BLL;
using Location.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace LocationWCFService.ServiceHelper
{
    /// <summary>
    /// U3D保存历史位置信息服务帮助类
    /// </summary>
    public class U3DPositionSP
    {
        private LocationBll db = new LocationBll(false, false,false);

        /// <summary>
        /// U3D位置信息列表
        /// </summary>
        public List<U3DPosition> U3DPositions;

        public U3DPositionSP()
        {
            U3DPositions = new List<U3DPosition>();
            StartInsertPositionTimer();
        }

        /// <summary>
        /// 添加位置信息列表
        /// </summary>
        /// <param name="list"></param>
        public void AddU3DPositions(List<U3DPosition> list)
        {
            U3DPositions.AddRange(list);
            //InsertPostions(posList);//在这里插入不会越来越慢，但是还是想分开。
            //InsertPostionsAsync(posList);
        }

        //private DispatcherTimer timerInsertPosition;
        //Stopwatch insertWatch = new Stopwatch();
        private Thread insertThread;

        /// <summary>
        /// 添加位置信息计时器
        /// </summary>
        public void StartInsertPositionTimer()
        {
            //if (timerInsertPosition == null)
            //{
            //    timerInsertPosition = new DispatcherTimer();
            //    timerInsertPosition.Interval = TimeSpan.FromMilliseconds(250);
            //    timerInsertPosition.Tick += (sender, e) =>
            //    {
            //        ////InsertPostions(); //同步
            //        ////InsertPostionsAsync();//异步，出异常时改成同步来调试
            //        ////这个异步会越来越慢 为什么呢？
            //        //TbTimer.Text = insertWatch.Elapsed.ToString();
            //    };
            //}
            //insertWatch.Start();

            //timerInsertPosition.Start();


            insertThread = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(100);
                    InsertPostions();
                }
            });
            insertThread.Start();
        }

        public void Stop()
        {
            if (insertThread != null)
            {
                insertThread.Abort();
            }
        }

        public bool isBusy = false; //没有这个标志位的话，很容易导致子线程间干扰

        private void InsertPostions()
        {
            lock (U3DPositions)
            {
                if (!isBusy && U3DPositions.Count > 0)
                {
                    isBusy = true;
                    //WriteLogRight(GetLogText(string.Format("写入{0}条数据 Start", Positions.Count)));

                    List<U3DPosition> posList2 = new List<U3DPosition>();
                    posList2.AddRange(U3DPositions);
                    if (InsertPostions(posList2))
                    {
                        U3DPositions.Clear();
                    }

                    isBusy = false;
                }
                else
                {
                    //if (U3DPositions.Count > 0)
                    //WriteLogRight(GetLogText(string.Format("等待 当前{0}条数据", Positions.Count)));
                }
            }
        }

        private bool InsertPostions(List<U3DPosition> posList2)
        {
            bool r = false;
            Stopwatch watch1 = new Stopwatch();
            watch1.Start();

            //using (LocationBll positionBll = new LocationBll())
            //{
            //    r = positionBll.AddPositions(posList2);
            //}

            r = AddPositions(posList2);

            watch1.Stop();
            //WriteLogRight(GetLogText(string.Format("写入{0}条数据 End 用时:{1}", posList2.Count, watch1.Elapsed)));
            ShowLog(watch1.Elapsed.ToString());
            return r;
        }

        public bool AddPositions(List<U3DPosition> u3dpositions)
        {
            bool r = true;
            try
            {
                string sql = GetInsertSql(u3dpositions);
                if (!string.IsNullOrEmpty(sql))
                    db.DbHistory.Database.ExecuteSqlCommand(sql);
                //批量插入历史数据数据
                //db.DbHistory.BulkInsert(u3dpositions);//插件Z.EntityFramework.Extensions功能
                //获取并修改列表
                //List<TagPosition> tagPosList = EditTagPositionList(u3dpositions);
                ////3.更新列表
                //TagPositions.Db.BulkUpdate(tagPosList);//插件Z.EntityFramework.Extensions功能
            }
            catch (Exception ex)
            {
                r = false;
            }
            return r;
        }

        public static Action<string> ShowLog_Action;


        private void ShowLog(string txt)
        {
            if (ShowLog_Action != null)
            {
                ShowLog_Action(txt);
            }
        }

        private string GetInsertSql(List<U3DPosition> u3dpositions)
        {
            string sql = "";
            foreach (U3DPosition p in u3dpositions)
            {
                sql +=
                    string.Format(
                        " INSERT INTO [U3DPosition]([Tag],[X],[Y],[Z],[Time],[Power],[Number],[Flag])VALUES( '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}') ",
                        p.Tag, p.X, p.Y, p.Z, p.Time, p.Power, p.Number, p.Flag);

            }
            return sql;
        }
    }
}
