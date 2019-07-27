using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;

namespace NetClient
{
    public partial class ClientForm : BaseForm
    {
        //public IntPtr hwndReplayfrm = IntPtr.Zero; 
        //public ReplayForm Replayfrm = null;

        private const int T_AUDIO8 = 0;
        private const int T_YUV420 = 1;
        private const int T_YUV422 = 2;

        private RECVDATA_NOTIFY RecvDataNotify = null;
        private COMRECV_NOTIFY ComRecvNotify = null;
        private DECYUV_NOTIFY DecYuvNotify = null;

        private MAIN_NOTIFY_V4 MainNotify_V40 = null;

        private ALARM_NOTIFY_V4 AlarmNotify_V40 = null;

        private static FileStream fsSdv = null;
        private static FileStream fsYuv = null;
        private static FileStream fsPcm = null;

        private const int FTP_CMD_SET_SNAPSHOT = 0;
        private const int FTP_CMD_GET_SNAPSHOT = 4;

        private string strContinuousSnapPath;
        private int m_iSnapCount = 0;
        private System.Timers.Timer tTimer;
     
        //视频窗口数组
        VideoWindow[] m_video;

        //视频窗口对应的连接状态结构体数组
        CONNECT_STATE[] m_conState;

        //当前登录状态结构体
        CLIENTINFO m_cltInfo;

        //当前视频窗口标记，从0开始
        int m_iCurrentFrame = 0;

        //可显示的最大视频窗口数
        const int CONST_iFrameNum = 16;

        //双击次数，用于多屏与单屏，单屏与全屏，返回原来状态之间切换
        //多窗口0 单窗口1 全屏2
        int m_iDBClick = 0;

        //双击DSM网络视频服务器
        bool m_blNSClick = false;

        //声明DSMInfo窗体
        FormDSM formDSM = null;
        public ClientForm()
        {
            InitializeComponent();
            m_cltInfo.m_iServerID = -1;
            CheckForIllegalCrossThreadCalls = false;
            StartUp();            
        }


        //启动SDK并初始化
        private void StartUp()
        {
            //设置客户端和主控端所用的默认网络端口
            NVSSDK.NetClient_SetPort(3000, 6000);

            //设置消息通知ID
            NVSSDK.NetClient_SetMSGHandle(SDKConstMsg.WM_MAIN_MESSAGE, this.Handle, SDKConstMsg.MSG_PARACHG, SDKConstMsg.MSG_ALARM);

            //启动SDK
            NVSSDK.NetClient_Startup();

            //初始化NSLook库
            NVSSDK.NSLook_Startup();

            // 设置登陆成功回调
            MainNotify_V40 = MyMAIN_NOTIFY_V4;
            AlarmNotify_V40 = MyAlarm_NOTIFY_V4;
            NVSSDK.NetClient_SetNotifyFunction_V4(MainNotify_V40, AlarmNotify_V40, null, null, null);

            //创建视频窗口对象
            m_conState = new CONNECT_STATE[16];
            m_video = new VideoWindow[16];
            for (int i = 0; i < 16; i++)
            {
                //初始化连接状态结构体
                m_conState[i].m_iChannelNO = -1;
                m_conState[i].m_iLogonID = -1;
                m_conState[i].m_uiConID = UInt32.MaxValue;

                //修改视频窗口属性并注册单击、双击事件
                m_video[i] = new VideoWindow();                
                m_video[i].Hide();
                m_video[i].pnlVideo.TabIndex = i;
                m_video[i].pnlVideo.Click += new EventHandler(Video_Click);
                m_video[i].pnlVideo.DoubleClick += new EventHandler(Video_DBClick);
            }
            //将视频窗口添加的主窗体上
            this.Controls.AddRange(m_video);
            cboChannel.SelectedIndex = 0;
            cboMode.SelectedIndex = 0;
            cboScreen.SelectedIndex = 1;
            cboStream.SelectedIndex = 0;

            //默认显示4屏
            DisplayWindows(2);

            //设置连接到注册中心的初始ID
            btnDSMLogon.Tag = -1;
        }

        //显示_iRows行视频窗口
        private void DisplayWindows(int _iRows)
        {
            //主窗体的客户区高度
            int iHeight =( ClientSize.Height - 100)/ _iRows;

            //四比三的高度
            int iWidth = iHeight*4/3;

            //隐藏各视频窗口
            for (int i = _iRows * _iRows; i < 16; i++)
            {
                m_video[i].Hide();
            }

            //显示并调整前_iRows*_iRows个视频窗口
            for (int i = 0; i < _iRows; i++)
            {
                for (int j = 0; j < _iRows; j++)
                {
                    m_video[i * _iRows + j].Left = j * iWidth;
                    m_video[i * _iRows + j].Top = i * iHeight;
                    m_video[i * _iRows + j].Width = iWidth;
                    m_video[i * _iRows + j].Height = iHeight;
                    m_video[i * _iRows + j].Show();
                }
            }
            //如果只显示一屏，修改双击次数
            m_iDBClick = _iRows == 1 ? 1 : 0;
        }

        //重写消息处理函数，以处理自定义消息
        protected override void DefWndProc(ref System.Windows.Forms.Message m)
        {
            //WM_MAIN_MESSAGE为自定义的系统消息
            if (m.Msg == SDKConstMsg.WM_MAIN_MESSAGE)
            {
                //自定义消息处理函数
                //OnMainMessage(m.WParam, m.LParam);
                this.Notify(m.WParam, m.LParam);
            }

            //默认消息处理函数
            base.DefWndProc(ref m);
        }
        public override void OnMessagePro(IntPtr wParam, IntPtr lParam)
        {
            //wParam的低16位是消息的类型；
            int iMsgType = wParam.ToInt32() & 0xFFFF;
            //lParam，网络视频服务器NVS的信息结构体NVS_IPAndID地址
            //Marshal.PtrToStructure函数将Intptr地址转化为结构体
            //NVS_IPAndID  ipAndID = (NVS_IPAndID)Marshal.PtrToStructure(lParam, typeof(NVS_IPAndID));

            switch (iMsgType)
            {
                //登陆状态消息 
                //param1 登陆IP
                //param2 登陆ID
                //param3 登陆状态
                case 29:
                    {
                        MessageBox.Show(" Download interrupt");
                        break;
                    }
                case SDKConstMsg.WCM_LOGON_NOTIFY:
                    {
                        NVS_IPAndID ipAndID = (NVS_IPAndID)Marshal.PtrToStructure(lParam, typeof(NVS_IPAndID));

                        int i = wParam.ToInt32();
                        LogonNotify(ipAndID.m_pIP.ToCharArray(), ipAndID.m_pID, wParam.ToInt32() >> 16);
                        break;
                    }


                //视频头消息，当收到视频头时产生。
                //lParam，网络视频服务器NVS的信息结构体NVS_IPAndID地址；
                //wParamHi低8位表示通道号；
                //wParamHi高8位表示码流类型；
                case SDKConstMsg.WCM_VIDEO_HEAD:
                    VideoArrive();
                    break;

                //视频被强制断开消息，当前的视频连接被代理强制断开后产生该消息。
                //param1,视频连接ID号
                case SDKConstMsg.WCM_VIDEO_DISCONNECT:
                    VideoDisconnect((UInt32)lParam.ToInt32());
                    break;

                //网络命令断开消息，当网络连接意外断开时产生。
                //param1，网络视频服务器的IP地址；
                case SDKConstMsg.WCM_ERR_ORDER:
                    {
                        NVS_IPAndID ipAndID = (NVS_IPAndID)Marshal.PtrToStructure(lParam, typeof(NVS_IPAndID));

                        NetDisconnect(ipAndID.m_pIP);
                        break;
                    }


                //网络数据错误，当连接超过最大数后将产生此消息。
                //param1，网络视频服务器的IP地址；
                case SDKConstMsg.WCM_ERR_DATANET:
                    {
                        NVS_IPAndID ipAndID = (NVS_IPAndID)Marshal.PtrToStructure(lParam, typeof(NVS_IPAndID));

                        NetDataError(ipAndID.m_pIP);
                        break;
                    }

                //录像错误消息，当视频录像出现错误时产生。
                //param1，视频连接ID号
                case SDKConstMsg.WCM_RECORD_ERR:
                    RecordError((UInt32)lParam.ToInt32());
                    break;

                default:
                    break;
            }
        }
        //处理SDK系统消息
        //private void OnMainMessage(IntPtr wParam, IntPtr lParam)
        //{
        //    //wParam的低16位是消息的类型；
        //    int iMsgType = wParam.ToInt32() & 0xFFFF;
        //    //lParam，网络视频服务器NVS的信息结构体NVS_IPAndID地址
        //    //Marshal.PtrToStructure函数将Intptr地址转化为结构体
        //    //NVS_IPAndID  ipAndID = (NVS_IPAndID)Marshal.PtrToStructure(lParam, typeof(NVS_IPAndID));


            
        //    switch (iMsgType)
        //    {
        //        //登陆状态消息 
        //        //param1 登陆IP
        //        //param2 登陆ID
        //        //param3 登陆状态
        //        case 29:
        //            {
        //                MessageBox.Show(" Download interrupt");
        //                break;
        //            }
        //        case WCM_LOGON_NOTIFY:
        //            {
        //                NVS_IPAndID ipAndID = (NVS_IPAndID)Marshal.PtrToStructure(lParam, typeof(NVS_IPAndID));

        //                LogonNotify(ipAndID.m_pIP.ToCharArray(), ipAndID.m_pID, wParam.ToInt32() >> 16);
        //                break;
        //            }
                    

        //        //视频头消息，当收到视频头时产生。
        //        //lParam，网络视频服务器NVS的信息结构体NVS_IPAndID地址；
        //        //wParamHi低8位表示通道号；
        //        //wParamHi高8位表示码流类型；
        //        case WCM_VIDEO_HEAD:
        //            VideoArrive();
        //            break;

        //        //视频被强制断开消息，当前的视频连接被代理强制断开后产生该消息。
        //        //param1,视频连接ID号
        //        case WCM_VIDEO_DISCONNECT:
        //            VideoDisconnect((UInt32)lParam.ToInt32());
        //            break;                                   

        //        //网络命令断开消息，当网络连接意外断开时产生。
        //        //param1，网络视频服务器的IP地址；
        //        case WCM_ERR_ORDER:
        //            {
        //                NVS_IPAndID ipAndID = (NVS_IPAndID)Marshal.PtrToStructure(lParam, typeof(NVS_IPAndID));

        //                NetDisconnect(ipAndID.m_pIP);
        //                break;
        //            }
                    

        //        //网络数据错误，当连接超过最大数后将产生此消息。
        //        //param1，网络视频服务器的IP地址；
        //        case WCM_ERR_DATANET:
        //            {
        //                NVS_IPAndID ipAndID = (NVS_IPAndID)Marshal.PtrToStructure(lParam, typeof(NVS_IPAndID));

        //                NetDataError(ipAndID.m_pIP);
        //                break;
        //            }

        //        //录像错误消息，当视频录像出现错误时产生。
        //        //param1，视频连接ID号
        //        case WCM_RECORD_ERR:
        //            RecordError((UInt32)lParam.ToInt32());
        //            break;

        //        default:
        //            break;
        //    }
        //    //if (this.MdiChildren.Length > 0)
        //    //{
        //    //    this.MdiChildren[0].Refresh();
        //    //}
            
        //}

        //WCM_LOGON_NOTIFY消息处理函数
        private void LogonNotify(char[] _cIP,string _strID,int iLogonState)
        {
            //iLogonState 登陆状态
            switch (iLogonState)
            {
                case SDKConstMsg.LOGON_SUCCESS://登陆成功显示设备ID号
                    {
                        m_cltInfo.m_cRemoteIP = _cIP;
                        textID.Text = _strID;
                        btnLogon.Text = "Logoff";

                        //双击DSM中的网络视频服务器后，直接连接视频
                        if (m_blNSClick)
                        {
                            UInt32 uiConID = m_conState[m_iCurrentFrame].m_uiConID;
                            if (m_conState[m_iCurrentFrame].m_uiConID == UInt32.MaxValue)
                            {
                                cboChannel.SelectedIndex = 0;
                                cboStream.SelectedIndex = 0;
                                btnConnect_Click(btnConnect, EventArgs.Empty);
                            }
                            m_blNSClick = false;
                        }
                        break;
                    }
                case SDKConstMsg.LOGON_FAILED:
                case SDKConstMsg.LOGON_ING:
                case SDKConstMsg.LOGON_RETRY:
                case SDKConstMsg.NOT_LOGON:
                case SDKConstMsg.LOGON_TIMEOUT://登陆失败
                    {
                        m_cltInfo.m_iServerID = -1;
                        textID.Text = "";
                        MessageBox.Show("Logon failed!");
                        btnLogon.Text = "Logon";
                        break;
                    }
            }
        }

        //WCM_VIDEO_HEAD消息处理函数
        private void VideoArrive()
        {
            RECT rect = new RECT();
            
            //视频到达后开始播放   
	        NVSSDK.NetClient_StartPlay( m_conState[m_iCurrentFrame].m_uiConID,m_video[m_iCurrentFrame].pnlVideo.Handle, rect, 0);
            btnPlay.Text = "Stop";

            //修改视频状态信息
            GetWindowStates();
        }

        //WCM_VIDEO_DISCONNECT消息处理函数
        private void VideoDisconnect(UInt32 _uiConID)
        {
            bool isCurrentFrame = false;
            for (int i = 0; i < CONST_iFrameNum; i++)
            {
                //视频被强制断开后，刷新对应窗口显示
                if (m_conState[i].m_uiConID == _uiConID)
                {
                    //停止一路视频接收
                    NVSSDK.NetClient_StopRecv(_uiConID);
                    m_conState[m_iCurrentFrame].m_iChannelNO = -1;
                    m_conState[m_iCurrentFrame].m_uiConID = UInt32.MaxValue;                    
                    m_video[m_iCurrentFrame].Invalidate(true);
                    if (i == m_iCurrentFrame)
                    {
                        isCurrentFrame = true;
                    }
                }
            }

            //如果是当前选中窗口，则更新其状态显示信息
            if (isCurrentFrame == true)
            {
                GetWindowStates();
            }	
        }

        //WCM_ERR_ORDER消息处理函数
        private void NetDisconnect(string _strIP)
        {
            string strMSG = "连接到网络视频服务器";
            strMSG += _strIP;
            strMSG += "的网络意外断开！";
            MessageBox.Show(strMSG);
        }

        //WCM_ERR_DATANET消息处理函数
        private void NetDataError(string _strIP)
        {
            string strMSG = "网络视频服务器";
            strMSG += _strIP;
            strMSG += "的连接数达到最大！";
            MessageBox.Show(strMSG);
        }

        //WCM_RECORD_ERR消息处理函数
        private void RecordError(UInt32 _uiConID)
        {
            bool isCurrentFrame = false;
            //连接ID为_uiCon的窗口停止录像
            for (int i = 0; i < CONST_iFrameNum; i++)
            {
                if (m_conState[i].m_uiConID == _uiConID)
                {
                    //停止将收到的数据写入文件
                    NVSSDK.NetClient_StopCaptureFile(_uiConID);
                    if (i == m_iCurrentFrame)
                    {
                        isCurrentFrame = true;
                    }
                }
            }
            //如果当前窗口录像错误，则更新录像按钮的Caption为Record
            if (isCurrentFrame == true)
            {
                btnRecord.Text = "Record";
            }
            MessageBox.Show("Record error !");	
        }

        //清空窗口对应的状态信息
        private void InitWindowStates()
        {
            btnLogon.Text = "Logon";
            btnConnect.Text = "Connect";
            btnPlay.Text = "Play";
            btnRecord.Text = "Record";
            btnMoveAuto.Text = "Auto";
            cboOSDEnable.SelectedIndex = -1;
            cboOSDType.SelectedIndex = -1;
            cboOSDX.SelectedIndex = -1;
            cboOSDY.SelectedIndex = -1;

            //设置默认串口个数为2
            if (cboComNo.Items.Count > 2)
            {
                for (int i = cboComNo.Items.Count-1; i> 1; i--)
                {
                    cboComNo.Items.RemoveAt(i);
                    cboComSend.Items.RemoveAt(i);
                }                
            }
            textOSD.Text = "";
            cboDeviceType.SelectedIndex = -1;
            cboWorkMode.SelectedIndex = -1;
            cboComNo.SelectedIndex = -1;
            textComFormat.Text = "";
            textAddress.Text = "";

            //当前视频窗口没有登陆，清空视频参数
            if (m_conState[m_iCurrentFrame].m_iLogonID < 0)
            {
                trckBrightness.Value = 0;
                trckContrast.Value = 0;
                trckHue.Value = 0;
                trckSaturation.Value = 0;
            }
        }

        //设置窗口对应的状态信息
        private void GetWindowStates()
        {
            btnLogon.Text = "Logoff";
            btnConnect.Text = "Disconnect";
            btnPlay.Text = "Stop";
            UInt32 uiConID = m_conState[m_iCurrentFrame].m_uiConID;

            //正在录像
            if (NVSSDK.NetClient_GetCaptureStatus(uiConID) == 1)
            {
                btnRecord.Text = "Stop";
            }
            Int32 iLogonID = m_conState[m_iCurrentFrame].m_iLogonID;
            Int32 iComPortCounts = 2;
            Int32 iComPortEnabledStatus = 0;

            //获得前端设备的串口号个数
            NVSSDK.NetClient_GetComPortCounts(iLogonID, ref iComPortCounts,ref iComPortEnabledStatus);

            //添加串口以达到前端设备支持的串口个数
            if (cboComNo.Items.Count < iComPortCounts)
            {
                for(int i = cboComNo.Items.Count; i < iComPortCounts; i++)
                {
                    cboComNo.Items.Add("COM" + (i + 1));
                    cboComSend.Items.Add("COM" + (i + 1));
                }
            }
            //修改字符叠加字符串、字符叠加类型、设备类型、视频参数的状态
            GetOSD();
            GetOSDType();
            GetDeviceType();
            GetVideoParam();
            //GetFTPUploadConfig();//new add
        }

        //更新窗口对应的状态信息
        private void SetWindowStates()
        {
            //正在播放时，设置窗口对应的状态信息；否则，清空窗口对应的状态信息
            if (NVSSDK.NetClient_GetPlayingStatus(m_conState[m_iCurrentFrame].m_uiConID) == SDKConstMsg.PLAYER_PLAYING)
            {
                GetWindowStates();
            }
            else
            {
                InitWindowStates();
            }
        }

        //单击视频显示窗口
        private void Video_Click(object sender, EventArgs e)
        {
            Panel pane = (Panel)sender;

            //修改以前视频窗口的边框
            m_video[m_iCurrentFrame].picVideo.BackColor = SystemColors.Control; 
           
            //修改当前视频窗口标记
            m_iCurrentFrame = pane.TabIndex;

            //修改通道号
            cboChannel.SelectedIndex = m_conState[m_iCurrentFrame].m_iChannelNO >= 0 ? m_conState[m_iCurrentFrame].m_iChannelNO : m_iCurrentFrame;
            
            //为当前视频窗口添加红色边框
            m_video[m_iCurrentFrame].picVideo.BackColor = Color.Red;

            //更新当前视频窗口的状态
            SetWindowStates();
        }

        //双击视频显示窗口
        private void Video_DBClick(object sender, EventArgs e)
        {
            //如果当前窗口没有播放视频，退出
            if (NVSSDK.NetClient_GetPlayingStatus(m_conState[m_iCurrentFrame].m_uiConID) != SDKConstMsg.PLAYER_PLAYING)
            {
                return;
            }
            
            if (m_iDBClick == 0)//多屏转单屏
            {
                //隐藏非当前视频窗口
                for (int i = 0; i <= cboScreen.SelectedIndex; i++)
                {
                    if (i != m_iCurrentFrame)
                    {
                        m_video[i].Hide();
                    }                    
                }

                //调整当前视频窗口的位置和大小
                m_video[m_iCurrentFrame].Left = ClientRectangle.Left;
                m_video[m_iCurrentFrame].Top = ClientRectangle.Top;                
                m_video[m_iCurrentFrame].Height = ClientRectangle.Height;
                m_video[m_iCurrentFrame].Width = m_video[m_iCurrentFrame].Height*4/3;

                //修改双击次数
                m_iDBClick = 1;
            }
            else if (m_iDBClick == 1)//单屏转全屏
            {
                //获得显示器的分辨率
                Rectangle rect = Screen.PrimaryScreen.Bounds;
                this.SetVisibleCore(false);

                //去边框,窗口最大化，要保顺序才能实现全屏
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized; 

                //放大屏幕，隐藏红色边框
                m_video[m_iCurrentFrame].Left = -3;
                m_video[m_iCurrentFrame].Top = -3;
                m_video[m_iCurrentFrame].Height = rect.Height+6;
                m_video[m_iCurrentFrame].Width = rect.Width+6;

                //设置当前视频窗口的Z顺序为0
                m_video[m_iCurrentFrame].BringToFront();
                this.SetVisibleCore(true);

                //修改双击次数
                m_iDBClick = 2;                
            }
            else//全屏转多屏
            {
                //先将窗口恢复正常，再去边框；否则，会出现窗口逐渐变大或变小的现象
                this.WindowState = FormWindowState.Normal;
                this.FormBorderStyle = FormBorderStyle.Sizable;

                //显示cboScreen.SelectedIndex + 1屏，恢复第一次双击时的窗口状态
                DisplayWindows(cboScreen.SelectedIndex + 1);
            }
        }

        //登陆和注销
        private void btnLogon_Click(object sender, EventArgs e)
        {
            if (btnLogon.Text == "Logon")//登陆
            {
                string strProxy = "";
                string strIP = cboIP.Text;
                string strUser = textUser.Text;
                string strPwd = textPwd.Text;
                string strProxyID = "";
                int iPort = 3000;
                int iRet;

                //登录指定的网络视频服务器
                iRet = NVSSDK.NetClient_Logon(strProxy, strIP, strUser, strPwd, strProxyID, iPort);
                if (iRet < 0)
                {
                    m_cltInfo.m_iServerID = -1;
                    MessageBox.Show("Logon failed !");
                    return;
                }
                m_cltInfo.m_iServerID = iRet;
                btnLogon.Text = "Logoff";
            }
            else //注销
            {
                btnLogon.Text = "Logon";
                int iLogonID = m_conState[m_iCurrentFrame].m_iLogonID;
                if (iLogonID < 0)//如果当前窗口没有登陆，不进行操作
                {
                    return;
                }

                //注销当前窗口对应的用户登录
                NVSSDK.NetClient_Logoff(iLogonID);	
                if (m_cltInfo.m_iServerID == iLogonID)
                {
                    textID.Text = "";
                }
                //更新对应的窗口信息
                for (int i = 0; i < CONST_iFrameNum; i++)
                {
                    if (m_conState[i].m_iLogonID == iLogonID)
                    {
                        m_conState[i].m_iLogonID = -1;
                        m_conState[i].m_iChannelNO = -1;
                        m_conState[i].m_uiConID = UInt32.MaxValue;
                        m_video[i].Invalidate(true);
                    }
                }
                //清空当前视频窗口的状态信息
                InitWindowStates();                
            }
        }

        //连接和断开
        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (btnConnect.Text == "Connect")//连接操作
            {
                m_cltInfo.m_iChannelNo = cboChannel.SelectedIndex;
                m_cltInfo.m_iNetMode = cboMode.SelectedIndex + 1;
                m_cltInfo.m_iStreamNO = cboStream.SelectedIndex;

                m_cltInfo.m_cNetFile = new char[255];
                m_cltInfo.m_cRemoteIP = new char[16];
                Array.Copy(cboIP.Text.ToCharArray(), m_cltInfo.m_cRemoteIP, cboIP.Text.Length);
                UInt32 uiConID = m_conState[m_iCurrentFrame].m_uiConID;

                //获得当前窗口对应的视频播放状态
                int iRet = NVSSDK.NetClient_GetPlayingStatus(uiConID);

                //如果正在播放视频，不进行连接操作
                if (iRet != SDKConstMsg.PLAYER_PLAYING)
                {
                    int iChannelNum = 0;

                    //获得当前窗口连接的网络视频服务器最大通道数
                    NVSSDK.NetClient_GetChannelNum(m_cltInfo.m_iServerID, ref iChannelNum);

                    //判断是否超过最大通道号
                    if (m_cltInfo.m_iChannelNo >= iChannelNum)
                    {
                        MessageBox.Show("Max Channel is " + iChannelNum);
                        cboChannel.SelectedIndex = iChannelNum - 1;
                        return;
                    }
                    //开始接收一路视频数据	
                    iRet = NVSSDK.NetClient_StartRecv(ref uiConID, ref m_cltInfo, null);

                    //操作失败，清除结构体m_conState的信息
                    if (iRet < 0)
                    {
                        m_conState[m_iCurrentFrame].m_iLogonID = -1;
                        m_conState[m_iCurrentFrame].m_uiConID = UInt32.MaxValue;
                        m_conState[m_iCurrentFrame].m_iChannelNO = -1;
                        MessageBox.Show("Connect failed !");                        
                        return;
                    }
                    //操作成功，更新结构体m_conState的信息
                    m_conState[m_iCurrentFrame].m_iLogonID = m_cltInfo.m_iServerID;
                    m_conState[m_iCurrentFrame].m_iChannelNO = m_cltInfo.m_iChannelNo;
                    m_conState[m_iCurrentFrame].m_uiConID = uiConID;
                    m_conState[m_iCurrentFrame].m_iStreamNO = m_cltInfo.m_iStreamNO;

                    //开始导出收到的数据
                    NVSSDK.NetClient_StartCaptureData(uiConID);
                    if (iRet == 1)
                    {
                        RECT rect = new RECT();

                        //开始播放某路视频
                        NVSSDK.NetClient_StartPlay(uiConID, m_video[m_iCurrentFrame].pnlVideo.Handle, rect, 0);
                        btnPlay.Text = "Stop";
                        GetWindowStates();
                    }
                    btnConnect.Text = "Disconnect";
                }
            }
            else //断开操作
            {
                NVSSDK.NetClient_StopRecv(m_conState[m_iCurrentFrame].m_uiConID);//停止一路视频接收
                m_conState[m_iCurrentFrame].m_iChannelNO = -1;//修改当前窗口的通道号和连接ID
                m_conState[m_iCurrentFrame].m_uiConID = UInt32.MaxValue;
                m_video[m_iCurrentFrame].Invalidate(true);//刷新当前窗口，并更新其状态信息
                btnConnect.Text = "Connect";

                //清空当前视频窗口的状态信息
                InitWindowStates();
                btnLogon.Text = "Logoff";
            }
               
        }

        //显示视频和停止播放
        private void btnPlay_Click(object sender, EventArgs e)
        {
            //当前窗口没有连接，退出
            if (m_conState[m_iCurrentFrame].m_uiConID == UInt32.MaxValue)
            {
                return;
            }
            string strCaption = btnPlay.Text;
            int iRet;
            if (strCaption == "Play") //显示视频
            {
                RECT rect = new RECT();

                //开始播放视频
                iRet = NVSSDK.NetClient_StartPlay
                (
                    m_conState[m_iCurrentFrame].m_uiConID,
                    m_video[m_iCurrentFrame].pnlVideo.Handle,
                    rect,
                    0
                );
                if (iRet == 0)
                {
                    btnPlay.Text = "Stop";
                }
            }
            else //停止播放
            {
                //停止接受视频数据
                iRet = NVSSDK.NetClient_StopCaptureData(m_conState[m_iCurrentFrame].m_uiConID);

                //停止播放某路视频
                iRet = NVSSDK.NetClient_StopPlay(m_conState[m_iCurrentFrame].m_uiConID);
                m_video[m_iCurrentFrame].Invalidate(true);
                btnPlay.Text = "Play";
            }
        }

        //更改视频显示画面个数
        private void cboScreen_SelectedIndexChanged(object sender, EventArgs e)
        {            
            DisplayWindows(cboScreen.SelectedIndex + 1);
        }        

        //录像
        private void btnRecord_Click(object sender, EventArgs e)
        {
            UInt32 uiConID = m_conState[m_iCurrentFrame].m_uiConID;

            //只对有连接的窗口进行录像操作
            if (uiConID == UInt32.MaxValue)
            {
                return;
            }
            string strCaption = btnRecord.Text;
            if (strCaption == "Record")//没有录像，开始录像
            {
                dlgSaveFile.Filter = "(*.sdv)|*.sdv";

                //显示文件保存对话框
                if(dlgSaveFile.ShowDialog() == DialogResult.OK)
                {
                    string strFileName = dlgSaveFile.FileName;

                    //开始将收到的数据写入文件
                    int iRet = NVSSDK.NetClient_StartCaptureFile(uiConID, strFileName,0);
                    if (iRet == 0)
                    {
                        btnRecord.Text = "Stop";
                    }
                }
            }
            else //正在录像
            {
                //停止将收到的数据写入文件
                NVSSDK.NetClient_StopCaptureFile(uiConID);
                btnRecord.Text = "Record";
            }
        }

        //抓图操作
        private void btnCapPic_Click(object sender, EventArgs e)
        {
            UInt32 uiConID = m_conState[m_iCurrentFrame].m_uiConID;

            //只对有连接的窗口进行录像操作
            if (uiConID == UInt32.MaxValue)
            {
                return;
            }
            dlgSaveFile.Filter = "(*.bmp)|*.bmp";

            //显示文件保存对话框
            if (dlgSaveFile.ShowDialog() == DialogResult.OK)
            {
                string strFileName = dlgSaveFile.FileName;

                //开始将收到的数据写入文件    
                NVSSDK.NetClient_CaptureBmpPic(uiConID, strFileName);           
            }
        }

        //获得并显示字符叠加参数
        private void GetOSD()
        {
            byte[] btOSD = new byte[32];
            UInt32 uiColor = 0;

            //获得视频源上叠加的字符串
            NVSSDK.NetClient_GetOsdText
            (
                m_conState[m_iCurrentFrame].m_iLogonID,
                m_conState[m_iCurrentFrame].m_iChannelNO,
                btOSD,
                ref uiColor
            );

            //将byte型数组转化为字符串
            textOSD.Text =Encoding.ASCII.GetString(btOSD);
        }

        //设置字符叠加参数
        private void SetOSD()
        {
            string strOSD = textOSD.Text;

            //判断是否为空字符串
            strOSD = strOSD == "" ? " " : strOSD;
            UInt32 uiColor = 0;

            //在视频源上叠加一个字符串
            NVSSDK.NetClient_SetOsdText
            (
                m_conState[m_iCurrentFrame].m_iLogonID,
                m_conState[m_iCurrentFrame].m_iChannelNO,
                Encoding.ASCII.GetBytes(strOSD),//将字符串转化为byte型数组
                uiColor
            );
            textOSD.Text = strOSD;
        }

        //获取字符叠加状态
        private void GetOSDType()
        {
            Int32 iType = cboOSDType.SelectedIndex;

            //转化为字符叠加类型码,0x01 叠加时间,0x02 叠加字符串,0x04 叠加LOGO标志 
            switch(iType)
            {
                case 0:
                    iType = 0x01;
                    break;
                case 1:
                    iType = 0x02;
                    break;
                case 2:
                    iType = 0x04;
                    break;
                default :
                    iType = 0x02;
                    cboOSDType.SelectedIndex = 1;
                    break;
            }
            int iX = 0;
            int iY = 0;
            int iEnable = 0;


            //获得网络视频服务器某一路的字符叠加状态
            NVSSDK.NetClient_GetOsdType
            (
                m_conState[m_iCurrentFrame].m_iLogonID,
                m_conState[m_iCurrentFrame].m_iChannelNO,
                iType,
                ref iX,
                ref iY,
                ref iEnable
            );
            cboOSDX.Text = iX.ToString();
            cboOSDY.Text = iY.ToString();
            cboOSDEnable.SelectedIndex = iEnable;           
        }

        //设置字符叠加状态
        private void SetOSDType()
        {
            Int32 iType = cboOSDType.SelectedIndex;

            //转化为字符叠加类型码,0x01 叠加时间,0x02 叠加字符串,0x04 叠加LOGO标志 
            switch (iType)
            {
                case 0:
                    iType = 0x01;
                    break;
                case 2:
                    iType = 0x04;
                    break;
                default:
                    iType = 0x02;
                    break;
            }
            int iX = 0;
            int iY = 0;
            int iEnable = 0;
            try
            {
                iX = Int32.Parse(cboOSDX.Text);
                iY = Int32.Parse(cboOSDY.Text);
                iEnable = cboOSDEnable.SelectedIndex;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            //开始或停止字符叠加操作，同时指定字符叠加的位置。
            int iRet = NVSSDK.NetClient_SetOsdType
            (
                m_conState[m_iCurrentFrame].m_iLogonID,
                m_conState[m_iCurrentFrame].m_iChannelNO,               
                iX,
                iY,
                iType,
                iEnable
            );
            if (iRet < 0)
            {
                MessageBox.Show("NetClient_SetOsdType Failed! USER_ERROR+" + (Marshal.GetLastWin32Error() - SDKConstMsg.USER_ERROR));
            }
        }

        //修改字符叠加信息
        private void btnOSDSet_Click(object sender, EventArgs e)
        {
            SetOSDType();
            SetOSD();
        }

        //获得并显示设备类型信息
        private void GetDeviceType()
        {
            int iCom = 0;
            int iDevAddress = 0;
            StringBuilder strDevType = new StringBuilder();
            StringBuilder strComFormat = new StringBuilder();
            int iWorkMode = 0;
            int iRet;

            //获得控制设备类型和串口属性设置
            iRet = NVSSDK.NetClient_GetDeviceType
            (
                m_conState[m_iCurrentFrame].m_iLogonID,
                m_conState[m_iCurrentFrame].m_iChannelNO,
                ref iCom,
                ref iDevAddress,
                strDevType
            );
            if (iRet < 0)
            {
                MessageBox.Show("NetClient_GetDeviceType Failed!" + (Marshal.GetLastWin32Error() - 0x10000000).ToString());
            }

            //串口号从1开始
            if (iCom < 1)
            {
                iCom = 1;
            }
            //获得串口属性
            iRet = NVSSDK.NetClient_GetComFormat
            (
                m_conState[m_iCurrentFrame].m_iLogonID,
                iCom,
                strComFormat,
                ref iWorkMode
            );
            if (iRet < 0)
            {
                MessageBox.Show("NetClient_GetComFormat Failed! USER_ERROR+" + (Marshal.GetLastWin32Error() - SDKConstMsg.USER_ERROR));
            }

            //串口工作方式　1：协议　2：透明通道
            if (iWorkMode < 1)
            {
                iWorkMode = 1;
            }		
            cboComNo.SelectedIndex = iCom - 1;
            cboComSend.SelectedIndex = iCom - 1;
            textAddress.Text = iDevAddress.ToString();
            cboDeviceType.Text = strDevType.ToString();
            textComFormat.Text = strComFormat.ToString();
            cboWorkMode.SelectedIndex = iWorkMode - 1;            
        }

        //修改设备信息
        private void SetDeviceType()
        {
            int iCom = cboComNo.SelectedIndex+1;
            int iDevAddress = 0;
            byte[] btDevType = Encoding.ASCII.GetBytes(cboDeviceType.Text);
            byte[] btComFormat = Encoding.ASCII.GetBytes(textComFormat.Text);
            int iWorkMode = cboWorkMode.SelectedIndex+1;
            try
            {
                //进行类型转换
                iDevAddress = Int32.Parse(textAddress.Text);
            }
            catch (System.Exception ex)
            {
            	MessageBox.Show("参数不正确！" + ex.Message);
            }
            int iRet;

            //设置控制设备类型和串口属性
            iRet = NVSSDK.NetClient_SetDeviceType
            (
                m_conState[m_iCurrentFrame].m_iLogonID,
                m_conState[m_iCurrentFrame].m_iChannelNO,
                iCom,
                iDevAddress,
                btDevType
            );
            if (iRet < 0)
            {
                MessageBox.Show("NetClient_SetDeviceType Failed! USER_ERROR+" + (Marshal.GetLastWin32Error() - SDKConstMsg.USER_ERROR));
            }

            //设置串口属性
            iRet = NVSSDK.NetClient_SetComFormat
            (
                m_conState[m_iCurrentFrame].m_iLogonID,
                iCom,
                btDevType,
                btComFormat,
                iWorkMode
            );
            if (iRet < 0)
            {
                MessageBox.Show("NetClient_SetComFormat Failed! USER_ERROR+" + (Marshal.GetLastWin32Error() - SDKConstMsg.USER_ERROR));
            }
        }

        //修改设备类型信息
        private void btnDevTypeSet_Click(object sender, EventArgs e)
        {
            SetDeviceType();
        }

        //获得并显示视频参数
        private void GetVideoParam()
        {
            //创建视频参数结构体
            STR_VideoParam structVideoParam = new STR_VideoParam();

            //获得网络视频服务器某一路的视频显示参数
            NVSSDK.NetClient_GetVideoParam
            (
                m_conState[m_iCurrentFrame].m_iLogonID,
                m_conState[m_iCurrentFrame].m_iChannelNO,
                ref structVideoParam
            );

            //更新各视频参数显示控件的状态
            trckBrightness.Value = structVideoParam.m_ustBrightness;
            trckContrast.Value = structVideoParam.m_ustContrast;
            trckHue.Value = structVideoParam.m_usHue;
            trckSaturation.Value = structVideoParam.m_ustSaturation;
        }

        //修改视频参数
        private void SetVideoParam()
        {
            if (m_conState[m_iCurrentFrame].m_iLogonID < 0)
            {
                return;
            }
            //创建视频参数结构体
            STR_VideoParam structVideoParam1 = new STR_VideoParam();
            structVideoParam1.m_usHue = (UInt16)trckHue.Value;
            structVideoParam1.m_ustBrightness = (UInt16)trckBrightness.Value;
            structVideoParam1.m_ustContrast = (UInt16)trckContrast.Value;
            structVideoParam1.m_ustSaturation = (UInt16)trckSaturation.Value;

            //设置网络视频服务器某一路的视频显示参数
            int iRet = NVSSDK.NetClient_SetVideoParam
            (
                m_conState[m_iCurrentFrame].m_iLogonID,
                m_conState[m_iCurrentFrame].m_iChannelNO,
                ref structVideoParam1
            );
            if (iRet < 0)
            {
                MessageBox.Show("NetClient_SetVideoParam Failed! USER_ERROR+" + (Marshal.GetLastWin32Error() - SDKConstMsg.USER_ERROR));
            }
        }

        //修改对比度
        private void trckContrast_ValueChanged(object sender, EventArgs e)
        {
            //修改对应标签的数值
            lblContrast.Text = ((TrackBar)sender).Value.ToString();
        }

        //修改亮度
        private void trckBrightness_ValueChanged(object sender, EventArgs e)
        {
            //修改对应标签的数值
            lblBrightness.Text = ((TrackBar)sender).Value.ToString();
        }
       
        private void trckSaturation_ValueChanged(object sender, EventArgs e)
        {
            lblSaturation.Text = ((TrackBar)sender).Value.ToString();
        }

        private void trckHue_ValueChanged(object sender, EventArgs e)
        {
            lblHue.Text = ((TrackBar)sender).Value.ToString();
        }

        //拖动滑竿后自动设置视频参数
        private void trckVideoParam_MouseUp(object sender, MouseEventArgs e)
        {
            SetVideoParam();
        }

        //单击设置按钮后设置视频参数
        private void btnVideoParamSet_Click(object sender, EventArgs e)
        {
            SetVideoParam();
        }

        private void trckSpeed_ValueChanged(object sender, EventArgs e)
        {
            lblSpeed.Text = ((TrackBar)sender).Value.ToString();
        }

        //通过协议对设备进行控制
        //_iAction 动作码
        //_iParam1 水平速度或预置位号
        //_iParam2 垂直速度
        private int ProtocalControl(int _iAction,int _iParam1,int _iParam2)
        {
            int iLogonID = m_conState[m_iCurrentFrame].m_iLogonID;           
            int iChannelNo = m_conState[m_iCurrentFrame].m_iChannelNO;            
            int iRet = -1;

            //电子云台的参数,控制类型(Normal,e_PTZ)
            int iControlType = chkPTZ.Checked == true ? 1 : 0;

            //控制网络视频服务器某一通道所连接的设备动作
            iRet = NVSSDK.NetClient_DeviceCtrlEx
            (
                iLogonID,
                iChannelNo,
                _iAction,
                _iParam1,
                _iParam2,
                iControlType
            );
            if (iRet < 0)//设备控制失败
            {
                MessageBox.Show("NetClient_DeviceCtrlEx Failed ! USER_ERROR+" + (Marshal.GetLastWin32Error() - SDKConstMsg.USER_ERROR));
            }
            return iRet;
        }

        //通过透明通道对设备进行控制
        //_iAction 动作码
        //_iParam1 水平速度或预置位号
        //_iParam2 垂直速度
        private int ChannelControl(int _iAction,int _iParam1,int _iParam2)
        {
            int iLogonID = m_conState[m_iCurrentFrame].m_iLogonID;
            string strDevType = cboDeviceType.Text;
            if (strDevType.Substring(0, 4) != "DOME")
            {
                return -1;
            }
            int iComNo = cboComSend.SelectedIndex + 1;
            int iRet = -1;

            //创建并设置控制参数结构体
            CONTROL_PARAM cParam = new CONTROL_PARAM();
            cParam.m_ptMove.x = _iParam1;//水平速度
            cParam.m_ptMove.y = _iParam2;//垂直速度

            //CALL_VIEW : 62  调用预置位 
            //SET_VIEW  : 63  设置预置位
            if (_iAction == 62 || _iAction == 63)
            {
                cParam.m_iPreset = _iParam1; //预置位号
            }
            
            cParam.m_btBuf = new byte[256];//需要处理的数据
            try
            {
                //设备地址
                cParam.m_iAddress = Int32.Parse(textAddress.Text);
            }
            catch (System.Exception ex)
            {
                //转换失败
                MessageBox.Show(ex.Message);
                return -1;
            }

            //调用设备类型对应的dll文件中的控制码处理函数
            iRet = NVSSDK.NetClient_GetControlCode(strDevType, _iAction, ref cParam);
            if (iRet != 1)//控制码处理失败
            {                
                MessageBox.Show("NetClient_GetControlCode Failed ! ");
                return -1;
            }

            //通过透明通道进行串口的数据发送操作
            iRet = NVSSDK.NetClient_ComSend
            (
                iLogonID,
                cParam.m_btBuf,
                cParam.m_iCount,
                iComNo
            );
            if (iRet < 0) //数据发送失败
            {
                MessageBox.Show("NetClient_ComSend Failed ! USER_ERROR+" + (Marshal.GetLastWin32Error() - SDKConstMsg.USER_ERROR));
            }
            return iRet;
        }

        //对设备进行控制
        //_iAction 动作码
        //_iParam1 水平速度或预置位号
        //_iParam2 垂直速度
        private int DevControl(int _iAction)
        {
            UInt32 uiConID = m_conState[m_iCurrentFrame].m_uiConID;

            //当前视频窗口没有播放
            if (uiConID == UInt32.MaxValue)
            {
                return -1;
            }
            int iWorkMode = cboWorkMode.SelectedIndex;
            int iParam1 = 0;
            int iParam2 = 0;
            int iSpeed = trckSpeed.Value;
            int iPreset = 0;
            switch (_iAction)
            {
                case ActionControlMsg.MOVE_UP://向上移动
                    iParam1 = 0;
                    iParam2 = iSpeed;

                    //MOVE 透明通道移动码
                    _iAction = iWorkMode == 0 ? _iAction : ActionControlMsg.MOVE;
                    break;
                case ActionControlMsg.MOVE_DOWN://向下移动
                    iParam1 = 0;
                    iParam2 = -iSpeed;
                    _iAction = iWorkMode == 0 ? _iAction : ActionControlMsg.MOVE;
                    break;
                case ActionControlMsg.MOVE_LEFT://向左移动
                    iParam1 = -iSpeed;
                    iParam2 = 0;
                    _iAction = iWorkMode == 0 ? _iAction : ActionControlMsg.MOVE;
                    break;
                case ActionControlMsg.MOVE_RIGHT://向右移动
                    iParam1 = iSpeed;
                    iParam2 = 0;
                    _iAction = iWorkMode == 0 ? _iAction : ActionControlMsg.MOVE;
                    break;
                case ActionControlMsg.MOVE_UP_LEFT://向左上移动
                    iParam1 = -iSpeed;
                    iParam2 = iSpeed;
                    _iAction = iWorkMode == 0 ? _iAction : ActionControlMsg.MOVE;
                    break;
                case ActionControlMsg.MOVE_UP_RIGHT://向右上移动
                    iParam1 = iSpeed;
                    iParam2 = iSpeed;
                    _iAction = iWorkMode == 0 ? _iAction : ActionControlMsg.MOVE;
                    break;
                case ActionControlMsg.MOVE_DOWN_LEFT://向左下移动
                    iParam1 = -iSpeed;
                    iParam2 = -iSpeed;
                    _iAction = iWorkMode == 0 ? _iAction : ActionControlMsg.MOVE;
                    break;
                case ActionControlMsg.MOVE_DOWN_RIGHT://向右下移动
                    iParam1 = iSpeed;
                    iParam2 = -iSpeed;
                    _iAction = iWorkMode == 0 ? _iAction : ActionControlMsg.MOVE;
                    break;
                case ActionControlMsg.ZOOM_BIG://变倍大
                    iParam1 = iSpeed;
                    iParam2 = iSpeed;
                    _iAction = iWorkMode == 0 ? _iAction : 31;
                    break;
                case ActionControlMsg.ZOOM_SMALL://变倍小
                    iParam1 = iSpeed;
                    iParam2 = iSpeed;
                    _iAction = iWorkMode == 0 ? _iAction : 33;
                    break;
                case ActionControlMsg.FOCUS_NEAR://聚焦近
                    iParam1 = iSpeed;
                    iParam2 = iSpeed;
                    _iAction = iWorkMode == 0 ? _iAction : 37;
                    break;
                case ActionControlMsg.FOCUS_FAR://聚焦远
                    iParam1 = iSpeed;
                    iParam2 = iSpeed;
                    _iAction = iWorkMode == 0 ? _iAction : 35;
                    break;
                case ActionControlMsg.IRIS_OPEN://光圈开
                    iParam1 = iSpeed;
                    iParam2 = iSpeed;
                    _iAction = iWorkMode == 0 ? _iAction : 39;
                    break;
                case ActionControlMsg.IRIS_CLOSE://光圈关
                    iParam1 = iSpeed;
                    iParam2 = iSpeed;
                    _iAction = iWorkMode == 0 ? _iAction : 41;
                    break;
                case ActionControlMsg.RAIN_ON://雨刷开
                    iParam1 = iSpeed;
                    iParam2 = iSpeed;
                    _iAction = iWorkMode == 0 ? _iAction : 47;
                    break;
                case ActionControlMsg.RAIN_OFF://雨刷关
                    iParam1 = iSpeed;
                    iParam2 = iSpeed;
                    _iAction = iWorkMode == 0 ? _iAction : 48;
                    break;
                case ActionControlMsg.LIGHT_ON://背光开
                    iParam1 = iSpeed;
                    iParam2 = iSpeed;
                    _iAction = iWorkMode == 0 ? _iAction : 43;
                    break;
                case ActionControlMsg.LIGHT_OFF://背光关
                    iParam1 = iSpeed;
                    iParam2 = iSpeed;
                    _iAction = iWorkMode == 0 ? _iAction : 44;
                    break;
                case ActionControlMsg.HOR_AUTO://自动巡航
                    iParam1 = 0;
                    iParam2 = 0;
                    _iAction = iWorkMode == 0 ? _iAction : 21;
                    break;
                case ActionControlMsg.HOR_AUTO_STOP://停止自动巡航
                    iParam1 = 0;
                    iParam2 = 0;
                    _iAction = iWorkMode == 0 ? _iAction : 22;
                    break;
                case ActionControlMsg.CALL_VIEW://调用预置位                    
                    try
                    {
                        iPreset = Int32.Parse(textPreset.Text);
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return -1;
                    }
                    iParam1 = iPreset;
                    iParam2 = 0;
                    _iAction = iWorkMode == 0 ? _iAction : 62;
                    break;
                case ActionControlMsg.SET_VIEW://设置预置位
                    try
                    {
                        iPreset = Int32.Parse(textPreset.Text);
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return -1;
                    }
                    iParam1 = iPreset;
                    iParam2 = 0;
                    _iAction = iWorkMode == 0 ? _iAction : 63;
                    break;
                case ActionControlMsg.POWER_ON://打开电源
                    iParam1 = iSpeed;
                    iParam2 = iSpeed;
                    _iAction = iWorkMode == 0 ? _iAction : 45;
                    break;
                case ActionControlMsg.POWER_OFF://关闭电源
                    iParam1 = iSpeed;
                    iParam2 = iSpeed;
                    _iAction = iWorkMode == 0 ? _iAction : 46;
                    break;                
                default:                
                    iParam1 = 0;
                    iParam2 = 0;

                    //9 协议控制停止码
                    _iAction = iWorkMode == 0 ? 9 : _iAction;
                    break;
            }

            if (iWorkMode == 0)// 串口工作方式为协议
            { 
                //通过协议方式对设备进行控制
                return ProtocalControl(_iAction, iParam1, iParam2);
            }
            else if (iWorkMode == 1)// 串口工作方式为透明通道
            {  
                //通过透明通道对设备进行控制
                return ChannelControl(_iAction, iParam1, iParam2);
            }
            return -1;
        }

        //移动控制处理函数
        private void MOVE_MouseDown(object sender, MouseEventArgs e)
        {
            Button btnControl = (Button)sender;
            btnMoveAuto.Text = "Auto";

            //获得Tag属性中的控制码
            int iAction = Int32.Parse(btnControl.Tag.ToString());
           
            //调用移动处理函数
            DevControl(iAction);
        }

        //停止移动处理函数
        private void MOVE_STOP_MouseUp(object sender, MouseEventArgs e)
        {
            //停止移动操作
            DevControl(ActionControlMsg.MOVE_STOP);
        }

        //控制中的Auto键
        private void btnMoveAuto_Click(object sender, EventArgs e)
        {
            int iRet;

            //自动巡航操作
            if (btnMoveAuto.Text == "Auto") //为Auto状态
            {
                iRet = DevControl(ActionControlMsg.HOR_AUTO);
                if (iRet == 0)
                {
                    btnMoveAuto.Text = "Stop";
                }
            }
            else //为Stop状态
            {
                iRet = DevControl(ActionControlMsg.HOR_AUTO_STOP);
                if (iRet == 0)
                {
                    btnMoveAuto.Text = "Auto";
                }
            }            
        }

        private void chkPower_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPower.Checked) //选中状态
            {
                //调用函数DevControl，进行打开电源操作
                DevControl(ActionControlMsg.POWER_ON);
            }
            else //非选中状态
            {
                //调用函数DevControl，进行关闭电源操作
                DevControl(ActionControlMsg.POWER_OFF);
            }
             
        }

        private void chkLight_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLight.Checked) //选中状态
            {
                //调用函数DevControl，进行打开背光操作
                DevControl(ActionControlMsg.LIGHT_ON);
            }
            else //非选中状态
            {
                //调用函数DevControl，进行打关闭背光操作
                DevControl(ActionControlMsg.LIGHT_OFF);
            }
        }

        private void chkRain_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRain.Checked) //选中状态
            {
                //调用函数DevControl，进行打开雨刷操作
                DevControl(ActionControlMsg.RAIN_ON);
            }
            else //非选中状态
            {
                //调用函数DevControl，进行关闭雨刷操作
                DevControl(ActionControlMsg.RAIN_OFF);
            }
        }

        private void btnZoomBig_MouseUp(object sender, MouseEventArgs e)
        {
            //调用函数DevControl，停止变倍大操作
            DevControl(ActionControlMsg.ZOOM_BIG_STOP);
        }

        private void btnZoomSmall_MouseUp(object sender, MouseEventArgs e)
        {
            //调用函数DevControl，停止变倍小操作
            DevControl(ActionControlMsg.ZOOM_SMALL_STOP);
        }

        private void btnIrisOpen_MouseUp(object sender, MouseEventArgs e)
        {
            //调用函数DevControl，停止打开光圈操作
            DevControl(ActionControlMsg.IRIS_OPEN_STOP);
        }

        private void btnIrisClose_MouseUp(object sender, MouseEventArgs e)
        {
            //调用函数DevControl，停止关闭光圈操作
            DevControl(ActionControlMsg.IRIS_CLOSE_STOP);
        }

        private void btnFocusNear_MouseUp(object sender, MouseEventArgs e)
        {
            //调用函数DevControl，停止聚焦近操作
            DevControl(ActionControlMsg.FOCUS_NEAR_STOP);
        }

        private void btnFocusFar_MouseUp(object sender, MouseEventArgs e)
        {
            //调用函数DevControl，停止聚焦远操作
            DevControl(ActionControlMsg.FOCUS_FAR_STOP);
        }

        // 调用预置位操作
        private void btnGotoPreset_Click(object sender, EventArgs e)
        {
            int iPreset = 0;
            try
            {
                //进行预置位类型转换操作
                iPreset = Int32.Parse(textPreset.Text.Trim());
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            if (iPreset < 1 || iPreset > 255)
            {
                MessageBox.Show("预置位为（1--255）之间的数字");
                return;
            }

            //调用函数DevControl，进行调用预置位操作
            DevControl(ActionControlMsg.CALL_VIEW);
        }

        // 设置预置位操作
        private void btnSetPreset_Click(object sender, EventArgs e)
        {
            int iPreset = 0;
            try
            {
                //进行预置位类型转换操作
                iPreset = Int32.Parse(textPreset.Text.Trim());
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            if (iPreset < 1 || iPreset > 255)
            {
                MessageBox.Show("预置位为（1--255）之间的数字");
                return;
            }

            //调用函数DevControl，进行调用预置位操作
            DevControl(ActionControlMsg.SET_VIEW);
        }

        private void btnAssistantON_Click(object sender, EventArgs e)
        {
            int iAssistantNo = 0;
            try
            {
                iAssistantNo = Int32.Parse(textAssistant.Text.Trim());
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            //只接受1--8之间输入值
            if (iAssistantNo < 1 || iAssistantNo > 8)
            {
                MessageBox.Show("只接受1--8之间的数字");
                return;
            }
            int iWorkMode = cboWorkMode.SelectedIndex;
            int iSpeed = trckSpeed.Value;

            //只有串口工作方式为协议时，进行操作
            if (iWorkMode == 0)
            {
                ProtocalControl(33,iAssistantNo,iSpeed);
            }	
        }

        private void btnAssistantOFF_Click(object sender, EventArgs e)
        {
            int iAssistantNo = 0;
            try
            {
                iAssistantNo = Int32.Parse(textAssistant.Text.Trim());
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            //只接受1--8之间输入值
            if (iAssistantNo < 1 || iAssistantNo > 8)
            {
                MessageBox.Show("只接受1--8之间的数字");
                return;
            }
            int iWorkMode = cboWorkMode.SelectedIndex;
            int iSpeed = trckSpeed.Value;

            //只有串口工作方式为协议时，进行操作
            if (iWorkMode == 0)
            {
                ProtocalControl(34, iAssistantNo, iSpeed);
            }	
        }

        //通过透明通道进行串口的数据发送操作
        private void btnComSend_Click(object sender, EventArgs e)
        {
            //将输入字符串按‘，’进行拆分处理
            string[] strValue = textComSend.Text.Trim().Split(new char[] { ',' });
            byte[] btBuffer = new byte[1024];
            int i = 0;
            try
            {
                for ( ; i < strValue.Length; i++)
                {
                    //将十六进制字符串转换为整数
                    btBuffer[i] = (byte)Int32.Parse(strValue[i],System.Globalization.NumberStyles.HexNumber);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            int iLogonID = m_conState[m_iCurrentFrame].m_iLogonID;
            int iComNo = cboComSend.SelectedIndex + 1;
            int iRet;

            //发送数据btBuffer
            iRet = NVSSDK.NetClient_ComSend(iLogonID, btBuffer, i, iComNo);

            //操作失败
            if (iRet < 0)
            {
                MessageBox.Show("Send data failed !");
            }
        }

        private void Client_FormClosed(object sender, FormClosedEventArgs e)
        {
            //清除NetClient库
            NVSSDK.NetClient_Cleanup();

            //清除NSLook库
            NVSSDK.NSLook_Cleanup();
        }

        //连接某个注册中心服务器
        private void btnDSMLogon_Click(object sender, EventArgs e)
        {            
            string strUser = textServerUser.Text;
            string strPwd = textServerPwd.Text;
            string strIP = textServerIP.Text;

            //去除中间空格
            strIP = strIP.Replace(" ", "");
            byte[] btIP = Encoding.ASCII.GetBytes(strIP);
            string strPort = textServerPort.Text;
            int iPort = 0;
            try
            {
                //服务器端口类型转换
                iPort = Int32.Parse(strPort);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            //先注销,再登陆
            int iID = (Int32)btnDSMLogon.Tag;
            if (iID >= 0)
            {
                //断开和某个注册中心的连接
                NVSSDK.NSLook_LogoffServer(iID);
                btnDSMLogon.Tag = -1;
            }

            //连接到指定的服务器，默认不允许重连
            int iRet = NVSSDK.NSLook_LogonServer(btIP, iPort, false);

            //连接失败,弹出连接失败对话框
            if (iRet < 0)
            {  
                MessageBox.Show("连接注册中心服务器 " + strIP + " 失败 ！");
                return;
            }

            //连接成功,将连接号保存在按钮btnDSMLogon的Tag中
            btnDSMLogon.Tag = iRet;

            //创建DSM子窗体
            if (formDSM == null)
            {
                formDSM = new FormDSM();
            } 

            //修改子窗体的属性
            formDSM.ServerIP = strIP;
            formDSM.ServerID = iRet;
            formDSM.UserName = textServerUser.Text;
            formDSM.Password = textServerPwd.Text;

            //判断是否存在非活动FormDSM窗体，避免创建多个FormDSM窗体
            foreach (Form form in Application.OpenForms)
            {
                if (form.GetType() == typeof(FormDSM))
                {
                    form.Activate();
                    return;
                }
            }

            //显示子窗体
            formDSM.Show(this); 
        }

        //子窗体调用，登陆网络视频服务器
        public void Logon(string _strIP)
        {
            m_blNSClick = true;
            cboIP.Text = _strIP;
            btnLogon.Text = "Logon";         
            btnLogon_Click(btnLogon, EventArgs.Empty);           
        }

        //退出注册中心服务器时，子窗体调用
        public void LogofServer()
        {
            //将formDSM域置空
            formDSM = null;
        }

        private void btnReplay_Click(object sender, EventArgs e)
        {
            ReplayForm frm = new ReplayForm(m_cltInfo, m_cltInfo.m_iServerID,this);

            frm.ShowDialog();
            //frm.Show(this);
            
        }

        private void MyMAIN_NOTIFY_V4(UInt32 _ulLogonID, IntPtr _iWparam, IntPtr _iLParam, Int32 _iUser)
        {
            switch (_iWparam.ToInt32())
            {
                //登陆状态消息 
                //param1 登陆IP
                //param2 登陆ID
                //param3 登陆状态
                case SDKConstMsg.WCM_LOGON_NOTIFY:
                    {
                        m_conState[m_iCurrentFrame].m_iLogonID = (int)_ulLogonID;
                        switch(_iLParam.ToInt32())
                        {
                            case SDKConstMsg.LOGON_SUCCESS:
                                MessageBox.Show("登陆成功！notify_v4");
                                break;
                            case SDKConstMsg.LOGON_TIMEOUT:
                                MessageBox.Show("登陆超时！notify_v4");
                                break;
                            default:
                                break;
                        }
                        
                        break;
                    }
               default:
                    break;
            }
        }

        private void MyAlarm_NOTIFY_V4(Int32 _ulLogonID, Int32 _iChan, Int32 _iAlarmState, Int32 _iAlarmType, Int32 _iUser)
        {


            StringBuilder sbAlarmMsg = new StringBuilder("AlarmMsg-", 128);

            sbAlarmMsg.Append(DateTime.Now.ToLocalTime().ToString());

            switch (_iAlarmType)
            {
                case AlarmConstMsgType.ALARM_VDO_MOTION:
                    sbAlarmMsg.Append("- MOTION");
                    break;
                case AlarmConstMsgType.ALARM_VDO_REC:
                    sbAlarmMsg.Append("- REC");
                    break;
                case AlarmConstMsgType.ALARM_VDO_LOST:
                    sbAlarmMsg.Append("- LOST");
                    break;
                case AlarmConstMsgType.ALARM_VDO_INPORT:
                    sbAlarmMsg.Append("- INPORT");
                    break;
                case AlarmConstMsgType.ALARM_VDO_OUTPORT:
                    sbAlarmMsg.Append("- OUTPORT");
                    break;
                case AlarmConstMsgType.ALARM_VDO_COVER:
                    sbAlarmMsg.Append("- COVER");
                    break;
                case AlarmConstMsgType.ALARM_VCA_INFO:
                    sbAlarmMsg.Append("- VCA");
                    break;
                default:
                    sbAlarmMsg.Append("-" + _iAlarmType.ToString());
                    break;
            }

            switch (_iAlarmState)
            {
                case 0:
                    sbAlarmMsg.Append("- OFF");
                    break;
                case 1:
                    sbAlarmMsg.Append("- ON");
                    break;
                default:
                    sbAlarmMsg.Append("-" + _iAlarmState.ToString());
                    break;
            }
            lbAlarmlistBox.Items.Insert(0, sbAlarmMsg.ToString());
        }
    }
}
