using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using NVSPlayer;
using NVSPlayer.SDK;

namespace PlaybackDemo
{
    public partial class PlayBackForm : Form
    {
        //成员变量
        int m_iLogonId = -1;        //登录ID
        int m_iConnectId = -1;      //视频回放ID
        int m_iDLFileId = -1;       //按文件下载的ID
        //int m_iDLTimeId = -1;       //按时间段下载的ID
        int m_iDLFileIndex = -1;    //按文件下载的索引
        //long m_iDLStartTime = 0;    //按时间段下载开始时间    
        //long m_iDLStopTime = 1;     //按时间段下载结束时间 
         
        //查询条件
        NETFILE_QUERY_V5 m_tQueryInfo = new NETFILE_QUERY_V5();

        //按文件查询每页最大个数
        public const int MAX_PAGESIZE = 20;
        int m_iCurrentPage = 0;     //文件查询当前页
        int m_iTotalPage = 0;       //文件查询总页数

        //码流类型
        public const int STREAM_1ST = 0;
        public const int STREAM_2ND = 1;

        //文件播放类型
        public const int PLAY_TYEBY_FILE = 0;
        public const int PLAY_TYEBY_TIME = 1;
        int m_iPlayType = 0;        //文件播放类型，0按文件，1按时间

        //查询文件类型
        public const int FILE_TYPE_ALL = 0;
        public const int FILE_TYPE_VIDEO = 1;
        public const int FILE_TYPE_PICTURE = 2;

        //抓拍图片格式
        public const int SNAP_PIC_TYPE_JPG = 0;
        public const int SNAP_PIC_TYPE_BMP = 1;

        public PlayBackForm()
        {
            InitializeComponent();
           
            SDK_Init();
            UI_Init();
        }

        public PlayBackForm(string[] args)
        {
            InitializeComponent();

            SDK_Init();
            UI_Init();

            try
            {
                string ip = args[0];
                string port = args[1];
                string user = args[2];
                string pass = args[3];
                string channel = args[4];

                SetLoginInfo(ip, port, user, pass);

                AutoLogin = true;

                AfterLogin = (f, s) =>
                {
                    if (s)
                    {
                        cboChanList.SelectedIndex = (channel.ToInt() - 1);
                        if (args.Length > 6)
                        {
                            string start = args[5];
                            string end = args[6];
                            dtStartTime.Value = start.ToDateTime();
                            dtEndTime.Value = end.ToDateTime();
                        }
                    }
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            //Download(channel.ToInt(), start.ToDateTime(), end.ToDateTime());
        }

        //SDK初始化
        private void SDK_Init()
        {
            NVSSDK.NetClient_Startup_V4(0, 0, 0);
            //此处SDK消息通过系统消息去处理
            NVSSDK.NetClient_SetMSGHandleEx(NetSDKMsg.WM_MAIN_MESSAGE, this.Handle, NetSDKMsg.MSG_PARACHG, NetSDKMsg.MSG_ALARM);
        }

        private void UI_Init()
        {
            //码流类型
            cboStreamNo.Items.Clear();
            cboStreamNo.Items.Insert(STREAM_1ST, "1st");
            cboStreamNo.Items.Insert(STREAM_2ND, "2nd");
            cboStreamNo.SelectedIndex = STREAM_1ST;

            //文件类型
            cboFileType.Items.Clear();
            cboFileType.Items.Insert(FILE_TYPE_ALL, "All");
            cboFileType.Items.Insert(FILE_TYPE_VIDEO, "Video");
            cboFileType.Items.Insert(FILE_TYPE_PICTURE, "Picture");
            cboFileType.SelectedIndex = FILE_TYPE_VIDEO;

            //录像类型
            cboVideoType.Items.Clear();
            cboVideoType.Items.Insert(0, "All");
            cboVideoType.Items.Insert(1, "Manual");
            cboVideoType.Items.Insert(2, "Schedule");
            cboVideoType.Items.Insert(3, "Event");
            cboVideoType.SelectedIndex = 0;

            //报警类型
            cboAlarmType.Items.Clear();
            cboAlarmType.Items.Insert(0, "Disable");
            cboAlarmType.Items.Insert(1, "Port Alarm");
            cboAlarmType.Items.Insert(2, "Motion Detect");
            cboAlarmType.Items.Insert(3, "Video Lost");
            cboAlarmType.SelectedIndex = 0;
           
            //文件下载保存类型
            cboFileSaveFlag.Items.Clear();
            cboFileSaveFlag.Items.Insert(0, "Multi File");
            cboFileSaveFlag.Items.Insert(1, "Single File");
            cboFileSaveFlag.SelectedIndex = 1;

            //文件保存格式
            cboFileSaveFormat.Items.Clear();
            cboFileSaveFormat.Items.Insert(0, "sdv");
            cboFileSaveFormat.Items.Insert(1, "ps(mp4)");
            cboFileSaveFormat.SelectedIndex = 1;
            cboTimeSaveFormat.Items.Clear();
            cboTimeSaveFormat.Items.Insert(0, "sdv");
            cboTimeSaveFormat.Items.Insert(1, "ps(mp4)");
            cboTimeSaveFormat.SelectedIndex = 1;

            //请求数据模式
            cboFileDataMode.Items.Clear();
            cboFileDataMode.Items.Insert(0, "Stream Mode");
            cboFileDataMode.Items.Insert(1, "Frame Mode");
            cboFileDataMode.SelectedIndex = 0;
            cboTimeDataMode.Items.Clear();
            cboTimeDataMode.Items.Insert(0, "Stream Mode");
            cboTimeDataMode.Items.Insert(1, "Frame Mode");
            cboTimeDataMode.SelectedIndex = 1;

            //开始时间设置为yy-dd-mm 00:00:00
            DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            dtStartFile.Value = dt;
            dtStartTime.Value = dt;

            //抓拍图片格式
            cboSnapPicType.Items.Clear();
            cboSnapPicType.Items.Insert(SNAP_PIC_TYPE_JPG, "JPG");
            cboSnapPicType.Items.Insert(SNAP_PIC_TYPE_BMP, "BMP");
            cboSnapPicType.SelectedIndex = SNAP_PIC_TYPE_JPG;
        }

        

        //重写消息处理函数，以处理自定义消息
        protected override void DefWndProc(ref System.Windows.Forms.Message m)
        {
            //WM_MAIN_MESSAGE为自定义的系统消息
            if (m.Msg == NetSDKMsg.WM_MAIN_MESSAGE)
            {
                //自定义消息处理函数
                this.OnNetSDKMsg(m.WParam, m.LParam);
            }
            //默认消息处理函数
            base.DefWndProc(ref m);
        }

        public bool isLogined = false;

        public Action<PlayBackForm,bool> AfterLogin;

        public void OnNetSDKMsg(IntPtr wParam, IntPtr lParam)
        {
            //wParam的低16位是消息的类型；
            int iMsgType = wParam.ToInt32() & 0xFFFF;
            switch (iMsgType)
            {
                case NetSDKMsg.WCM_LOGON_NOTIFY:
                {
                    WriteLog("OnNetSDKMsg:WCM_LOGON_NOTIFY");
                    Int32 iStatus = wParam.ToInt32() >> 16;
                    if (NetSDKMsg.LOGON_SUCCESS == iStatus)
                    {
                        btnLogon.Text = "Logoff";
                        m_iLogonId = (Int32) lParam;

                        //初始化通道列表                         
                        Int32 iChanNum = 0;
                        NVSSDK.NetClient_GetChannelNum(m_iLogonId, ref iChanNum);
                        cboChanList.Items.Clear();
                        for (Int32 i = 0; i < iChanNum; ++i)
                        {
                            cboChanList.Items.Add((i + 1).ToString());
                        }

                        cboChanList.Items.Add("All");
                        if (cboChanList.Items.Count > 0)
                        {
                            cboChanList.SelectedIndex = 0;
                        }

                        //初始化报警通道列表
                        int iAlarmInNum = 0, iAlarmOutNum = 0;
                        int iRet = NVSSDK.NetClient_GetAlarmPortNum(m_iLogonId, ref iAlarmInNum, ref iAlarmOutNum);
                        cboInputPort.Items.Clear();
                        for (int i = 0; i < iAlarmInNum; i++)
                        {
                            cboInputPort.Items.Add((i + 1).ToString());
                        }

                        if (cboInputPort.Items.Count > 0)
                        {
                            cboInputPort.SelectedIndex = 0;
                        }

                        isLogined = true;
                        MessageBoxShow("logon success!");
                    }
                    else
                    {
                        isLogined = false;
                        MessageBoxShow("logon failed, reason " + iStatus);
                    }

                    if (AfterLogin != null)
                    {
                        AfterLogin(this,isLogined);
                    }
                }
                    break;
                case NetSDKMsg.WCM_DWONLOAD_FINISHED:
                {
                    WriteLog("OnNetSDKMsg:WCM_DWONLOAD_FINISHED");
                    int iConnID = lParam.ToInt32();
                    if (m_iDLFileId == iConnID)
                    {
                        m_iDLFileId = -1;
                        lvFileData.Items[m_iDLFileIndex].SubItems[6].Text = "Finished";
                    }
                    else if (downloader != null && downloader.m_iDLTimeId == iConnID)
                    {
                        downloader.m_iDLTimeId = -1;
                        downloader.State = "FINISHED";
                        downloader.IsFinished = true;

                        lbDownloadStatusTime.Text = "Finished";
                        pbDownloadTime.Value = 100;
                    }

                    NVSSDK.NetClient_NetFileStopDownloadFile(iConnID);
                }
                    break;
                case NetSDKMsg.WCM_DWONLOAD_FAULT:
                case NetSDKMsg.WCM_DOWNLOAD_INTERRUPT:
                {
                    WriteLog("OnNetSDKMsg:WCM_DWONLOAD_FAULT|WCM_DOWNLOAD_INTERRUPT");
                    int iConnID = lParam.ToInt32();
                    if (m_iDLFileId == iConnID)
                    {
                        m_iDLFileId = -1;
                    }
                    else if (downloader != null && downloader.m_iDLTimeId == iConnID)
                    {
                        downloader.m_iDLTimeId = -1;

                        downloader.State = "INTERRUPT";
                        downloader.IsFinished = true;
                    }

                    NVSSDK.NetClient_NetFileStopDownloadFile(iConnID);
                }
                    break;
                default:
                {
                    WriteLog("OnNetSDKMsg:default,iMsgType=" + iMsgType);
                }
                    break;
            }
        }

        private void btnLogon_Click(object sender, EventArgs e)
        {
            LoginSwitch();
        }

        public void SetIp(string ip)
        {
            textIP.Text = ip;
        }

        public void SetLoginInfo(string ip,string port,string user,string pass)
        {
            textIP.Text = ip;
            textPort.Text = port;
            textUser.Text = user;
            textPwd.Text = pass;
        }

        public string GetIp()
        {
            return textIP.Text;
        }

        public bool LoginSwitch()
        {
            WriteLog("LoginSwitch");
            if ("Logon" == btnLogon.Text)   //登陆
            {
                return Login();
            }
            else  //注销
            {
                return Logout();
            }
        }

        public bool Logout()
        {
            WriteLog("Logout");
            btnLogon.Text = "Logon";
            if (m_iLogonId < 0)
            {
                WriteLog("m_iLogonId < 0");
                return false;
            }

            //停止视频播放
            Play_Stop();

            //停止下载
            DLFile_Stop();
            DLTime_Stop();

            NVSSDK.NetClient_Logoff(m_iLogonId);
            m_iLogonId = -1;

            isLogined = false;
            return true;
        }

        public bool Login()
        {
            WriteLog("Login");
            LogonPara tInfo = new LogonPara();
            tInfo.iSize = Marshal.SizeOf(tInfo);
            tInfo.iNvsPort = Int32.Parse(textPort.Text);
            tInfo.cNvsIP = new char[32];
            Array.Copy(textIP.Text.ToCharArray(), tInfo.cNvsIP, textIP.Text.Length);
            tInfo.cUserName = new char[16];
            Array.Copy(textUser.Text.ToCharArray(), tInfo.cUserName, textUser.Text.Length);
            tInfo.cUserPwd = new char[16];
            Array.Copy(textPwd.Text.ToCharArray(), tInfo.cUserPwd, textPwd.Text.Length);

            IntPtr intptr = Marshal.AllocCoTaskMem(tInfo.iSize);
            Marshal.StructureToPtr(tInfo, intptr, true);
            Int32 iLogonId = NVSSDK.NetClient_Logon_V4(0, intptr, tInfo.iSize);
            Marshal.FreeHGlobal(intptr);
            if (iLogonId < 0)
            {
                MessageBoxShow("logon failed!");
                return false;
            }
            else
            {
                return true;
            }
        }


        //原始流回调函数
        private void Notify_RawFrame(UInt32 _ulID, IntPtr _cData, int _iLen, ref RAWFRAME_INFO _pRawFrameInfo, IntPtr _iUser)
        {
            Int32 iConnId = (Int32)_ulID;
            if (iConnId != m_iConnectId)
            {
                return;
            }

            if (null == _cData || _iLen <= 0)
            {
                return;
            }

            if (0 == _pRawFrameInfo.nType)         //I帧
            {
                //do something
            }
            else if (1 == _pRawFrameInfo.nType)    //P帧
            {
                //do something
            }
            else if (5 == _pRawFrameInfo.nType)    //音频数据
            {
                //do something
            }
        }

        /*************************************************************************************************************************************/
        /*************************************************************************************************************************************/
        /*
                                                                分界线，文件查询  
         */
        /*************************************************************************************************************************************/
        /*************************************************************************************************************************************/      
        private void UI_UpdataList(NVS_FILE_DATA _tData, Int32 _iIndex)
        {
            ListViewItem item = new ListViewItem();
            item.Text = (m_iCurrentPage * MAX_PAGESIZE + _iIndex + 1).ToString();
            string str = new string(_tData.m_cFileName, 0, _tData.m_cFileName.Length);
            str = str.Trim("\0".ToCharArray());
            item.SubItems.Add(str);
            String strInfo = "CH:" + _tData.m_iChannel + ",TPYE:" + _tData.m_iType;
            item.SubItems.Add(strInfo);
            item.SubItems.Add(_tData.m_iFileSize.ToString());
            string stime = _tData.m_struStartTime.m_iYear.ToString() + "-" +
                                  _tData.m_struStartTime.m_iMonth.ToString("D2") + "-" +
                                  _tData.m_struStartTime.m_iDay.ToString("D2") + " " +
                                  _tData.m_struStartTime.m_iHour.ToString("D2") + ":" +
                                  _tData.m_struStartTime.m_iSecond.ToString("D2") + ":" +
                                  _tData.m_struStartTime.m_iMinute.ToString("D2");
            item.SubItems.Add(stime);
            stime = _tData.m_struStoptime.m_iYear.ToString() + "-" +
                           _tData.m_struStoptime.m_iMonth.ToString("D2") + "-" +
                           _tData.m_struStoptime.m_iDay.ToString("D2") + " " +
                           _tData.m_struStoptime.m_iHour.ToString("D2") + ":" +
                           _tData.m_struStoptime.m_iSecond.ToString("D2") + ":" +
                           _tData.m_struStoptime.m_iMinute.ToString("D2");
            item.SubItems.Add(stime);
            item.SubItems.Add("");  // 预留增加下载进度
            lvFileData.Items.Add(item);
        }

        private void QueryFile(int _iPageNo)
        {          
            if (m_iLogonId < 0)
            {
                MessageBoxShow("Please logon device first!");
                return;
            }

            if (_iPageNo < 0 || (_iPageNo >= m_iTotalPage && m_iTotalPage > 0))
            {
                return;
            }

            if (_iPageNo == m_iCurrentPage && m_iTotalPage > 0)
            {
                return;
            }

            //入参
            m_tQueryInfo.iPageNo = _iPageNo;
            int iQuerySize = m_tQueryInfo.iBufSize;
            IntPtr ptrQuery = Marshal.AllocCoTaskMem(iQuerySize);
            Marshal.StructureToPtr(m_tQueryInfo, ptrQuery, true);
            //出参
            int iRetSize = Marshal.SizeOf(typeof(NVS_FILE_DATA));
            NVS_FILE_DATA[] tResult = new NVS_FILE_DATA[MAX_PAGESIZE];//注：此处数组大小要与每页查询总数(m_tQueryInfo.iPageSize)一致
            for (int i = 0; i < MAX_PAGESIZE; i++)
            {
                tResult[i] = new NVS_FILE_DATA();
            }
            IntPtr ptrResult = Marshal.AllocHGlobal(iQuerySize * MAX_PAGESIZE);

            //查询
            int iRet = NVSSDK.NetClient_Query_V5(m_iLogonId, NetSDKCmd.CMD_NETFILE_QUERY_FILE, m_tQueryInfo.iChannNo, ptrQuery, iQuerySize, ptrResult, iQuerySize);
            Marshal.FreeHGlobal(ptrQuery);
            if (iRet < 0)
            {
                Marshal.FreeHGlobal(ptrResult);
                MessageBoxShow("Query file failed, ret=" + iRet);
                return;
            }

            //获取总数,更新列表
            m_iCurrentPage = _iPageNo;
            lvFileData.Items.Clear();
            int iTotalCount = 0, iCurCount = 0;           
            NVSSDK.NetClient_NetFileGetFileCount(m_iLogonId, ref iTotalCount, ref iCurCount);

            lableFileCount.Text = "Total File :" + iTotalCount;
            for (int i = 0; i < MAX_PAGESIZE && i < iCurCount; i++)
            {
                IntPtr ptr = (IntPtr)((UInt32)ptrResult + i * iQuerySize);
                tResult[i] = (NVS_FILE_DATA)Marshal.PtrToStructure(ptr, typeof(NVS_FILE_DATA));
                UI_UpdataList(tResult[i], i);
            }
            Marshal.FreeHGlobal(ptrResult);


            //计算总页数
            m_iTotalPage = iTotalCount / MAX_PAGESIZE;
            if (0 != iTotalCount % MAX_PAGESIZE && iTotalCount > 0)
            {
                m_iTotalPage++;
            }
            
            cboTotolPage.Items.Clear();
            for (int i = 0; i < m_iTotalPage; i++)
            {
                cboTotolPage.Items.Add((i + 1).ToString());
            }
            if (cboTotolPage.Items.Count > 0)
            {
                cboTotolPage.SelectedIndex = m_iCurrentPage;
            }
        }

        //查询按钮
        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (m_iLogonId < 0)
            {
                MessageBoxShow("Please logon device first!");
                return;
            }

            if (dtStartFile.Value >= dtEndFile.Value)
            {
                MessageBoxShow("Start time >= End time");
                return;
            }

            m_tQueryInfo.iBufSize = Marshal.SizeOf(m_tQueryInfo); ;

            //通道号
            if ((cboChanList.Items.Count-1) == cboChanList.SelectedIndex)
            {
                m_tQueryInfo.iChannNo = 0x7FFFFFFF;
            }
            else
            {
                m_tQueryInfo.iChannNo = cboChanList.SelectedIndex;
            }

            //码流号
            m_tQueryInfo.iStreamNo = cboStreamNo.SelectedIndex;

            //文件类型
            m_tQueryInfo.iFiletype = cboFileType.SelectedIndex;  

            //录像类型
            int iVideoType = cboVideoType.SelectedIndex;
            if (0 == iVideoType)
            {
                m_tQueryInfo.iType = 0xFF;
            }
            else if (iVideoType <= 3)
            {
                m_tQueryInfo.iType = iVideoType;
            }

            //时间
            m_tQueryInfo.tStartTime.m_iYear = Convert.ToUInt16(dtStartFile.Value.Year);
            m_tQueryInfo.tStartTime.m_iMonth = Convert.ToUInt16(dtStartFile.Value.Month);
            m_tQueryInfo.tStartTime.m_iDay = Convert.ToUInt16(dtStartFile.Value.Day);
            m_tQueryInfo.tStartTime.m_iHour = Convert.ToUInt16(dtStartFile.Value.Hour);
            m_tQueryInfo.tStartTime.m_iMinute = Convert.ToUInt16(dtStartFile.Value.Minute);
            m_tQueryInfo.tStartTime.m_iSecond = Convert.ToUInt16(dtStartFile.Value.Second);

            m_tQueryInfo.tStopTime.m_iYear = Convert.ToUInt16(dtEndFile.Value.Year);
            m_tQueryInfo.tStopTime.m_iMonth = Convert.ToUInt16(dtEndFile.Value.Month);
            m_tQueryInfo.tStopTime.m_iDay = Convert.ToUInt16(dtEndFile.Value.Day);
            m_tQueryInfo.tStopTime.m_iHour = Convert.ToUInt16(dtEndFile.Value.Hour);
            m_tQueryInfo.tStopTime.m_iMinute = Convert.ToUInt16(dtEndFile.Value.Minute);
            m_tQueryInfo.tStopTime.m_iSecond = Convert.ToUInt16(dtEndFile.Value.Second);

            //每页最大20个
            m_tQueryInfo.iPageSize = MAX_PAGESIZE;
            m_iTotalPage = 0;
            m_iCurrentPage = 0;

            QueryFile(0);
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {            
            QueryFile(0);
        }

        private void btnUpPage_Click(object sender, EventArgs e)
        {
            QueryFile(m_iCurrentPage - 1);
        }

        private void butNextPage_Click(object sender, EventArgs e)
        {          
            QueryFile(m_iCurrentPage+1);
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {           
            QueryFile(m_iTotalPage - 1);
        }

        private void cboTotolPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            QueryFile(cboTotolPage.SelectedIndex);
        }

        /*************************************************************************************************************************************/
        /*************************************************************************************************************************************/
        /*
                                                                分界线，文件播放  
         */
        /*************************************************************************************************************************************/
        /*************************************************************************************************************************************/
        private void Play_Stop()
        {
            timerPlayPos.Enabled = false;   //停止定时器
            if (m_iConnectId >= 0)
            {
                NVSSDK.NetClient_StopPlayBack(m_iConnectId);
                m_iConnectId = -1;
                lablePlayProcess.Text = "00:00:00";
            }
        }

        //按文件播放
        private void btnPlayByFile_Click(object sender, EventArgs e)
        {
            if (m_iLogonId < 0)
            {
                MessageBoxShow("Please logon device first!");
                return;
            }

            //选择录像文件
            int iIndex = -1;
            for (int i = 0; i < lvFileData.Items.Count; i++)
            {
                if (lvFileData.Items[i].Checked)
                {
                    iIndex = i;
                    break;
                }
            }
            if (iIndex < 0)
            {
                MessageBoxShow("Please select a file to play!");
                return;
            }

            //先停止
            Play_Stop();

            //播放
            PlayerParam stParam = new PlayerParam();
            stParam.iSize = Marshal.SizeOf(stParam);
            stParam.iLogonID = m_iLogonId;
            stParam.cFileName = new char[128];
            stParam.cLocalFilename = new char[256];
            string strFileName = lvFileData.Items[iIndex].SubItems[1].Text.ToString();
            Array.Copy(strFileName.ToCharArray(), stParam.cFileName, strFileName.Length);
            int iRet = NVSSDK.NetClient_PlayBack(ref m_iConnectId, NetSDKCmd.PLAYBACK_TYPE_FILE, ref stParam, panelVideoShow.Handle);
            if (iRet < 0)
            {
                MessageBoxShow("Play back by file failed，ret=" + iRet);
                return;
            }
            m_iPlayType = PLAY_TYEBY_FILE;
            timerPlayPos.Enabled = true;    //开启定时器获取下载进度
            //设置原始流回调
            //NVSSDK.NetClient_SetRawFrameCallBack(m_iConnectId, Notify_RawFrame, IntPtr.Zero);
        }

        //按时间段播放
        private void btnPlayByTime_Click(object sender, EventArgs e)
        {
            if (m_iLogonId < 0)
            {
                MessageBoxShow("Please logon device first!");
                return;
            }

            if (cboChanList.SelectedIndex >= (cboChanList.Items.Count-1))
            {
                MessageBoxShow("Please select a correct channel number!");
                return;
            }

            if (dtStartTime.Value >= dtEndTime.Value)
            {
                MessageBoxShow("Start time >= End time");
                return;
            }
        
            //先停止
            Play_Stop();

            Int32 iTotalChanNum = 0;
            NVSSDK.NetClient_GetChannelNum(m_iLogonId, ref iTotalChanNum);

            //计算真实通道号
            int iStreamNo = cboStreamNo.SelectedIndex;
            int iReanChanNo = cboChanList.SelectedIndex + iStreamNo*iTotalChanNum;

            //播放
            PlayerParam stParam = new PlayerParam();
            stParam.iSize = Marshal.SizeOf(stParam);
            stParam.iLogonID = m_iLogonId;
            stParam.iChannNo = iReanChanNo;
            stParam.tBeginTime.m_iYear = Convert.ToUInt16(dtStartTime.Value.Year);
            stParam.tBeginTime.m_iMonth = Convert.ToUInt16(dtStartTime.Value.Month);
            stParam.tBeginTime.m_iDay = Convert.ToUInt16(dtStartTime.Value.Day);
            stParam.tBeginTime.m_iHour = Convert.ToUInt16(dtStartTime.Value.Hour);
            stParam.tBeginTime.m_iMinute = Convert.ToUInt16(dtStartTime.Value.Minute);
            stParam.tBeginTime.m_iSecond = Convert.ToUInt16(dtStartTime.Value.Second);

            stParam.tEndTime.m_iYear = Convert.ToUInt16(dtEndTime.Value.Year);
            stParam.tEndTime.m_iMonth = Convert.ToUInt16(dtEndTime.Value.Month);
            stParam.tEndTime.m_iDay = Convert.ToUInt16(dtEndTime.Value.Day);
            stParam.tEndTime.m_iHour = Convert.ToUInt16(dtEndTime.Value.Hour);
            stParam.tEndTime.m_iMinute = Convert.ToUInt16(dtEndTime.Value.Minute);
            stParam.tEndTime.m_iSecond = Convert.ToUInt16(dtEndTime.Value.Second);

            int iRet = NVSSDK.NetClient_PlayBack(ref m_iConnectId, NetSDKCmd.PLAYBACK_TYPE_TIME, ref stParam, panelVideoShow.Handle);
            if (iRet < 0)
            {
                MessageBoxShow("Play back by time failed!");
                return;
            }
            m_iPlayType = PLAY_TYEBY_TIME;
            timerPlayPos.Enabled = true;      //启动定时器获取播放进度
            //设置原始流回调
            //NVSSDK.NetClient_SetRawFrameCallBack(m_iConnectId, Notify_RawFrame, IntPtr.Zero);
        }

        //播放控制：播放
        private void btnPlayPlay_Click(object sender, EventArgs e)
        {
            if (m_iConnectId >= 0)
            {
                int iOutLen = 0;
                NVSSDK.NetClient_PlayBackControl(m_iConnectId, NetSDKCmd.PLAY_CONTROL_PLAY, IntPtr.Zero, 0, IntPtr.Zero, ref iOutLen);
            }
        }

        //播放控制：暂停
        private void btnPlayPause_Click(object sender, EventArgs e)
        {
            if (m_iConnectId >= 0)
            {
                int iOutLen = 0;
                NVSSDK.NetClient_PlayBackControl(m_iConnectId, NetSDKCmd.PLAY_CONTROL_PAUSE, IntPtr.Zero, 0, IntPtr.Zero, ref iOutLen);
            }
        }

        //播放控制：停止
        private void btnPlayStop_Click(object sender, EventArgs e)
        {
            Play_Stop();
        }

        //播放控制：慢进
        private void btnPlaySlow_Click(object sender, EventArgs e)
        {
            int iOutLen = 0;
            Int32 iSpeed = 2;
            IntPtr prtIn = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(Int32)));
            Marshal.StructureToPtr(iSpeed, prtIn, true);
            NVSSDK.NetClient_PlayBackControl(m_iConnectId, NetSDKCmd.PLAY_CONTROL_SLOW_FORWARD, prtIn, 0, IntPtr.Zero, ref iOutLen);
            Marshal.FreeHGlobal(prtIn);
        }

        //播放控制：快进
        private void btnPlayFast_Click(object sender, EventArgs e)
        {
            if (m_iConnectId >= 0)
            {
                int iOutLen = 0;
                Int32 iSpeed = 4;
                IntPtr prtIn = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(Int32)));
                Marshal.StructureToPtr(iSpeed, prtIn, true);
                NVSSDK.NetClient_PlayBackControl(m_iConnectId, NetSDKCmd.PLAY_CONTROL_FAST_FORWARD, prtIn, 0, IntPtr.Zero, ref iOutLen);
                Marshal.FreeHGlobal(prtIn);
            }
        }

        //播放控制：步进
        private void btnPlayStep_Click(object sender, EventArgs e)
        {
           if (m_iConnectId >= 0)
            {
                int iOutLen = 0;
                NVSSDK.NetClient_PlayBackControl(m_iConnectId, NetSDKCmd.PLAY_CONTROL_STEPFORWARD, IntPtr.Zero, 0, IntPtr.Zero, ref iOutLen);
            } 
        }

        private void btnPlaySeek_Click(object sender, EventArgs e)
        {
            if (m_iConnectId < 0)
            {
                return;
            }

            int iPos = Int32.Parse(textSeekPos.Text);
            if (iPos <= 0 || iPos >= 100)
            {
                MessageBoxShow("Please input number between 1 and 99!");
                return;
            }

            int iOutLen = 0;
            IntPtr prtIn = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(Int32)));
            Marshal.StructureToPtr(iPos, prtIn, true);
            NVSSDK.NetClient_PlayBackControl(m_iConnectId, NetSDKCmd.PLAY_CONTROL_SEEK, prtIn, 0, IntPtr.Zero, ref iOutLen);
            Marshal.FreeHGlobal(prtIn);
        }

        //获取播放进度
        private void timerPlayPos_Tick(object sender, EventArgs e)
        {
            if (m_iConnectId < 0) 
            {
                return;
            }

            if (PLAY_TYEBY_FILE == m_iPlayType)
            {
                int iOutLen = 0;
                PlaybackProcess tInfo = new PlaybackProcess();
                tInfo.iSize = Marshal.SizeOf(tInfo);
                tInfo.iPlayByFileOrTime = NetSDKCmd.PLAYBACK_TYPE_FILE;
                IntPtr intptr = Marshal.AllocCoTaskMem(tInfo.iSize);
                Marshal.StructureToPtr(tInfo, intptr, true);
                NVSSDK.NetClient_PlayBackControl(m_iConnectId, NetSDKCmd.PLAY_CONTROL_GET_PROCESS, intptr, tInfo.iSize, IntPtr.Zero, ref iOutLen);
                tInfo = (PlaybackProcess)Marshal.PtrToStructure(intptr, typeof(PlaybackProcess));
                Marshal.FreeHGlobal(intptr);

                int iPlayTime = (int)tInfo.uiCurrentPlayTime / 1000;
                string strPlayTime = (iPlayTime / 3600).ToString("D2") + ":" + ((iPlayTime % 3600) / 60).ToString("D2") + ":" +
                                    ((iPlayTime % 3600) % 60).ToString("D2");
                lablePlayProcess.Text = strPlayTime;
            }
            else if (PLAY_TYEBY_TIME == m_iPlayType)
            {
                Int32 iAbsTime = 0;
                Int32 iFlag = NetSDKCmd.GET_USERDATA_INFO_TIME;
                Int32 iSize = Marshal.SizeOf(iFlag);
                IntPtr prtIn = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(Int32)));
                IntPtr prtOut = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(Int32)));
                Marshal.StructureToPtr(iFlag, prtIn, true);
                Marshal.StructureToPtr(iAbsTime, prtOut, true);

                NVSSDK.NetClient_PlayBackControl(m_iConnectId, NetSDKCmd.PLAY_CONTROL_GETUSERINFO, prtIn, iSize, prtOut, ref iSize);                
                iAbsTime = (Int32)Marshal.PtrToStructure(prtOut, typeof(Int32));
                Marshal.FreeHGlobal(prtIn);
                Marshal.FreeHGlobal(prtOut);
                if (iAbsTime > 0)
                {
                    lablePlayProcess.Text = CommonFunc.AbsSecondsToStr(iAbsTime);
                }                
            }
        }

        //抓拍图片
        private void btnSnapShot_Click(object sender, EventArgs e)
        {
            if (m_iConnectId < 0) {
                return;
            }

            String strFileName = "Snap_";
            strFileName += CommonFunc.GetCurTimeStr();

            int iType = cboSnapPicType.SelectedIndex;
            if (SNAP_PIC_TYPE_JPG == iType)
            {
                iType = NetSDKType.CAPTURE_PICTURE_TYPE_JPG;
                strFileName += ".jpg";
            }
            else if (SNAP_PIC_TYPE_BMP == iType)
            {
                iType = NetSDKType.CAPTURE_PICTURE_TYPE_BMP;
                strFileName += ".bmp";
            }

            int iRet = NVSSDK.NetClient_CapturePicture(m_iConnectId, iType, strFileName);
            if (iRet >= 0)
            {
                MessageBoxShow("Snap shot success! name=" + strFileName);
            }
            else
            {
                MessageBoxShow("Snap shot failed!");
            }
        } 

        /*************************************************************************************************************************************/
        /*************************************************************************************************************************************/
        /*
                                                                分界线，文件下载  
         */
        /*************************************************************************************************************************************/
        /*************************************************************************************************************************************/

        private void DLFile_Stop()
        {
            timerDLFilePos.Enabled = false;    //停止定时器
            if (m_iDLFileId >= 0)
            {
                NVSSDK.NetClient_NetFileStopDownloadFile(m_iDLFileId);
                m_iDLFileId = -1;
                if (m_iDLFileIndex >= 0)
                {
                    lvFileData.Items[m_iDLFileIndex].SubItems[6].Text = "";
                    m_iDLFileIndex = -1;
                }
            }
        }

        public void Stop()
        {
            DLTime_Stop();
            DLFile_Stop();
        }

        public void DLTime_Stop()
        {
            timerDLTimePos.Enabled = false;    //停止定时器
            //if (m_iDLTimeId >= 0)
            //{               
            //    NVSSDK.NetClient_NetFileStopDownloadFile(m_iDLTimeId);
            //    m_iDLTimeId = -1;
            //}

            if (downloader != null)
            {
                downloader.Stop();
            }
            lbDownloadStatusTime.Text = "";
            pbDownloadTime.Value = 0;
        }

        //按文件：开始下载
        private void btnDLFileStart_Click(object sender, EventArgs e)
        {
            if (m_iLogonId < 0)
            {
                MessageBoxShow("Please logon device first!");
                return;
            }

            //判断选择的文件是否有在下载
            if (m_iDLFileId > 0)
            {
                MessageBoxShow("It is downloading!");
                return;
            }

            //选择录像文件
            int iIndex = -1;
            for (int i = 0; i < lvFileData.Items.Count; i++)
            {
                if (lvFileData.Items[i].Checked)
                {
                    iIndex = i;
                    break;
                }
            }

            if (iIndex < 0)
            {
                MessageBoxShow("Please select a file to download!");
                return;
            }

            lvFileData.Items[iIndex].SubItems[6].Text = "";
            m_iDLFileIndex = iIndex;
            string strFileName = lvFileData.Items[iIndex].SubItems[1].Text.ToString();

            //下载所需信息
            DOWNLOAD_FILE tInfo = new DOWNLOAD_FILE();
            tInfo.m_iSize = Marshal.SizeOf(tInfo);
            tInfo.m_iSpeed = 32;
            tInfo.m_iPosition = -1;
            tInfo.m_cRemoteFilename = new char[255];
            tInfo.m_cLocalFilename = new char[255];
            tInfo.m_iReqMode = cboFileDataMode.SelectedIndex;
            Array.Copy(strFileName.ToCharArray(), tInfo.m_cRemoteFilename, strFileName.Length);

            //本地保存文件格式和名称
            string strLocalFileName = "Download" + strFileName;
            string strFileType = strFileName.Substring(strFileName.Length - 4, 4);
            int iSaveFormat = cboFileSaveFormat.SelectedIndex;
            if (0 == iSaveFormat || ".jpg" == strFileType || ".Jpg" == strFileType || ".JPG" == strFileType)
            {
                //下载sdv格式或者图片
                tInfo.m_iSaveFileType = NetSDKCmd.DOWNLOAD_FILE_TYPE_SDV;
            }
            else if (1 == iSaveFormat) //PS(MP4格式)
            {
                tInfo.m_iSaveFileType = NetSDKCmd.DOWNLOAD_FILE_TYPE_PS;
                strLocalFileName = "Download" + strFileName.Substring(0, strFileName.Length - 4) + ".ps";
            }

            Array.Copy(strLocalFileName.ToCharArray(), tInfo.m_cLocalFilename, strLocalFileName.Length);

            IntPtr intptr = Marshal.AllocCoTaskMem(tInfo.m_iSize);
            Marshal.StructureToPtr(tInfo, intptr, true);
            int iRet = NVSSDK.NetClient_NetFileDownload(ref m_iDLFileId, m_iLogonId, NetSDKCmd.DOWNLOAD_CMD_FILE,
                intptr, tInfo.m_iSize);
            Marshal.FreeHGlobal(intptr);
            if (iRet < 0)
            {
                MessageBoxShow("Download file failed, ret=!" + iRet);
                return;
            }

            timerDLFilePos.Enabled = true; //开启定时器获取下载进度
        }

        //按文件：停止下载
        private void btnDLFileStop_Click(object sender, EventArgs e)
        {
            DLFile_Stop();   
        }

        //按文件：暂停下载
        private void btnDLFilePause_Click(object sender, EventArgs e)
        {
            if (m_iDLFileId > 0)
            {
                DOWNLOAD_CONTROL tInfo = new DOWNLOAD_CONTROL();
                tInfo.m_iSize = Marshal.SizeOf(tInfo);
                tInfo.m_iSpeed = 0;
                tInfo.m_iPosition = -1;
                tInfo.m_iReqMode = cboFileDataMode.SelectedIndex;
                IntPtr intptr = Marshal.AllocCoTaskMem(tInfo.m_iSize);
                Marshal.StructureToPtr(tInfo, intptr, true);
                NVSSDK.NetClient_NetFileDownload(ref m_iDLFileId, m_iLogonId, NetSDKCmd.DOWNLOAD_CMD_CONTROL, intptr, tInfo.m_iSize);
                Marshal.FreeHGlobal(intptr);
            } 
        }

        //按文件：继续下载
        private void btnDLFileContinue_Click(object sender, EventArgs e)
        {
            if (m_iDLFileId > 0)
            {
                DOWNLOAD_CONTROL tInfo = new DOWNLOAD_CONTROL();
                tInfo.m_iSize = Marshal.SizeOf(tInfo);
                tInfo.m_iSpeed = 16;
                tInfo.m_iPosition = -1;
                tInfo.m_iReqMode = cboFileDataMode.SelectedIndex;
                IntPtr intptr = Marshal.AllocCoTaskMem(tInfo.m_iSize);
                Marshal.StructureToPtr(tInfo, intptr, true);
                NVSSDK.NetClient_NetFileDownload(ref m_iDLFileId, m_iLogonId, NetSDKCmd.DOWNLOAD_CMD_CONTROL, intptr, tInfo.m_iSize);
                Marshal.FreeHGlobal(intptr);
            }
        }

        //定时器获取下载进度
        private void timerDownloadPos_Tick(object sender, EventArgs e)
        {
            if (m_iDLFileId >= 0)        //获取按文件下载进度
            {
                Int32 iPos = 0, iSize = 0;
                NVSSDK.NetClient_NetFileGetDownloadPos(m_iDLFileId, ref iPos, ref iSize);
                string strPos = "0%";
                if (iPos > 0 & iPos < 100)
                {
                    strPos = iPos.ToString() + "%";
                }
                lvFileData.Items[m_iDLFileIndex].SubItems[6].Text = strPos;
            }
            else
            {

            }
        }

        //获取按时间段下载进度
        private void timerDLTimePos_Tick(object sender, EventArgs e)
        {
            var m_iDLTimeId = downloader.m_iDLTimeId;
            if (m_iDLTimeId >= 0) //获取按时间段下载进度
            {
                Int32 iPos = 0, iSize = 0;
                NVSSDK.NetClient_NetFileGetDownloadPos(downloader.m_iDLTimeId, ref iSize, ref iPos);
                WriteLog(string.Format("iPos:{0},iSize:{1}", iPos, iSize));
                Int32 iTotal = (Int32) (downloader.m_iDLStopTime - downloader.m_iDLStartTime);
                Int32 iSetPos = (iPos - (Int32)downloader.m_iDLStartTime) * 100 / iTotal;
                WriteLog(string.Format("iTotal:{0},iSetPos:{1}", iTotal, iSetPos));
                if (iSetPos > 0 && iSetPos <= 100)
                {
                    pbDownloadTime.Value = iSetPos;
                    lbDownloadStatusTime.Text = CommonFunc.AbsSecondsToStr(iPos);
                }
            }

            //if (downloader.GetProgress())
            //{
            //    pbDownloadTime.Value = downloader.Progress;
            //    lbDownloadStatusTime.Text = downloader.ProgressText;
            //}
        }

        //按时间：开始下载
        private void btnDLTimeStart_Click(object sender, EventArgs e)
        {
            Download();
        }

        public string Message = "";

        public bool IsShowMessageBox = true;

        private void MessageBoxShow(string msg)
        {
            Message = msg;
            if(IsShowMessageBox)
                MessageBox.Show(msg);
            WriteLog("msg:"+msg);
        }


        public bool Download(int channel, DateTime dateTime1, DateTime dateTime2)
        {
            try
            {
                dtStartTime.Value = dateTime1;
                dtEndTime.Value = dateTime2;
                cboChanList.SelectedIndex = (channel-1);
                return Download();
            }
            catch (Exception e)
            {
                WriteLog(e.ToString());
                Message = e.ToString();
                return false;
            }
            
        }

        public string Log = "";

        public const string LogonFirst = "Please logon device first!";

        public const string Downloading = "It is downloading!";

        public const string TimeError = "Start time >= End time";

        public void WriteLog(string log)
        {
            string txt = string.Format("[{0}]{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), log);
            Log = txt + "\n" + Log;
            richTextBox1.Text = Log;
        }

        public void ClearLog()
        {
            Log = "";
            richTextBox1.Text = Log;
        }

        private bool Download()
        {
            WriteLog("Download");
            if (m_iLogonId < 0)
            {
                MessageBoxShow(LogonFirst);
                return false;
            }
            if (downloader != null && downloader.m_iDLTimeId > 0)
            {
                MessageBoxShow(LogonFirst);
                return false;
            }
            if (dtStartTime.Value >= dtEndTime.Value)
            {
                MessageBoxShow(LogonFirst);
                return false;
            }
            lbDownloadStatusTime.Text = "";
            pbDownloadTime.Value = 0;

            //DOWNLOAD_TIMESPAN tInfo = new DOWNLOAD_TIMESPAN();
            //tInfo.m_iSize = Marshal.SizeOf(tInfo);
            //tInfo.m_iChannelNO = cboChanList.SelectedIndex;
            //tInfo.m_iSaveFileType = 1;      //下载保存为PS(MP4)格式
            //tInfo.m_iSpeed = 32;
            //tInfo.m_iPosition = -1;
            //tInfo.m_iStreamNo = cboStreamNo.SelectedIndex;
            //tInfo.m_iFileFlag = cboFileSaveFlag.SelectedIndex;//0多个文件  1单个文件
            //tInfo.m_iReqMode = cboTimeDataMode.SelectedIndex;//0流模式(设备不发下载时间进度,不支持跨文件), 1帧模式(设备发下载时间进度，支持跨文件)
            //tInfo.m_tTimeBegin.m_iYear = Convert.ToUInt16(dtStartTime.Value.Year);
            //tInfo.m_tTimeBegin.m_iMonth = Convert.ToUInt16(dtStartTime.Value.Month);
            //tInfo.m_tTimeBegin.m_iDay = Convert.ToUInt16(dtStartTime.Value.Day);
            //tInfo.m_tTimeBegin.m_iHour = Convert.ToUInt16(dtStartTime.Value.Hour);
            //tInfo.m_tTimeBegin.m_iMinute = Convert.ToUInt16(dtStartTime.Value.Minute);
            //tInfo.m_tTimeBegin.m_iSecond = Convert.ToUInt16(dtStartTime.Value.Second);

            //tInfo.m_tTimeEnd.m_iYear = Convert.ToUInt16(dtEndTime.Value.Year);
            //tInfo.m_tTimeEnd.m_iMonth = Convert.ToUInt16(dtEndTime.Value.Month);
            //tInfo.m_tTimeEnd.m_iDay = Convert.ToUInt16(dtEndTime.Value.Day);
            //tInfo.m_tTimeEnd.m_iHour = Convert.ToUInt16(dtEndTime.Value.Hour);
            //tInfo.m_tTimeEnd.m_iMinute = Convert.ToUInt16(dtEndTime.Value.Minute);
            //tInfo.m_tTimeEnd.m_iSecond = Convert.ToUInt16(dtEndTime.Value.Second);
            //tInfo.m_cLocalFilename = new char[255];

            //DOWNLOAD_TIMESPAN tInfo=StructFactory.CreateDownLoadTimeSpan(cboChanList.SelectedIndex, cboStreamNo.SelectedIndex,
            //    cboFileSaveFlag.SelectedIndex, cboTimeDataMode.SelectedIndex, dtStartTime.Value, dtEndTime.Value);
            Log += string.Format("m_iLogonId:" + m_iLogonId);
            downloader = new Downloader(m_iLogonId);
            downloader.Init(textIP.Text, cboChanList.SelectedIndex, cboStreamNo.SelectedIndex, dtStartTime.Value, dtEndTime.Value,  
                cboFileSaveFlag.SelectedIndex, cboTimeDataMode.SelectedIndex, cboTimeSaveFormat.SelectedIndex);
            //var tInfo = downloader.tInfo;

            //string strFileName = "Download";
            //strFileName += dtStartTime.Value.Year.ToString() + dtStartTime.Value.Month.ToString("D2") + dtStartTime.Value.Day.ToString("D2") +
            //               dtStartTime.Value.Hour.ToString("D2") + dtStartTime.Value.Second.ToString("D2") + dtStartTime.Value.Minute.ToString("D2");
            //strFileName += "-";
            //strFileName += dtEndTime.Value.Year.ToString() + dtEndTime.Value.Month.ToString("D2") + dtEndTime.Value.Day.ToString("D2") +
            //               dtEndTime.Value.Hour.ToString("D2") + dtEndTime.Value.Second.ToString("D2") + dtEndTime.Value.Minute.ToString("D2");

            //int iSaveFormat = cboTimeSaveFormat.SelectedIndex;
            //if (0 == iSaveFormat)       //sdv格式
            //{
            //    tInfo.m_iSaveFileType = NetSDKCmd.DOWNLOAD_FILE_TYPE_SDV;
            //    strFileName += ".sdv";
            //}
            //else if (1 == iSaveFormat)  //PS(MP4格式)
            //{
            //    tInfo.m_iSaveFileType = NetSDKCmd.DOWNLOAD_FILE_TYPE_PS;
            //    strFileName += ".ps";
            //}           
            //Array.Copy(strFileName.ToCharArray(), tInfo.m_cLocalFilename, strFileName.Length);

            //IntPtr intptr = Marshal.AllocCoTaskMem(tInfo.m_iSize);
            //Marshal.StructureToPtr(tInfo, intptr, true);
            //int iRet = NVSSDK.NetClient_NetFileDownload(ref m_iDLTimeId, m_iLogonId, NetSDKCmd.DOWNLOAD_CMD_TIMESPAN, intptr, tInfo.m_iSize);
            //Marshal.FreeHGlobal(intptr);
            //if (iRet < 0)
            //{
            //    MessageBoxShow("Download file failed, ret=!" + iRet);
            //    return;
            //}
            //m_iDLStartTime = CommonFunc.NvsFileTimeToAbsSeconds(tInfo.m_tTimeBegin);
            //m_iDLStopTime = CommonFunc.NvsFileTimeToAbsSeconds(tInfo.m_tTimeEnd);
            //timerDLTimePos.Enabled = true;    //开启定时器获取下载进度

            var r = downloader.Start();
            if (r == false)
            {
                MessageBoxShow("Download file failed, ret=!" + downloader.iRet);
                return false;
            }
            else
            {
                timerDLTimePos.Enabled = true;
                return true;
            }
        }

        public Downloader downloader;

        //按时间：停止下载
        private void btnDLTimeStop_Click(object sender, EventArgs e)
        {
            DLTime_Stop();
        }

        private void PlayBackForm_Load(object sender, EventArgs e)
        {
            if (AutoLogin)
            {
                LoginSwitch();
            }
        }

        public bool AutoLogin = false;

        private void button1_Click(object sender, EventArgs e)
        {
            ClearLog();
        }

        private void cboStreamNo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
