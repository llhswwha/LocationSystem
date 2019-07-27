using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace NetClient
{
    public delegate void ReplayCallBackDelegate( UInt32 _ulID,
                                                 IntPtr _ucData,
                                                 Int32 _iLen,
                                                 Int32 _iFlag,
                                                 Int32 _lpUserData);


    public partial class PlayForm : BaseForm 
    {
        CLIENTINFO m_ClientInfo;
        string m_szFileName;
        UInt32 m_ulConnID = 0;
        int m_iPlayerID= -1;

        NVS_FILE_TIME m_begintime;
        NVS_FILE_TIME m_endtime;
        int m_iRealChannel;

        //int m_iDataLen;
        int m_iDownloadSpeed;
        int m_iDownloadPause = 1;
        int m_iDownloadType = 0;   // 0: by file 1:by time


        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024 * 1024)]
        public byte[] m_cDataBuffer;

        public static ReplayCallBackDelegate DoCallBack;

        public PlayForm()
        {
            InitializeComponent();
        }

        public PlayForm(CLIENTINFO clientinfo,string sFileName,BaseForm parentfrm)
            :base(parentfrm)
        {
            m_ClientInfo = clientinfo;
            m_szFileName = sFileName;
            m_iDownloadType = 0;
            InitializeComponent();
        }

        public PlayForm(CLIENTINFO clientinfo, int iRealChannel, NVS_FILE_TIME Starttime, NVS_FILE_TIME EndTime, BaseForm parentfrm)
            :base(parentfrm)
        {
            
            InitializeComponent();
            m_iDownloadType = 1;
            m_ClientInfo = clientinfo;
            m_iRealChannel = iRealChannel;
            m_begintime = Starttime;
            m_endtime = EndTime;
        }
        

        private void PlayForm_Shown(object sender, EventArgs e)
        {
            //// TODO:  Add extra initialization here
            //TC_RegisterEventMsg(m_hWnd, PLAYSDKMSG);

            //UI_UpdateText();
            //m_ProgressPlay.SetRange(1, 100);
            //m_ProgressDownload.SetRange(1, 100);
            //m_ProgressDownload.SetPos(0);
            //m_ProgressPlay.SetPos(0);
            //if (m_iDownloadType == DownloadByFile)
            //{
            //    InitDownloadByFile();
            //}
            //else if (m_iDownloadType == DownloadByTime)
            //{
            //    InitDownloadByTime();
            //}
            //SetTimer(TIMER_PLAY_REVIEW_CHECK_PROGRESS, 1000, NULL);
            //SetTimer(TIMER_CHEACK_CONTINUE_DOWNLOAD, 100, NULL);
            //m_hWndVideo = GetDlgItem(IDC_STATIC_PLAYVIDEO)->m_hWnd;

            if (m_iDownloadType == 0)
            {
                StartDownloadByFile();
            }
            else if (m_iDownloadType == 1)
            {
                StartDownloadByTime();
            }
        }
        private void DoCallBackFunction(UInt32 _ulID, IntPtr _ucData, Int32 _iLen,
                                                 Int32 _iFlag,
                                                 Int32 _lpUserData)
        {
            
            
            if (_ulID == m_ulConnID)
            {
	            if (_iFlag == 1)//处理文件头
	            {
                    if (m_iPlayerID == -1)
                    {
                        Control.CheckForIllegalCrossThreadCalls = false;
                        m_iPlayerID = PLAYSDK.TC_CreatePlayerFromVoD(pnlVideoPlay.Handle, _ucData, _iLen);//创建VOD播放器
		                if (m_iPlayerID >= 0)
		                {
                            int i = PLAYSDK.TC_Play(m_iPlayerID);//播放
                            if (i < 0)
                            {
                                MessageBox.Show(m_iPlayerID.ToString()+"TC_Play faild");
                            }
                            i = PLAYSDK.TC_PlayAudio(m_iPlayerID);
                            if (i < 0)
                            {
                                MessageBox.Show(m_iPlayerID.ToString() + "TC_PlayAudio faild");
                            }
		                }
		                else
		                {
			                MessageBox.Show("CreatePlayerFromVoD failed!\n");
		                }
                    }
                    
	            }
	            else//处理数据
	            {
                    int i = PLAYSDK.TC_PutStreamToPlayer(m_iPlayerID, _ucData, _iLen);
                    //if (i < 0)
                    //{
                    //    MessageBox.Show(m_iPlayerID.ToString() + "TC_PutStreamToPlayer faild");
                    //}
		            //CheckStatus();
	            }

            }
            
        }
        public void StartDownloadByFile()
        {
            DoCallBack = new ReplayCallBackDelegate(DoCallBackFunction);

            int iRet = NVSSDK.NetClient_NetFileDownloadFile(ref m_ulConnID, m_ClientInfo.m_iServerID, m_szFileName.ToString(), "", 0, -1, 2);
            if (iRet >= 0)
            {
                IntPtr temp = IntPtr.Zero;
                int irt = NVSSDK.NetClient_SetNetFileDownloadFileCallBack(m_ulConnID, DoCallBack, temp);
            }
            else
            {
                //if (m_ulConnID != -1)  // INVALID_ID)
                {
                    NVSSDK.NetClient_NetFileStopDownloadFile(m_ulConnID);
                    //m_ulConnID = -1;
                }
            }
        }

        public void StartDownloadByTime()
        {
            DoCallBack = new ReplayCallBackDelegate(DoCallBackFunction);
            
            int iRet = NVSSDK.NetClient_NetFileDownloadByTimeSpanEx(ref m_ulConnID, 
                m_ClientInfo.m_iServerID,
                null,
                m_iRealChannel,
                ref m_begintime,
                ref m_endtime , 0, -1, 2);
            if (iRet < 0)
            {
                if (m_ulConnID != -1)
                {
                    NVSSDK.NetClient_NetFileStopDownloadFile(m_ulConnID);
                    m_ulConnID = 0;
                }
                return;
            }
            else
            {
                IntPtr tmep = IntPtr.Zero;
                iRet = NVSSDK.NetClient_SetNetFileDownloadFileCallBack(m_ulConnID, DoCallBack, tmep);
            }
        }

        private void timerPlay_Tick(object sender, EventArgs e)
        {
            ContinueDownload();

            if (m_iDownloadType == 0)
            {
                UI_UpdateProgressByFile();

            }
            else if (m_iDownloadType == 1)
            {
                UI_UpdateProgressByTime();
            }
            else
                return;
        }
        public void ContinueDownload()
        {
	        if (m_ClientInfo.m_iServerID ==  -1 || m_ulConnID == -1)
	        {
		        return;
	        }
        	
	        if (m_iDownloadPause == 1)
	        {
		        int iState = 0;
                int iRet = PLAYSDK.TC_GetStreamPlayBufferState(m_iPlayerID, ref iState);
		        if(iRet == 0)
		        {
			        if (iState == -19) //RET_BUFFER_WILL_BE_EMPTY )
			        {
                        //iRet = NVSSDK.NetClient_NetFileDownloadFile(ref m_ulConnID, m_ClientInfo.m_iServerID, m_szFileName, "", 1, -1, 1);
                        iRet = NVSSDK.NetClient_NetFileDownloadFile(ref m_ulConnID, m_ClientInfo.m_iServerID, "", "", 1, -1, 1);
                        m_iDownloadPause = 0;
			        }
		        }
	        }
        }
        private void UI_UpdateProgressByFile()
        {
            if (m_ulConnID != -1)
            {
                int iProgress = 0, iSize = 0;
                int iRet = NVSSDK.NetClient_NetFileGetDownloadPos(m_ulConnID, ref iProgress, ref iSize);
                if (iRet >= 0)
                {
                    string sProgress = "***" + iProgress.ToString() + "%/" + iSize.ToString() + "dB***"; 
                    lbDownloadProgress.Text = sProgress;
                    if (iProgress > 0 & iProgress < 100)
                    {
                        pgbDownload.Value = iProgress;
                    }
                    
                }
            }
            if (m_iPlayerID != -1)
            {
                if (PLAYSDK.TC_GetFrameCount(m_iPlayerID) == 0)
                {
                    return;
                }
                int i = PLAYSDK.TC_GetPlayingFrameNum(m_iPlayerID);
                int j = PLAYSDK.TC_GetFrameCount(m_iPlayerID);
                int iProgress = i * 100 / j;
                string sProgress = "***" + iProgress.ToString() + "%***"; 
                lbPlayProgress.Text = sProgress;
                if (iProgress <= 100 & iProgress >= 0)
                {
                    pgbPlay.Value = iProgress;
                }
                
            }
        }
        private void UI_UpdateProgressByTime()
        {
            if (m_ulConnID != -1)
            {
                int iCurrentDonwnTime = 0, iSize = 0;
                int iRet = NVSSDK.NetClient_NetFileGetDownloadPos(m_ulConnID, ref iCurrentDonwnTime, ref iSize);
                if (iRet >= 0)
                {
                    
                    DateTime dtTime = CommonFunction.ConvertIntToDateTime(iCurrentDonwnTime);
                    lbDownloadProgress.Text = dtTime.Year.ToString() + "-" +
                                                dtTime.Month.ToString() + "-" +
                                                dtTime.Day.ToString() + " " +
                                                dtTime.Hour.ToString() + ":" +
                                                dtTime.Minute.ToString() + ":" +
                                                dtTime.Second.ToString() + " Size:" +
                                                iSize.ToString() + "db";
                    
                    
                    long uiBeginTime = CommonFunction.NvsFileTimeToAbsSeconds(m_begintime);
                    long uiEndTime = CommonFunction.NvsFileTimeToAbsSeconds(m_endtime);
                    long iTimeInterval = uiEndTime - uiBeginTime;
                    if (iTimeInterval > 0)
                    {
                        long iCurrentInterval = iCurrentDonwnTime - uiBeginTime;
                        if (iCurrentInterval > 0 && iCurrentInterval < iTimeInterval)
                        {

                            long iProcess = iCurrentInterval*100/iTimeInterval;
                            if (iProcess > 100)
                            {
                                return;
                            }
                            pgbDownload.Value = Convert.ToInt32(iProcess);
                        }

                     }
                    
                   
                }
            }
            if (m_iPlayerID != -1)
            {
        		
                Int32  uiCurrentPlayTime  = 0;
                PLAYSDK.TC_GetUserDataInfo(m_iPlayerID, -1, 0,ref uiCurrentPlayTime, Marshal.SizeOf(uiCurrentPlayTime));
                
                if(uiCurrentPlayTime > 0)
                {

                    DateTime dtTime = CommonFunction.ConvertIntToDateTime(uiCurrentPlayTime);
                    lbDownloadProgress.Text = dtTime.Year.ToString() + "-" +
                                                dtTime.Month.ToString() + "-" +
                                                dtTime.Day.ToString() + " " +
                                                dtTime.Hour.ToString() + ":" +
                                                dtTime.Minute.ToString() + ":" +
                                                dtTime.Second.ToString();


                    long uiBeginTime = CommonFunction.NvsFileTimeToAbsSeconds(m_begintime);
                    long uiEndTime = CommonFunction.NvsFileTimeToAbsSeconds(m_endtime);
                    long iTimeInterval = uiEndTime - uiBeginTime;
                    if (iTimeInterval > 0)
                    {
                        long iCurrentInterval = uiCurrentPlayTime - uiBeginTime;
                        if (iCurrentInterval > 0 && iCurrentInterval < iTimeInterval)
                        {

                            long iProcess = iCurrentInterval * 100 / iTimeInterval;
                            if (iProcess > 100)
                            {
                                return;
                            }
                            pgbDownload.Value = Convert.ToInt32(iProcess);
                        }

                    }
                }
            }	
        }
        
        
        private void PlayForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            PLAYSDK.TC_Stop(m_iPlayerID);//停止播放
            PLAYSDK.TC_StopAudio(m_iPlayerID);
            PLAYSDK.TC_DeletePlayer(m_iPlayerID);//删除播放器实例
            PLAYSDK.TC_DeleteSystem();
            NVSSDK.NetClient_NetFileStopDownloadFile(m_ulConnID);

        }
        /// <summary>
        /// Download location
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDLLocation_Click(object sender, EventArgs e)
        {
            if (m_ClientInfo.m_iServerID == -1 || m_ulConnID == -1)
            {
                return;
            }


            int iPos = 0;
            try
            {
                iPos = Convert.ToInt32(tbDLLocation.Text.ToString());
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Please input the Number: 0~100!" + ex.Message);
            }
            

            if (iPos >= 0 && iPos < 100)
            {
                NVSSDK.NetClient_NetFileDownloadFile(ref m_ulConnID, m_ClientInfo.m_iServerID, "", "", 1, iPos, -1);
                if (m_iPlayerID >= 0)
                {
                    PLAYSDK.TC_CleanStreamBuffer(m_iPlayerID);
                }
            }
        }

        private void btnDLSpeedChange_Click(object sender, EventArgs e)
        {
            if (m_ClientInfo.m_iServerID == -1 || m_ulConnID == -1 )//|| m_iDownloadPause)
            {
                return;
            }
            
            int iSpeed = 0;
            try
            {
                iSpeed = Convert.ToInt32(tbDLSpeed.Text.ToString());
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Please input the Number: 0~16!" + ex.Message);
            }
            if (iSpeed >= 0 && iSpeed <= 16)
            {
                m_iDownloadSpeed = iSpeed;
                NVSSDK.NetClient_NetFileDownloadFile(ref m_ulConnID, m_ClientInfo.m_iServerID, "", "", 1, -1, iSpeed);
            }
        }

        private void btnPlaySpeedChange_Click(object sender, EventArgs e)
        {
            if (m_ClientInfo.m_iServerID == -1 || m_ulConnID == -1)
            {
                return;
            }
            
            int iSpeed = 0;
            try
            {
                iSpeed = Convert.ToInt32(tbPlaySpeed.Text.ToString());
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Please input the Number: -4~4!" + ex.Message);
            }
            if (m_iPlayerID != -1)
            {
                if (iSpeed != 0 & iSpeed > -4 & iSpeed < 4)
                {
                    PLAYSDK.TC_FastForward(m_iPlayerID, iSpeed);
                }
                else
                {
                    PLAYSDK.TC_Play(m_iPlayerID);
                }
            }
        }

        private void btnVDderyption_Click(object sender, EventArgs e)
        {
            //if (m_iPlayerID >= 0)
            //{
            //    string szDecryptKey;
            //    GetDlgItemText(IDC_EDIT_PLAYBACK_DECRYPT, szDecryptKey);
            //    TC_SetVideoDecryptKey(m_iPlayerID, szDecryptKey.GetBuffer(), szDecryptKey.GetLength());
            //}
        }




    }
}
