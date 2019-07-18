using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace NVSPlayer
{
    class NVSSDK
    {
        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_Startup_V4(Int32 _iServerPort, Int32 _iClientPort, Int32 _iWnd);

        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_SetNotifyFunction_V4(
                                            MAIN_NOTIFY_V4 _MainNotify,
                                            ALARM_NOTIFY_V4 _AlarmNotify,
                                            PARACHANGE_NOTIFY_V4 _ParaNotify,
                                            COMRECV_NOTIFY_V4 _ComNotify,
                                            PROXY_NOTIFY _ProxyNotify);

        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_SetMSGHandleEx(UInt32 _uiMessage, IntPtr _hWnd, UInt32 _uiParaMsg, UInt32 _uiAlarmMsg);   

        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_Logon_V4(Int32 _iLogonType, IntPtr _pvPara, Int32 _iInBufferSize);

        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_Logoff(Int32 _iLogonId);

        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_GetChannelNum(Int32 _iLogonID, ref Int32 _piChanNum);

        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_StartRecv(ref UInt32 _uiConID, ref CLIENTINFO _cltInfo, RECVDATA_NOTIFY _cbkDataArrive);

        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_StopRecv(Int32 _uiConID);

        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_StartPlay(Int32 _uiConID, IntPtr _hWnd, RECT _rcShow, Int32 _iDecflag);

        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_StopPlay(Int32 _uiConID);

        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_StartCaptureFile(Int32 _uiConID, String _pszFilePath, Int32 _iRecordFileType);

        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_StopCaptureFile(Int32 _uiConID);

        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_CapturePicture(Int32 _uiConID, Int32 _iPicType, String _pcFileName);

        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_GetUserNum(Int32 _iLogonID, ref Int32 _piUserNum);

        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_GetUserInfo(Int32 _iLogonID, Int32 _iUserSerial, ref byte _cUserName, ref byte _cPassword, ref Int32 _iAuthority);

        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_AddUser(Int32 _iLogonID, String _cUserName, String _cPassword, Int32 _iAuthority);

        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_DelUser(Int32 _iLogonID, String _cUserName);

	    [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_ModifyPwd(Int32 _iLogonID, String _cUserName, String _cNewPwd);

        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_SetVideoPara(Int32 _iLogonID, Int32 _iChanNO, ref STR_VideoParam _tParam);

        [DllImport("NVSSDK.dll", SetLastError = true)]
        public static extern Int32 NetClient_DeviceCtrlEx(Int32 _iLogonID, Int32 _iChannelNum, Int32 _iActionType, Int32 _iParam1, Int32 _iParam2, Int32 _iControlType);
        
        [DllImport("NVSSDK.dll", SetLastError = true)]
        public static extern Int32 NetClient_DrawRectOnLocalVideo(Int32 _uiConID, ref RECT _rcRect, int _iCount);

        [DllImport("NVSSDK.dll", SetLastError = true)]
        public static extern Int32 NetClient_GetVideoSize(int _iLogonID, int _iChannelNum, ref int _width, ref int _height, int _iStreamNO);
	   
        [DllImport("NVSSDK.dll", SetLastError = true)]
        public static extern Int32 NetClient_SendCommand(int _iLogonID, int _iCommand, int _iChannel, IntPtr _pBuffer, int _iBufferSize);

        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_GetAlarmPortNum(Int32 _iLogonID, ref Int32 _iAlarmChannelNo, ref Int32 _iAlarmOutPortNum);

        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_Query_V5(Int32 _iLogonId, Int32 _iCmdId, Int32 _iChanNo, IntPtr _lpIn, Int32 _iInLen, IntPtr _lpOut, Int32 _iOutLen);

        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_NetFileGetFileCount(Int32 _iLogonID, ref Int32 iTotalCount,ref Int32 _iCurrentCount);

        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_PlayBackControl(Int32 _ulConID, int _iControlCode, IntPtr _pcInBuffer, int _iInLen, IntPtr _pcOutBuffer, ref Int32 _iOutLen);    
   
        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_PlayBack(ref Int32 _ulConID, int _iCmd, ref PlayerParam _PlayerParam, IntPtr _hWnd);
        
        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_StopPlayBack(Int32 _ulConID);

        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_NetFileDownload(ref Int32 _ulConID, int _iLogonID, int _iCmd, IntPtr _lpBuf, int _iBufSize);

        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_NetFileStopDownloadFile(Int32 _ulConID);

        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_NetFileGetDownloadPos(Int32 _uiConID, ref Int32 _iPos, ref Int32 _dlSize);

        [DllImport("NVSSDK.dll")]
        public static extern Int32 NetClient_SetRawFrameCallBack(Int32 _uiConID, RAWFRAME_NOTIFY _cbkGetFrame, IntPtr _pContext);
    }
}
