using Location.BLL.Tool;
using Location.TModel.Location.AreaAndDev;
using LocationServices.Converters;
using LocationServices.Locations.Plugins;
using LocationServices.Locations.Services;
using NVSPlayer;
using PlaybackDemo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NVSPlayer.SDK;
using TModel.Tools;
using System.Threading;
using System.Windows.Forms;

namespace LocationServices.Locations
{
    //岗位相关的接口
    public partial class LocationService : ILocationService, IDisposable
    {
        public DownloadInfo StopGetNVSVideo(DownloadInfo info)
        {
            try
            {
                Log.Info(LogTags.DbGet, "StopGetNVSVideo:" + info);
                NVSManage.GetPlayBackFormEx(info.Ip, (f, s) => { f.Stop(); });
                return info;
            }
            catch (Exception e)
            {
                Log.Error(LogTags.DbGet, "LocationService.StopGetNVSVideo:" + ToString());
                return null;
            }
        }

        public DownloadInfo StartGetNVSVideo(DownloadInfo info)
        {
            try
            {
                Log.Info(LogTags.DbGet, "StartGetNVSVideo:"+ info);
                if (info == null) return null;

                //客户端可以把IP（录像机的IP)和Channel传过来(不用查数据库)，也可以只传一个摄像机Id（要查数据库）
                if (string.IsNullOrEmpty(info.Ip) || string.IsNullOrEmpty(info.Channel)) 
                {
                    Log.Info(LogTags.DbGet, "GetIpAndChannelFormDb");

                    if (!GetIpAndChannelFormDb(info))//GetIpAndChannelFormDb里面会填充Ip和Channel信息，但是没有找到摄像头的话就返回了。
                    {
                        info.Result = false;
                        info.Message = "camera == null";
                        return info;
                    } 
                }

                Log.Info(LogTags.DbGet, "info:"+ info);

                var start = info.StartTime.ToDateTime();
                var end = info.EndTime.ToDateTime();
                var file = Downloader.GetFileName(info.Ip, info.Channel,1,start, end);//视频文件的名称（预订）
                Log.Info(LogTags.DbGet, string.Format("start:{0},end:{1},file:{2}",start,end,file));

                if (Downloader.IsFileExist(file))//文件已经存在
                {
                    string url = Downloader.GetHlsUrl(NVSManage.RTMP_Host, file);
                    info.Url = url;
                    info.Result = true;
                    info.Message = "ExistFile";
                    Log.Info(LogTags.DbGet, string.Format("ExistFile url:{0}", url));
                }
                else
                {
                    var waitForLogin = true;

                    NVSManage.GetPlayBackFormEx(info.Ip, (form1,loginState) =>
                    {
                        Log.Info(LogTags.DbGet, string.Format("loginState:{0}", loginState));
                        if (loginState)
                        {
                            var channel = info.Channel.ToInt();
                            var r = form1.Download(channel, start, end);//有文件正在下载的情况
                            info.Result = r;
                            if (r == false)
                            {
                                Log.Error(LogTags.DbGet, form1.Message);
                                info.Message = form1.Message;
                            }
                            else
                            {
                                info.DId = form1.downloader.m_iDLTimeId;//下载用的Id,查询下载进度用
                            }
                        }
                        else
                        {
                            info.Result = false;
                            info.Message = form1.Message;
                        }
                        waitForLogin = false;
                    });

                    for (int i = 0; i < 300; i++)//等待登录结果
                    {
                        if (waitForLogin == false) break;
                        Application.DoEvents();
                        Thread.Sleep(50);//等待登录结果
                    }
                }
                Log.Info(LogTags.DbGet, string.Format("Return:{0}", info));
                return info;
            }
            catch (Exception e)
            {
                Log.Error(LogTags.DbGet, "LocationService.StartGetNVSVideo:"+ToString());
                return null;
            }
        }

        //private bool waitForLogin;

        private bool GetIpAndChannelFormDb(DownloadInfo info)
        {
            var camera = db.Dev_CameraInfos.Find(info.CId); //摄像机Id
            if (camera == null)
            {
                Log.Error(LogTags.DbGet, "camera == null");
                return false;
            }

            var rtsp = camera.RtspUrl; //"rtsp://admin:1111@192.168.108.107/13"
            Log.Info(LogTags.DbGet, "rtsp:" + rtsp);
            string[] parts = rtsp.Split('@');
            string[] parts2 = parts[1].Split('/');

            string ip = parts2[0];
            info.Ip = ip;
            string channel = parts2[1];
            info.Channel = channel;
            return true;
        }

        public DownloadProgress GetNVSProgress(DownloadInfo info)
        {
            try
            {

                Log.Info(LogTags.DbGet, "GetNVSProgress:" + info);
                if (info == null) return null;

                DownloadProgress progress = new DownloadProgress();
                progress.DId = info.DId;

                PlayBackForm form1 = NVSManage.Form;
                var downloader = form1.downloader;
                if (downloader == null)
                {
                    progress.Progress = 0;
                    progress.ProgressText = "downloader == null";
                    progress.IsFinished = false;
                }
                else
                {
                    if (downloader.m_iDLTimeId!=-1 && downloader.m_iDLTimeId != info.DId)//当前下载的文件不是客户端申请的，多个客户端下载文件的话。
                    {
                        progress.Progress = 0;
                        progress.ProgressText = "downloader.m_iDLTimeId != info.DId";
                        progress.IsFinished = false;
                    }
                    else
                    {
                        progress.Progress = downloader.Progress;
                        progress.ProgressText = downloader.ProgressText;
                        progress.IsFinished = downloader.IsFinished;
                        if (progress.IsFinished)
                        {
                            progress.Url = downloader.GetUrl(NVSManage.RTMP_Host);
                            if (!progress.Url.StartsWith("http"))
                            {
                                Log.Error(LogTags.DbGet, progress.Url);
                            }
                            else
                            {
                                Log.Info(LogTags.DbGet, "GetNVSProgress Finished !!!!!!!!:" + progress);
                            }
                        }
                    }
                }
                Log.Info(LogTags.DbGet, "GetNVSProgress:" + progress);
                return progress;
            }
            catch (Exception e)
            {
                Log.Error("LocationService.GetNVSProgress", e.ToString());
                return null;
            }
        }
    }
}
