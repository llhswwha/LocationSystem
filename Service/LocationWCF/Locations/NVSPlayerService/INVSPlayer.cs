using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LocationServices.Locations.Plugins
{
    [ServiceContract]
    public interface INVSPlayer
    {
        /// <summary>
        /// 停止下载
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [OperationContract]
        DownloadInfo StopGetNVSVideo(DownloadInfo info);

        /// <summary>
        /// 开始下载
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [OperationContract]
        DownloadInfo StartGetNVSVideo(DownloadInfo info);

        /// <summary>
        /// 获取下载进度
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [OperationContract]
        DownloadProgress GetNVSProgress(DownloadInfo info);
    }

    public class DownloadInfo
    {
        /// <summary>
        /// Dev_Camera的Id，为了获取下面的IP和通道
        /// </summary>
        public int CId { get; set; }

        /// <summary>
        /// 录像机IP
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 录像机通道
        /// </summary>
        public string Channel { get; set; }

        /// <summary>
        /// 开始时间 2019-07-19 13:07:45 "yyyy-MM-dd HH:mm:ss"
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// 结果
        /// </summary>
        public bool Result { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 下载Id
        /// </summary>
        public int DId { get; set; }

        /// <summary>
        /// hls地址(已经存在该文件的情况下)
        /// </summary>
        public string Url { get; set; }

        public override string ToString()
        {
            //return string.Format("cid:{0},start:{1},end:{2}", CId, StartTime, EndTime);
            string json = JsonConvert.SerializeObject(this);
            return json;
        }
    }

    public class DownloadProgress
    {
        /// <summary>
        /// 下载Id
        /// </summary>
        public int DId { get; set; }

        /// <summary>
        /// 进度
        /// </summary>
        public int Progress { get; set; }

        /// <summary>
        /// 进度内容
        /// </summary>
        public string ProgressText { get; set; }

        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsFinished { get; set; }

        /// <summary>
        ///  hls地址
        /// </summary>
        public string Url { get; set; }

        public override string ToString()
        {
            //return string.Format("cid:{0},Progress:{1},ProgressText:{2},IsFinished:{3},Url{4}", CId, Progress,
            //    ProgressText, IsFinished, Url);

            string json = JsonConvert.SerializeObject(this);
            return json;
        }
    }
}
