using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace NVSPlayer.SDK
{
    public class Downloader
    {
        private int m_iLogonId;

        public Downloader(int logonId)
        {
            m_iLogonId = logonId;
        }

        public DownloadTimespan tInfo;

        private DateTime start;
        private DateTime end;

        private int saveFileType;

        private string id;

        private string ip;

        private int channelNo;

        private int streamNo;

        public void Init(string ip, int channelNo, int streamNo, DateTime start, DateTime end,  int fileFlag, int reqMode, int saveFileType)
        {
            id = ip + "_" + channelNo + "_" + streamNo + "_" + GetTimeSpaneText(start, end);

            this.ip = ip;
            this.channelNo = channelNo;
            this.streamNo = streamNo;
            this.start = start;
            this.end = end;
            this.saveFileType = saveFileType;

            tInfo = StructFactory.CreateDownLoadTimeSpan(start, end,channelNo, streamNo,
                fileFlag, reqMode, saveFileType);

            GetFileName();
        }

        private string strFileName;

        public string GetFileName()
        {
            strFileName = GetFileName(ip, (channelNo+1), (streamNo+1), start, end, saveFileType);
            int iSaveFormat = saveFileType;
            if (0 == iSaveFormat) //sdv格式
            {
                tInfo.m_iSaveFileType = NetSDKCmd.DOWNLOAD_FILE_TYPE_SDV;
            }
            else if (1 == iSaveFormat) //PS(MP4格式)
            {
                tInfo.m_iSaveFileType = NetSDKCmd.DOWNLOAD_FILE_TYPE_PS;
            }

            tInfo.m_cLocalFilename = new char[255];
            Array.Copy(strFileName.ToCharArray(), tInfo.m_cLocalFilename, strFileName.Length);
            return strFileName;
        }

        public static string GetFileName(string ip, object channelNo, object streamNo, DateTime start, DateTime end, int iSaveFormat=1)
        {
            string strFileName = "DV_"+ip + "_" + channelNo + "_" + streamNo + "_" + GetTimeSpaneText(start, end);
            if (0 == iSaveFormat)       //sdv格式
            {
                strFileName += ".sdv";
            }
            else if (1 == iSaveFormat)  //PS(MP4格式)
            {
                //strFileName += ".ps";
                strFileName += ".mp4";
            }
            return strFileName;
        }

        public static string GetTimeSpaneText(DateTime start, DateTime end)
        {
            string txt = "";
            txt += start.Year.ToString() + start.Month.ToString("D2") + start.Day.ToString("D2") +
                   start.Hour.ToString("D2") + start.Minute.ToString("D2") + start.Second.ToString("D2");
            txt += "-";
            txt += end.Year.ToString() + end.Month.ToString("D2") + end.Day.ToString("D2") +
                   end.Hour.ToString("D2") + end.Minute.ToString("D2") + end.Second.ToString("D2");
            return txt;
        }

        public string GetUrl(string rtmpIp)
        {
            try
            {
                if (!IsFileExist(strFileName))//文件已经存在的话就不用移动
                {
                    string file1 = AppDomain.CurrentDomain.BaseDirectory + strFileName;
                    if (!File.Exists(file1))
                    {
                        return "文件不存在:" + file1;
                    }
                    string file2 = hlsVideoPath + strFileName;
                    FileInfo fileInfo2 = new FileInfo(file2);
                    if (!fileInfo2.Directory.Exists)
                    {
                        fileInfo2.Directory.Create();
                    }
                    //Thread.Sleep(100);
                    File.Move(file1, file2);
                    //File.Copy(file1, file2, true);
                    //Thread.Sleep(100);
                }
                return GetHlsUrl(rtmpIp, strFileName);
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public static string hlsVideoPath = AppDomain.CurrentDomain.BaseDirectory +
                                            "\\nginx-1.7.11.3-Gryphon\\temp\\hls_temp\\download\\";

        public static bool IsFileExist(string fileName)
        {
            DirectoryInfo dir = new DirectoryInfo(hlsVideoPath);
            if (dir.Exists == false)
            {
                dir.Create();
            }
            var files=dir.GetFiles();
            foreach (FileInfo file in files)
            {
                if (file.Name == fileName)
                {
                    return true;
                }
            }
            return false;
        }

        public static string GetHlsUrl(string rtmpIp, string fileName)
        {
            return string.Format("http://{0}:9099/live/download/{1}", rtmpIp, fileName);
        }

        public int m_iDLTimeId;

        public string State;

        public bool IsFinished = false;

        public long m_iDLStartTime;
        public long m_iDLStopTime;
        public int iRet;

        public bool Start()
        {
            var structureInfo = tInfo.GetStructure();

            IntPtr intptr = Marshal.AllocCoTaskMem(structureInfo.m_iSize);
            Marshal.StructureToPtr(structureInfo, intptr, true);
            iRet = NVSSDK.NetClient_NetFileDownload(ref m_iDLTimeId, m_iLogonId, NetSDKCmd.DOWNLOAD_CMD_TIMESPAN, intptr, structureInfo.m_iSize);
            Marshal.FreeHGlobal(intptr);
            if (iRet < 0)
            {
                MessageBox.Show("Download file failed, ret=!" + iRet);
                return false;
            }
            m_iDLStartTime = CommonFunc.NvsFileTimeToAbsSeconds(structureInfo.m_tTimeBegin);
            m_iDLStopTime = CommonFunc.NvsFileTimeToAbsSeconds(structureInfo.m_tTimeEnd);
            //timerDLTimePos.Enabled = true;    //开启定时器获取下载进度
            return true;
        }

        public Int32 Progress;

        public string ProgressText;

        public bool GetProgress()
        {
            if (m_iDLTimeId >= 0)       //获取按时间段下载进度
            {
                Int32 iPos = 0, iSize = 0;
                NVSSDK.NetClient_NetFileGetDownloadPos(m_iDLTimeId, ref iSize, ref iPos);
                Int32 iTotal = (Int32)(m_iDLStopTime - m_iDLStartTime);
                Int32 iSetPos = (iPos - (Int32)m_iDLStartTime) * 100 / iTotal;
                if (iSetPos > 0 && iSetPos <= 100)
                {
                    Progress = iSetPos;
                    ProgressText = CommonFunc.AbsSecondsToStr(iPos);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        internal bool Stop()
        {
            if (m_iDLTimeId >= 0)
            {
                NVSSDK.NetClient_NetFileStopDownloadFile(m_iDLTimeId);
                m_iDLTimeId = -1;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
