using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace NVSPlayer.SDK
{
    public static class StructFactory
    {
        public static DOWNLOAD_TIMESPAN CreateDOWNLOAD_TIMESPAN(int channelNo,int streamNo, int fileFlag,int reqMode,DateTime start,DateTime end)
        {
            DOWNLOAD_TIMESPAN tInfo = new DOWNLOAD_TIMESPAN();
            tInfo.m_iSize = Marshal.SizeOf(tInfo);
            tInfo.m_iChannelNO = channelNo;
            tInfo.m_iSaveFileType = 1;      //下载保存为PS(MP4)格式
            tInfo.m_iSpeed = 32;
            tInfo.m_iPosition = -1;
            tInfo.m_iStreamNo = streamNo;
            tInfo.m_iFileFlag = fileFlag;//0多个文件  1单个文件
            tInfo.m_iReqMode = reqMode;//0流模式(设备不发下载时间进度,不支持跨文件), 1帧模式(设备发下载时间进度，支持跨文件)
            tInfo.m_tTimeBegin.m_iYear = Convert.ToUInt16(start.Year);
            tInfo.m_tTimeBegin.m_iMonth = Convert.ToUInt16(start.Month);
            tInfo.m_tTimeBegin.m_iDay = Convert.ToUInt16(start.Day);
            tInfo.m_tTimeBegin.m_iHour = Convert.ToUInt16(start.Hour);
            tInfo.m_tTimeBegin.m_iMinute = Convert.ToUInt16(start.Minute);
            tInfo.m_tTimeBegin.m_iSecond = Convert.ToUInt16(start.Second);

            tInfo.m_tTimeEnd.m_iYear = Convert.ToUInt16(end.Year);
            tInfo.m_tTimeEnd.m_iMonth = Convert.ToUInt16(end.Month);
            tInfo.m_tTimeEnd.m_iDay = Convert.ToUInt16(end.Day);
            tInfo.m_tTimeEnd.m_iHour = Convert.ToUInt16(end.Hour);
            tInfo.m_tTimeEnd.m_iMinute = Convert.ToUInt16(end.Minute);
            tInfo.m_tTimeEnd.m_iSecond = Convert.ToUInt16(end.Second);
            return tInfo;
        }

        public static DownloadTimespan CreateDownLoadTimeSpan(DateTime start, DateTime end,int channelNo, int streamNo, int fileFlag, int reqMode,int saveFileType)
        {
            DownloadTimespan tInfo = new DownloadTimespan();
            //tInfo.m_iSize = Marshal.SizeOf(tInfo);
            tInfo.m_iChannelNO = channelNo;
            tInfo.m_iSaveFileType = saveFileType;      //0:sdv格式,  1:下载保存为PS(MP4)格式
            tInfo.m_iSpeed = 32;
            tInfo.m_iPosition = -1;
            tInfo.m_iStreamNo = streamNo;
            tInfo.m_iFileFlag = fileFlag;//0多个文件  1单个文件
            tInfo.m_iReqMode = reqMode;//0流模式(设备不发下载时间进度,不支持跨文件), 1帧模式(设备发下载时间进度，支持跨文件)
            tInfo.m_tTimeBegin.m_iYear = Convert.ToUInt16(start.Year);
            tInfo.m_tTimeBegin.m_iMonth = Convert.ToUInt16(start.Month);
            tInfo.m_tTimeBegin.m_iDay = Convert.ToUInt16(start.Day);
            tInfo.m_tTimeBegin.m_iHour = Convert.ToUInt16(start.Hour);
            tInfo.m_tTimeBegin.m_iMinute = Convert.ToUInt16(start.Minute);
            tInfo.m_tTimeBegin.m_iSecond = Convert.ToUInt16(start.Second);

            tInfo.m_tTimeEnd.m_iYear = Convert.ToUInt16(end.Year);
            tInfo.m_tTimeEnd.m_iMonth = Convert.ToUInt16(end.Month);
            tInfo.m_tTimeEnd.m_iDay = Convert.ToUInt16(end.Day);
            tInfo.m_tTimeEnd.m_iHour = Convert.ToUInt16(end.Hour);
            tInfo.m_tTimeEnd.m_iMinute = Convert.ToUInt16(end.Minute);
            tInfo.m_tTimeEnd.m_iSecond = Convert.ToUInt16(end.Second);
            return tInfo;
        }
    }
}
