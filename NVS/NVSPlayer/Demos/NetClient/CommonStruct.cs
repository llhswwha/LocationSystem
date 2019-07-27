using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace NetClient
{
    [StructLayout(LayoutKind.Sequential)]
    struct DOWNLOAD_FILE
    {
        public Int32 m_iSize;			//结构体大小
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 255)]
        public char[] m_cRemoteFilename;   //前端录像文件名
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 255)]
        public char[] m_cLocalFilename;	  //本地录像文件名
        public Int32 m_iPosition;		//文件定位时,按百分比0～100;断点续传时，请求的文件指针偏移量
        public Int32 m_iSpeed;			//1，2，4，8，控制文件播放速度, 0-暂停
        public Int32 m_iIFrame;			//只发I帧 1,只播放I帧;0,全部播放					
        public Int32 m_iReqMode;			//需求数据的模式 1,帧模式;0,流模式					
        public Int32 m_iRemoteFileLen;	//	如果本地文件名不为空，此参数置为空
    };
    [StructLayout(LayoutKind.Sequential)]
    struct _MAIN_NOTIFY_DATA
    {
        public Int32 m_iLogonID;
        public Int32 m_wParam;
        public Int32 m_lParam;
        public Int32 m_iUserData;
    };

    [StructLayout(LayoutKind.Sequential)]
    struct DOWNLOAD_CONTROL
    {
        public Int32 m_iSize;			//结构体大小
        public Int32 m_iPosition;		//0～100，定位文件播放位置；-1，不进行定位
        public Int32 m_iSpeed;			//1，2，4，8，控制文件播放速度, 0-暂停
        public Int32 m_iIFrame;			//只发I帧 1,只播放I帧;0,全部播放
        public Int32 m_iReqMode;			//需求数据的模式 1,帧模式;0,流模式
    };

    [StructLayout(LayoutKind.Sequential)]
    struct S_header
    {
        public UInt16 FrameRate;
        public UInt16 Width;
        public UInt16 Height;
    };

    [StructLayout(LayoutKind.Sequential)]
    struct NVS_FILE_QUERY
    {
        public int m_iType;          /* Record type 1-Manual record, 2-Schedule record, 3-Alarm record*/
        public int m_iChannel;       /* Record channel 0~channel defined channel number*/
        public NVS_FILE_TIME m_struStartTime;  /* File start time */
        public NVS_FILE_TIME m_struStoptime;   /* File end time */
        public int m_iPageSize;      /* Record number returned by each research*/
        public int m_iPageNo;        /* From which page to research */
        public int m_iFiletype;      /* File type, 0-All, 1-AVstream, 2-picture*/
        public int m_iDevType;       /* 设备类型，0-摄像 1-网络视频服务器 2-网络摄像机 0xff-全部*/
    };
    [StructLayout(LayoutKind.Sequential)]
    public struct NVS_FILE_TIME
    {
        public UInt16 m_iYear;   /* Year */
        public UInt16 m_iMonth;  /* Month */
        public UInt16 m_iDay;    /* Day */
        public UInt16 m_iHour;   /* Hour */
        public UInt16 m_iMinute; /* Minute */
        public UInt16 m_iSecond; /* Second */
    }
    [StructLayout(LayoutKind.Sequential)]
    struct NVS_FILE_DATA
    {
        public int m_iType;          /* Record type 1-Manual record, 2-Schedule record, 3-Alarm record*/
        public int m_iChannel;       /* Record channel 0~channel defined channel number*/
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 250)]
        public Char[] m_cFileName; /* File name */
        public NVS_FILE_TIME m_struStartTime;  /* File start time */
        public NVS_FILE_TIME m_struStoptime;   /* File end time */
        public int m_iFileSize;      /* File size */
    }

    /// <summary>
    /// /////////////////////////////
    /// </summary>

    [StructLayout(LayoutKind.Sequential)]
    struct CONNECT_STATE
    {
        public int m_iLogonID;
        public int m_iChannelNO;
        public int m_iStreamNO;
        public UInt32 m_uiConID;
    };
    [StructLayout(LayoutKind.Sequential)]
    public struct CLIENTINFO
    {
        public int m_iServerID;        //NVS ID,NetClient_Logon 返回值
        public int m_iChannelNo;	    //Remote host to be connected video channel number (Begin from 0)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 255)]
        public Char[] m_cNetFile;    //Play the file on net, not used temporarily
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public Char[] m_cRemoteIP;	//IP address of remote host
        public int m_iNetMode;		    //Select net mode 1--TCP  2--UDP  3--Multicast
        public int m_iTimeout;		    //Timeout length for data receipt
        public int m_iTTL;			    //TTL value when Multicast
        public int m_iBufferCount;     //Buffer number
        public int m_iDelayNum;        //Start to call play progress after which buffer is filled
        public int m_iDelayTime;       //Delay time(second), reserve
        public int m_iStreamNO;        //Stream type
        public int m_iFlag;			//0，首次请求该录像文件；1，操作录像文件
        public int m_iPosition;		//0～100，定位文件播放位置；-1，不进行定位
        public int m_iSpeed;			//1，2，4，8，控制文件播放速度        
    };

    [StructLayout(LayoutKind.Sequential)]
    struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct NVS_IPAndID
    {
        public string m_pIP;
        public string m_pID;
        public UInt32 m_puiLogonID;
    };

    [StructLayout(LayoutKind.Sequential)]
    struct NVS_SCHEDTIME
    {
        public UInt16 m_ustStartHour;
        public UInt16 m_usStartMin;
        public UInt16 m_ustStopHour;
        public UInt16 m_ustStopMin;
        public UInt16 m_ustRecordMode;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct STR_VideoParam
    {
        public UInt16 m_ustBrightness;             //亮度
        public UInt16 m_usHue;                     //色度
        public UInt16 m_ustContrast;               //对比度
        public UInt16 m_ustSaturation;             //饱和度
        [MarshalAs(UnmanagedType.Struct)]
        public NVS_SCHEDTIME m_strctTempletTime;   //时间模板        
    }

    //Ctrl param
    [StructLayout(LayoutKind.Sequential)]
    struct CONTROL_PARAM
    {
        public Int32 m_iAddress;   //device address
        public Int32 m_iPreset;	   //preset pos
        [MarshalAs(UnmanagedType.Struct)]
        public POINT m_ptMove;     //move pos
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] m_btBuf;     //Ctrl-Code(OUT)
        public Int32 m_iCount;     //Ctrl-Code count(OUT)
    };

    [StructLayout(LayoutKind.Sequential)]
    struct POINT
    {
        public Int32 x;
        public Int32 y;
    };

    [StructLayout(LayoutKind.Sequential)]
    class Reserve
    {
        public Int32 m_iReserved1;
        public UInt32 m_ustReserved2;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] m_btReserved1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] m_btReserved2;
        public Reserve()
        {
            m_iReserved1 = new Int32();
            m_ustReserved2 = new UInt32();
            m_btReserved1 = new byte[32];
            m_btReserved2 = new byte[64];
        }
    };

    [StructLayout(LayoutKind.Sequential)]
    class NvsSingle
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] m_btNvsIP;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] m_btNvsName;
        public Int32 m_iNvsType;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] m_btFactoryID;
        [MarshalAs(UnmanagedType.Struct)]
        public Reserve m_stReserve;
        public NvsSingle()
        {
            m_btNvsIP = new byte[32];
            m_btNvsName = new byte[32];
            m_btFactoryID = new byte[32];
            m_stReserve = new Reserve();
        }

    };

    [StructLayout(LayoutKind.Sequential)]
    class DNSRegInfo
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] m_btUserName;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] m_btPwd;
        [MarshalAs(UnmanagedType.Struct)]
        public NvsSingle m_stNvs;
        public Int32 m_iPort;
        public Int32 m_iChannel;
        [MarshalAs(UnmanagedType.Struct)]
        public Reserve m_stReserve;
        public DNSRegInfo()
        {
            m_btUserName = new byte[32];
            m_btPwd = new byte[32];
            m_stNvs = new NvsSingle();
            m_stReserve = new Reserve();
        }
    };

    [StructLayout(LayoutKind.Sequential)]
    class REG_DNS
    {
        [MarshalAs(UnmanagedType.Struct)]
        public DNSRegInfo m_stDNSInfo;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] m_btRegTime;
        [MarshalAs(UnmanagedType.Struct)]
        public Reserve m_stReserve;
        public REG_DNS()
        {
            m_stDNSInfo = new DNSRegInfo();
            m_btRegTime = new byte[32];
            m_stReserve = new Reserve();
        }
    };

    [StructLayout(LayoutKind.Sequential)]
    class REG_NVS
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] m_btPrimaryDS;
        [MarshalAs(UnmanagedType.Struct)]
        public NvsSingle m_stNvs;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] m_btRegTime;
        public UInt32 m_uiClientConnNum;
        public Boolean m_blRegister;
        [MarshalAs(UnmanagedType.Struct)]
        public Reserve m_stReserve;
        public REG_NVS()
        {
            m_btPrimaryDS = new byte[32];
            m_stNvs = new NvsSingle();
            m_btRegTime = new byte[32];
            m_stReserve = new Reserve();
        }
    };

    struct FRAME_INFO
    {
        public UInt32 nWidth;    //Video width, audio data is 0；
        public UInt32 nHeight;   //Video height, audio data is 0；
        public UInt32 nStamp;    //Time stamp(ms)。
        public UInt32 nType;     //Audio type，T_AUDIO8,T_YUV420，。
        public UInt32 nFrameRate;//Frame rate。
        public UInt32 nReserved; //reserve
    };

    struct LogonInfo
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] cDSIP;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] cUserName;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] cUserPwd;
        [MarshalAs(UnmanagedType.Struct)]
        public NvsSingle stNvs;
        [MarshalAs(UnmanagedType.Struct)]
        public Reserve m_stReserve;
    };

    struct ProxyInfo
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] cProxyIP;
        public int iProxyPort;
        [MarshalAs(UnmanagedType.Struct)]
        public Reserve m_stReserve;
    };

    [StructLayout(LayoutKind.Sequential)]
    struct FTP_SNAPSHOT
    {
        public Int32 iChannel;			//	通道 
        public Int32 iEnable;			//	模式 0:不使能,1:使能(定时),2:(报警联动抓拍),3:报警联动多次抓拍注释,默认为2
        public Int32 iQValue;			//	图片质量 取值范围0-100
        public Int32 iInterval;			//	抓拍时间间隔 取值范围1-3600(秒)
        public Int32 iPictureSize;		//  抓拍图片大小	0x7fff：代表自动，其余对应分辨率大小
    };

    [StructLayout(LayoutKind.Sequential)]
    struct FTP_LINKSEND
    {
        public Int32 iChannel;			//	通道
        public Int32 iEnable;			//	使能
        public Int32 iMethod;			//	方式
    };

    [StructLayout(LayoutKind.Sequential)]
    struct STR_Para
    {

    };

    delegate void RECVDATA_NOTIFY(uint _ulID, string _strData, int _iLen);
    delegate void DNSList_NOTIFY(Int32 _iCount, IntPtr _pDns);
    delegate void NVSList_NOTIFY(Int32 _iCount, IntPtr _pNvs);
    delegate void COMRECV_NOTIFY(Int32 _iLogonID, IntPtr _pBuf, Int32 _iLen, Int32 _iComNO);
    delegate void DECYUV_NOTIFY(UInt32 _ulID, IntPtr _pData, Int32 _iLen, ref FRAME_INFO _pFrameInfo, IntPtr _pContext);

    //  SDK4.0回调接口委托V4接口为—__CDECL调用
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void MAIN_NOTIFY_V4(UInt32 _ulLogonID, IntPtr _iWparam, IntPtr _iLParam, Int32 _iUser);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void ALARM_NOTIFY_V4(Int32 _ulLogonID, Int32 _iChan, Int32 _iAlarmState, Int32 _iAlarmType, Int32 _iUser);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void PARACHANGE_NOTIFY_V4(Int32 _ulLogonID, Int32 _iChan, Int32 _iParaType, ref STR_Para _strPara, Int32 _iUser);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void COMRECV_NOTIFY_V4(Int32 _ulLogonID, IntPtr _cData, Int32 _iLen, Int32 _iComNo, Int32 _iUser);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void PROXY_NOTIFY(Int32 _ulLogonID, Int32 _iCmdKey, IntPtr _cData, Int32 _iLen, Int32 _iUser);


    public class SDKConstMsg
    {
        public const int WM_USER = 0x0400; //
        public const int WM_MAIN_MESSAGE = WM_USER + 1001;
        public const int MSG_PARACHG = WM_USER + 90910;
        public const int MSG_ALARM = WM_USER + 90911;
        public const int WCM_ERR_ORDER = 2;
        public const int WCM_ERR_DATANET = 3;
        public const int WCM_LOGON_NOTIFY = 7;
        public const int WCM_VIDEO_HEAD = 8;
        public const int WCM_VIDEO_DISCONNECT = 9;
        public const int WCM_RECORD_ERR = 13;

        

        public const int LOGON_SUCCESS = 0;
        public const int LOGON_ING = 1;
        public const int LOGON_RETRY = 2;
        public const int LOGON_DSMING = 3;
        public const int LOGON_FAILED = 4;
        public const int LOGON_TIMEOUT = 5;
        public const int NOT_LOGON = 6;
        public const int LOGON_DSMFAILED = 7;
        public const int LOGON_DSMTIMEOUT = 8;
        public const int PLAYER_PLAYING = 0x02;
        public const int USER_ERROR = 0x10000000;


        public const int WCM_QUERYFILE_FINISHED = 18;  
        public const int WCM_DWONLOAD_FINISHED = 19;
        public const int WCM_DWONLOAD_FAULT = 20;
    }

    public class AlarmConstMsgType
    {
        public const int ALARM_VDO_MOTION	=	0;  //移动侦测
        public const int ALARM_VDO_REC		=	1;
        public const int ALARM_VDO_LOST		=	2;    
        public const int ALARM_VDO_INPORT	=	3;  //报警输入
        public const int ALARM_VDO_OUTPORT	=	4;  //报警输出
        public const int ALARM_VDO_COVER 	=	5;  //视频遮挡
        public const int ALARM_VCA_INFO		=	6;  //智能分析报警
        public const int ALARM_AUDIO_LOST	=	7;  //音频丢失
        public const int ALARM_EXCEPTION    =   8;  
                                           
    }


    public class ActionControlMsg
    {
        public const int MOVE = 60;
        public const int MOVE_STOP = 61;
        public const int MOVE_UP = 1;
        public const int MOVE_DOWN = 2;
        public const int MOVE_LEFT = 3;
        public const int MOVE_RIGHT = 4;
        public const int MOVE_UP_LEFT = 6;
        public const int MOVE_UP_RIGHT = 5;
        public const int MOVE_DOWN_LEFT = 8;
        public const int MOVE_DOWN_RIGHT = 7;
        public const int ZOOM_BIG = 10;
        public const int ZOOM_SMALL = 11;
        public const int FOCUS_NEAR = 13;
        public const int FOCUS_FAR = 14;
        public const int IRIS_OPEN = 17;
        public const int IRIS_CLOSE = 18;
        public const int RAIN_ON = 19;
        public const int RAIN_OFF = 20;
        public const int LIGHT_ON = 21;
        public const int LIGHT_OFF = 22;
        public const int HOR_AUTO = 23;
        public const int HOR_AUTO_STOP = 24;
        public const int CALL_VIEW = 25;
        public const int SET_VIEW = 28;
        public const int POWER_ON = 29;
        public const int POWER_OFF = 30;
        public const int ZOOM_BIG_STOP = 32;
        public const int ZOOM_SMALL_STOP = 34;
        public const int FOCUS_FAR_STOP = 36;
        public const int FOCUS_NEAR_STOP = 38;
        public const int IRIS_OPEN_STOP = 40;
        public const int IRIS_CLOSE_STOP = 42;
    }


}
