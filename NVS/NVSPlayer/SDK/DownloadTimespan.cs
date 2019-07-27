using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace NVSPlayer.SDK
{
    public class DownloadTimespan
    {
        public int m_iSize;				//Structure size
        public char[] m_cLocalFilename;	//Local video file name
        public int m_iChannelNO;
        public NVS_FILE_TIME m_tTimeBegin;			//Start time
        public NVS_FILE_TIME m_tTimeEnd;				//End time     
        public int m_iPosition;			//File location by percentage 0～100;When continue send after stop send,file pointer offset request 
        public int m_iSpeed;				//1，2，4，8，Control file play speed, 0-Suspend
        public int m_iIFrame;				//Only send I frame 1,Only play I Frame;0, All play					
        public int m_iReqMode;				//Require data mode 1,Frame mode;0,Stream mode					
        public int m_iVodTransEnable;		//Enable
        public int m_iVodTransVideoSize;	//Video pixel
        public int m_iVodTransFrameRate;   //Frame rate
        public int m_iVodTransStreamRate;  //Code rate
        public int m_iFileFlag;            //0:Download multiple files  1:Download into a single file
        public int m_iSaveFileType;
        public int m_iStreamNo;
        public int m_iFileAttr;			   //File attributes:0: nvr local storage; 10000: ipc storage
        public int m_iCryptType;			//iCryptType = 0, no encryption; iCryptType = 1, is AES encryption
        public char[] m_cCryptKey;

        public DOWNLOAD_TIMESPAN GetStructure()
        {
            DOWNLOAD_TIMESPAN tInfo = new DOWNLOAD_TIMESPAN();
            tInfo.m_iSize = Marshal.SizeOf(tInfo);
            tInfo.m_iChannelNO = m_iChannelNO;
            tInfo.m_iSaveFileType = m_iSaveFileType; //下载保存为PS(MP4)格式
            tInfo.m_iSpeed = m_iSpeed;
            tInfo.m_iPosition = m_iPosition;
            tInfo.m_iStreamNo = m_iStreamNo;
            tInfo.m_iFileFlag = m_iFileFlag; //0多个文件  1单个文件
            tInfo.m_iReqMode = m_iReqMode; //0流模式(设备不发下载时间进度,不支持跨文件), 1帧模式(设备发下载时间进度，支持跨文件)
            tInfo.m_tTimeBegin.m_iYear = m_tTimeBegin.m_iYear;
            tInfo.m_tTimeBegin.m_iMonth = m_tTimeBegin.m_iMonth;
            tInfo.m_tTimeBegin.m_iDay = m_tTimeBegin.m_iDay;
            tInfo.m_tTimeBegin.m_iHour = m_tTimeBegin.m_iHour;
            tInfo.m_tTimeBegin.m_iMinute = m_tTimeBegin.m_iMinute;
            tInfo.m_tTimeBegin.m_iSecond = m_tTimeBegin.m_iSecond;

            tInfo.m_tTimeEnd.m_iYear = m_tTimeEnd.m_iYear;
            tInfo.m_tTimeEnd.m_iMonth = m_tTimeEnd.m_iMonth;
            tInfo.m_tTimeEnd.m_iDay = m_tTimeEnd.m_iDay;
            tInfo.m_tTimeEnd.m_iHour = m_tTimeEnd.m_iHour;
            tInfo.m_tTimeEnd.m_iMinute = m_tTimeEnd.m_iMinute;
            tInfo.m_tTimeEnd.m_iSecond = m_tTimeEnd.m_iSecond;


            tInfo.m_cLocalFilename = m_cLocalFilename;

            tInfo.m_iVodTransEnable = m_iVodTransEnable;
            tInfo.m_iVodTransVideoSize = m_iVodTransVideoSize;
            tInfo.m_iVodTransFrameRate = m_iVodTransFrameRate;
            tInfo.m_iVodTransStreamRate = m_iVodTransStreamRate;
            tInfo.m_iFileAttr = m_iFileAttr;
            tInfo.m_cCryptKey = m_cCryptKey;
            return tInfo;
        }
    }
}
