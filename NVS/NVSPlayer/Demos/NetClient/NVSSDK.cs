using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace NetClient
{
    class NVSSDK
    {       
        public const int MAX_PAGESIZE    =20;
        
        public const int TYPE_NVS        = 0;       //nvs
        public const int TYPE_PROXY      = 1;       //代理服务器
        public const int TYPE_CLIENT     = 2;       //待连接的客户端
        public const int TYPE_TRANSFER   = 3;       //视频转发关系
        public const int TYPE_ASSIGN     = 4;       //代理分配
        public const int TYPE_DNS        = 5;       //域名解析
        public const int TYPE_DS         = 6;       //二级注册中心
        public const int TYPE_P2P_NVS    = 7;       //p2p nvs
        public const int TPYE_P2P_CLIENT = 8;       //使用P2P连接方式的客户端

        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_NetFileQuery(Int32 _iServerPort, ref NVS_FILE_QUERY _FileQueryt);
        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_NetFileGetFileCount(Int32 _iLogonID, ref Int32 iTotalCount,ref Int32 _iCurrentCount);
        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_NetFileGetQueryfile(Int32 _iLogonID,Int32 _Index, ref NVS_FILE_DATA _filedata);
        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_NetFileDownloadFile(ref UInt32 _uiConID, Int32 _iLogonID, 
                                                    string _cRemoteFilename, 
                                                    string _cLocalFilename,
                                                    Int32 _iFlag,
                                                    Int32 _iPosition,
                                                    Int32 _iSpeed);
        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_SetNetFileDownloadFileCallBack(UInt32 _uiConID,ReplayCallBackDelegate _CallBack,IntPtr _UserDate);
        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_NetFileStopDownloadFile(UInt32 _uiConID);

        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_NetFileGetDownloadPos(UInt32 _uiConID, ref Int32 _iPos,ref Int32 _dlSize);

        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_NetFileDownload(ref UInt32 uiConID, Int32 _iLogonID, Int32 _iCmd, [MarshalAs(UnmanagedType.LPArray)]byte[] _lpBuf, Int32 _iBufSize);

        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_GetAlarmPortNum(Int32 _iLogonID, ref Int32 _iAlarmChannelNo, ref Int32 _iAlarmOutPortNum);

        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_NetFileDownloadByTimeSpanEx(ref UInt32 _uiConID, 
                                                                         Int32 _iLogonID,
                                                                         string _cLocalFilename,
                                                                         Int32 _iChannelNO,
                                                                         ref NVS_FILE_TIME _uiFromSecond,
                                                                         ref NVS_FILE_TIME _uiToSecond,
                                                                         Int32 _iFlag,
                                                                         Int32 _iPosition,
                                                                         Int32 _iSpeed);


        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_SetPort(Int32 _iServerPort, Int32 _iClientPort);
        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_SetMSGHandle(UInt32 _uiMessage, IntPtr _hWnd, UInt32 _uiParaMsg, UInt32 _uiAlarmMsg);
        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_Startup();
        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_Cleanup();
        [DllImport("NVSSDK.dll", SetLastError = true)]
        public static extern Int32 NetClient_SetNotifyFunction_V4(MAIN_NOTIFY_V4 _MainNotify,
                                          ALARM_NOTIFY_V4 _AlarmNotify,
                                          PARACHANGE_NOTIFY_V4 _ParaNotify,
                                          COMRECV_NOTIFY_V4 _ComNotify,
                                          PROXY_NOTIFY _ProxyNotify);
        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_Logon(String _cProxy, String _cIP, String _cUserName, String _cPassword, String _pcProID, Int32 _iPort);
        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_Logoff(Int32 _iLogonID);
        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_StartRecv(ref UInt32 _uiConID, ref CLIENTINFO _cltInfo, RECVDATA_NOTIFY _cbkDataArrive);
        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_StopRecv(UInt32 _uiConID);
        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_StartCaptureData(UInt32 _uiConID);
        [DllImport("NVSSDK.dll", EntryPoint = "NetClient_StopCaptureDate")]
        public static extern Int32 NetClient_StopCaptureData(UInt32 _uiConID);
        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_StartPlay(UInt32 _uiConID, IntPtr _hWnd, RECT _rcShow, UInt32 _iDecflag);
        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_StopPlay(UInt32 _uiConID);
        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_GetPlayingStatus(UInt32 _uiConID);
        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_StartCaptureFile(UInt32 _uiConID, string _strFileName, Int32 _iRecFileType);
        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_StopCaptureFile(UInt32 _uiConID);
        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_CaptureBmpPic(UInt32 _uiConID, string _strFileName);
        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_GetOsdText(Int32 _iLogonID, Int32 _iChannelNum,byte[] _btOSDText, ref UInt32 _ulTextColor);
        [DllImport("NVSSDK.dll", SetLastError = true)]
        public static extern Int32 NetClient_SetOsdText(Int32 _iLogonID, Int32 _iChannelNum, byte[] _btOSDText,UInt32 _ulTextColor);
        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_GetOsdType(Int32 _iLogonID, Int32 _iChannelNum, Int32 _iPositionX,ref Int32 _iPositionY,ref Int32 _iOSDType,ref Int32 _iEnabled);
        [DllImport("NVSSDK.dll",SetLastError = true)]
        public static extern Int32 NetClient_SetOsdType(Int32 _iLogonID, Int32 _iChannelNum, Int32 _iPositionX, Int32 _iPositionY, Int32 _iOSDType, Int32 _iEnabled);
        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_GetComPortCounts(Int32 _iLogonID, ref Int32 _iComPortCounts, ref Int32 _iComPortEnabledStatus);
        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_GetDeviceType(Int32 _iLogonID, Int32 _iChannelNum, ref Int32 _iComNo, ref Int32 _iDevAddress, StringBuilder _strDevType);
        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_SetDeviceType(Int32 _iLogonID, Int32 _iChannelNum, Int32 _iComNo, Int32 _iDevAddress, byte[] _btDevType);
        [DllImport("NVSSDK.dll", SetLastError = true)]
        public static extern Int32 NetClient_GetComFormat(Int32 _iLogonID, Int32 _iComNo, StringBuilder _strComFormat, ref Int32 _iWorkMode);
        [DllImport("NVSSDK.dll", SetLastError = true)]
        public static extern Int32 NetClient_SetComFormat(Int32 _iLogonID, Int32 _iComNo,byte[] _btDeviceType,byte[] _btComFormat, Int32 _iWorkMode);
        [DllImport("NVSSDK.dll", EntryPoint = "NetClient_GetVideoPara")]
        public static extern Int32 NetClient_GetVideoParam(Int32 _iLogonID, Int32 _iChannelNum, ref STR_VideoParam _structVideoParam);
        [DllImport("NVSSDK.dll", EntryPoint = "NetClient_SetVideoPara", SetLastError = true)]
        public static extern Int32 NetClient_SetVideoParam(Int32 _iLogonID, Int32 _iChannelNum, ref STR_VideoParam _structVideoParam);
        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_ResetPlayerWnd(UInt32 _uiConID, IntPtr _hwnd);
        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_GetChannelNum(Int32 _iLogonID,ref Int32 _iChannelNum);
        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_GetCaptureStatus(UInt32 _uiConID);
        [DllImport("NVSSDK.dll", SetLastError = true)]
        public static extern Int32 NetClient_DeviceCtrlEx(Int32 _iLogonID, Int32 _iChannelNum, Int32 _iActionType, Int32 _iParam1, Int32 _iParam2, Int32 _iControlType);
        [DllImport("NVSSDK.dll", SetLastError = true)]
        public static extern Int32 NetClient_ComSend(Int32 _iLogonID, byte[] _btBuf, Int32 _iLength, Int32 _iComNo);
        [DllImport("DeviceDll/DOME_PELCO_D.dll", EntryPoint = "GetControlCode", SetLastError = true)]
        private static extern Int32 GetControlCode_DOME_PELCO_D(Int32 _iAction,ref CONTROL_PARAM _cParam);
        [DllImport("DeviceDll/DOME_PELCO_P.dll", EntryPoint = "GetControlCode", SetLastError = true)]
        private static extern Int32 GetControlCode_DOME_PELCO_P(Int32 _iAction, ref CONTROL_PARAM _cParam);
        [DllImport("DeviceDll/DOME_TIANDY.dll", EntryPoint = "GetControlCode", SetLastError = true)]
        private static extern Int32 GetControlCode_DOME_TIANDY(Int32 _iAction, ref CONTROL_PARAM _cParam);
        [DllImport("DeviceDll/PTZ_PELCO_D.dll", EntryPoint = "GetControlCode", SetLastError = true)]
        private static extern Int32 GetControlCode_PTZ_PELCO_D(Int32 _iAction, ref CONTROL_PARAM _cParam);
        [DllImport("DeviceDll/PTZ_PELCO_P.dll", EntryPoint = "GetControlCode", SetLastError = true)]
        private static extern Int32 GetControlCode_PTZ_PELCO_P(Int32 _iAction, ref CONTROL_PARAM _cParam);
        [DllImport("DeviceDll/PTZ_TC615_P.dll", EntryPoint = "GetControlCode", SetLastError = true)]
        private static extern Int32 GetControlCode_PTZ_TC615_P(Int32 _iAction, ref CONTROL_PARAM _cParam);
        public static Int32 NetClient_GetControlCode(string strDevType,Int32 _iAction, ref CONTROL_PARAM _cParam)
        {
            if (strDevType == "DOME_PELCO_D")
            {
                return GetControlCode_DOME_PELCO_D(_iAction, ref _cParam);
            }
            if (strDevType == "DOME_PELCO_P")
            {
                return GetControlCode_DOME_PELCO_P(_iAction, ref _cParam);
            }
            if (strDevType == "DOME_TIANDY")
            {
                return GetControlCode_DOME_TIANDY(_iAction, ref _cParam);
            }
            if (strDevType == "PTZ_PELCO_D")
            {
                return GetControlCode_PTZ_PELCO_D(_iAction, ref _cParam);
            }
            if (strDevType == "PTZ_PELCO_P")
            {
                return GetControlCode_PTZ_PELCO_P(_iAction, ref _cParam);
            }
            if (strDevType == "PTZ_TC615_P")
            {
                return GetControlCode_PTZ_TC615_P(_iAction, ref _cParam);
            }
            return -1;
        }

        
        
        
        //NSLook
        [DllImport("nslook.dll")]
        public static extern Int32 NSLook_Startup();
        [DllImport("nslook.dll")]
        public static extern Int32 NSLook_Cleanup();
        [DllImport("nslook.dll")]
        public static extern Int32 NSLook_LogonServer(byte[] _btServer, Int32 _iServerPort, Boolean _blRepeat);
        [DllImport("nslook.dll")]
        public static extern Int32 NSLook_LogoffServer(Int32 _iID);
        [DllImport("nslook.dll",SetLastError = true)]
        public static extern Int32 NSLook_Query(Int32 _iID, IntPtr _pDvs, IntPtr _pNvs, Int32 _iType);
        [DllImport("nslook.dll")]
        public static extern Int32 NSLook_GetCount(Int32 _iID, byte[] _btUserName, byte[] _btPwd, ref Int32 _iCount, Int32 _iType);
        [DllImport("nslook.dll",CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 NSLook_GetList(Int32 _iID, byte[] _btUserName, byte[] _btPwd, Int32 _iPageIndex, DNSList_NOTIFY _pGetDNS, NVSList_NOTIFY _pGetNVS, Int32 _iType);
        

    }
    class PLAYSDK
    {
        [DllImport("playsdkm4.dll")]
        public static extern Int32 TC_CreateSystem(IntPtr _hwHand);
        [DllImport("playsdkm4.dll")]
        public static extern Int32 TC_DeleteSystem();
        [DllImport("playsdkm4.dll")]
        public static extern Int32 TC_CreatePlayerFromVoD(IntPtr _hwHand, IntPtr _sData, Int32 _iHeadSize);

        [DllImport("playsdkm4.dll")]
        public static extern Int32 TC_Play(Int32 _iID);
        [DllImport("playsdkm4.dll")]
        public static extern Int32 TC_Stop(Int32 _iID);//停止播放
	    [DllImport("playsdkm4.dll")]
        public static extern Int32 TC_DeletePlayer(Int32 _iID);//删除播放器实例
        [DllImport("playsdkm4.dll")]
        public static extern Int32 TC_PlayAudio(Int32 _iID);
        [DllImport("playsdkm4.dll")]
        public static extern Int32 TC_StopAudio(Int32 _iID);
        [DllImport("playsdkm4.dll")]
        public static extern Int32 TC_PutStreamToPlayer(Int32 _iID, IntPtr _Buffer, Int32 _iSize);
        [DllImport("playsdkm4.dll")]
        public static extern Int32 TC_GetFrameCount(Int32 _iID);
        [DllImport("playsdkm4.dll")]
        public static extern Int32 TC_GetPlayingFrameNum(Int32 _iID);
        [DllImport("playsdkm4.dll")]
        public static extern Int32 TC_GetStreamPlayBufferState(Int32 _iID,ref Int32 _iState);
        [DllImport("playsdkm4.dll")]
        public static extern Int32 TC_CleanStreamBuffer(Int32 _iID);
        [DllImport("playsdkm4.dll")]
        public static extern Int32 TC_FastForward(Int32 _iID, Int32 _iSpeed);
        [DllImport("playsdkm4.dll")]
        public static extern Int32 TC_GetUserDataInfo(Int32 _iID, Int32 _iFrameNo, Int32 _iFlag, ref Int32 _Buffer, Int32 _iSize);
    }
}
