using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArchorUDPTool.ArchorManagers
{
    class UDPScanWorker
    {
        ArchorManager manager;
        public UDPScanWorker(ArchorManager manager,string[] ips)
        {
            this.manager = manager;
            this.Ips = ips;
        }

        public string[] Ips;
        BackgroundWorker worker;
        public void Start()
        {
            worker = new BackgroundWorker();
            worker.WorkerSupportsCancellation = true;
            worker.WorkerReportsProgress = true;
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerAsync();
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            manager.OnPercentChanged(e.ProgressPercentage);
        }

        public void Stop()
        {
            worker.CancelAsync();
        }

        public int CmdSleepTime = 10;

        private bool IsCancel = false;

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            IsCancel = false;
            BackgroundWorker worker = sender as BackgroundWorker;

            for (int j = 0; j < Ips.Length; j++)
            {
                string ip = Ips[j];

                var localIp = manager.GetLocalIp(ip);

                if (localIp != null)
                {
                    var udp = manager.GetLightUDP(localIp);
                    IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), manager.archorPort);

                    foreach (var cmd in manager.Cmds)
                    {
                        //AddLog(string.Format("发送 :: [{0}]:{1}", ipEndPoint, cmd));
                        udp.SendHex(cmd, ipEndPoint);
                        //Thread.Sleep(CmdSleepTime);
                    }

                    if (j % 3 == 0)
                    {
                        Thread.Sleep(CmdSleepTime * manager.Cmds.Length);
                    }
                }

                double p1 = (j + 1.0) / Ips.Length;
                int percent = (int)(p1 * 100);
                worker.ReportProgress(percent, Ips.Length);

                if (IsCancel)
                {
                    return;
                }
            }
        }
    }
}
