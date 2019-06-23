using BLL;
using CommunicationClass.SihuiThermalPowerPlant.Models;
using DbModel.Location.AreaAndDev;
using DbModel.LocationHistory.AreaAndDev;
using Location.BLL.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebApiLib.Clients
{
    public class SisDataSaveClient
    {
        protected SisDataSaveClient()
        {

        }

        private static SisDataSaveClient _instance;

        public static SisDataSaveClient Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SisDataSaveClient();
                    _instance.StartSaveSisThread();
                }
                return _instance;
            }
        }

        public static void Stop()
        {
            if (_instance != null)
            {
                _instance.StopSaveSisThread();
                _instance = null;
            }
        }

        public static long nEightHourSecond = 0;

        private Bll bll = Bll.Instance();

        Thread saveSisThread;

        public void Save(List<sis> sisList)
        {
            if (sisList == null) return;
            ThreadPool.QueueUserWorkItem(state =>
            {
                lock (sisWaitToSave)
                {
                    sisWaitToSave.AddRange(sisList);
                }
            });
        }

        private void StartSaveSisThread()
        {
            if (saveSisThread == null)
            {
                saveSisThread = new Thread(SaveSisToDb);
                saveSisThread.IsBackground = true;
                saveSisThread.Start();
            }
        }

        private void StopSaveSisThread()
        {
            if (saveSisThread != null)
            {
                saveSisThread.Abort();
            }
        }

        private List<sis> sisWaitToSave = new List<sis>();

        private void SaveSisToDb()
        {
            while (true)
            {
                List<sis> tmp = new List<sis>();
                lock (sisWaitToSave)
                {
                    tmp.AddRange(sisWaitToSave);
                    sisWaitToSave.Clear();
                }
                SaveSisToDb(tmp, true, null);
                Thread.Sleep(5000);//5s
            }
        }

        private void SaveSisToDb(List<sis> sisList, bool isSaveToHistory, Action<List<DevMonitorNode>> callback)
        {
            List<DevMonitorNode> monitorNodes = new List<DevMonitorNode>();
            try
            {
                foreach (sis item in sisList)
                {
                    string strTag = item.kks;
                    //DevMonitorNode Dmn = bll.DevMonitorNodes.DbSet.Where(p => p.KKS == item.kks).FirstOrDefault();
                    DevMonitorNode Dmn = bll.DevMonitorNodes.DbSet.Where(p => p.TagName == strTag).FirstOrDefault();
                    if (Dmn == null)
                    {
                        continue;
                    }
                    Dmn.Value = item.value;
                    Dmn.Time = item.t + nEightHourSecond;
                    bll.DevMonitorNodes.Edit(Dmn);//修改数据库数据
                    monitorNodes.Add(Dmn);
                }

                if (isSaveToHistory)
                {
                    foreach (var mn in monitorNodes)
                    {
                        DevMonitorNodeHistory Dmnh = mn.ToHistory();
                        bll.DevMonitorNodeHistorys.Add(Dmnh);
                    }
                }
            }
            catch (Exception ex)
            {
                string messgae = ex.Message;
                Log.Error(LogTags.KKS,"SaveSisToDb:" + ex);
            }
            if (callback != null)
            {
                callback(monitorNodes);
            }
        }
    }
}
