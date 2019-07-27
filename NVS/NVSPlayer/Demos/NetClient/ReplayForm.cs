using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


using System.Runtime.InteropServices;


namespace NetClient
{
    public partial class ReplayForm : BaseForm
    {
        public int m_iLogonID = -1;


        private CLIENTINFO  m_ClientInfo;

        private int	m_iTotalCount = 0;     // total file number
	    private int m_iCurrentCount = 0;  // file number of current page
	    private int m_iCurrentPage = 0;    // index of page
	    private int m_iTotalPage = 0;         // the number of page
        private List<Download> m_lstDownloadFile = new List<Download>();


        private NVS_FILE_TIME m_StartTime = new NVS_FILE_TIME();
        private NVS_FILE_TIME m_EndTime = new NVS_FILE_TIME();
        private UInt32 m_ulConnID;
        

        public ReplayForm()
        {
            InitializeComponent();
        }
        public ReplayForm(CLIENTINFO clientinfo, int iLogonID,BaseForm parentfrm)
            : base(parentfrm)
        {
            m_ClientInfo = clientinfo;
            m_iLogonID = iLogonID;
            InitializeComponent();
            
            Update_UI_IPAndID();

        }

        private void Update_UI_IPAndID()
        {

            cbVideoType.SelectedIndex = 0;
            cbFileType.SelectedIndex = 0;
            cbBitStreamType.SelectedIndex = 0;
            cbQueryMode.SelectedIndex = 0;

            DateTime dt = new DateTime(DateTime.Now.Year,
                                       DateTime.Now.Month,
                                       DateTime.Now.Day,
                                       0,0,0);

            dtStartTimeFile.Value = dt;
            dtEndTimeFile.Value = DateTime.Now;
            dtStartTimeTime.Value = dt;
            dtEndTimeTime.Value = DateTime.Now;

            
            int iChannelNum = 0;
	        NVSSDK.NetClient_GetChannelNum(m_iLogonID, ref iChannelNum);
            cbPassNOFile.Items.Clear();
            cbPassNOFile.Items.Add("All");
	        for (int i = 0; i < iChannelNum; i++)
	        {
                cbPassNOFile.Items.Add(i.ToString());
	        }
            if (cbPassNOFile.Items.Count > 0)
            {
                cbPassNOFile.SelectedIndex = 0;
            }
            

	        int iAlarmChannelNo = 0,iAlarmOutPortNum = 0;
	        int iRet =NVSSDK.NetClient_GetAlarmPortNum(m_iLogonID, ref iAlarmChannelNo, ref iAlarmOutPortNum);
            cbAlarmType.Items.Clear();
	        for (int i = 0; i < iAlarmChannelNo; i++)
	        {
                cbAlarmType.Items.Add(i.ToString());
	        }
            if (cbAlarmType.Items.Count > 0)
            {
                cbAlarmType.SelectedIndex = 0;
            }
            


            cbPassNOTime.Items.Clear();
            cbPassNOTime.Items.Add("All");
            for (int i = 0; i < iChannelNum; i++)
            {
                cbPassNOTime.Items.Add(i.ToString());
            }
            if (cbPassNOTime.Items.Count > 0)
            {
                cbPassNOTime.SelectedIndex = 0;
            }
            
	        
        }

       
        private void btnQuery_Click(object sender, EventArgs e)
        {
            m_iCurrentPage = 0;
            QueryFile();
            
        }

        private void ReplayForm_Load(object sender, EventArgs e)
        {
                        
           
        }

        private void ReplayForm_FormClosing(object sender, FormClosingEventArgs e)
        {
             
        }

        public override void OnMessagePro(IntPtr wParam, IntPtr lParam)
        {
            int iMsg = wParam.ToInt32() & 0xFFFF;
            switch (iMsg)    
            {
                case SDKConstMsg.WCM_QUERYFILE_FINISHED: 
                    GetFileCountInfo();
                    Update_UI_Query_Result();
                    break;
                case SDKConstMsg.WCM_DWONLOAD_FINISHED:
                    try
                    {
                        int iConnID = lParam.ToInt32();
                        DownloadtFinished(iConnID);
                    }
                    catch (System.Exception ex)
                    {
                    	
                    }
                    
                    break;
                case SDKConstMsg.WCM_DWONLOAD_FAULT:

                    break;
                case SDKConstMsg.WCM_LOGON_NOTIFY:
                    _MAIN_NOTIFY_DATA Info = (_MAIN_NOTIFY_DATA)Marshal.PtrToStructure(wParam, typeof(_MAIN_NOTIFY_DATA));
                    
                    if (Info.m_lParam == SDKConstMsg.LOGON_SUCCESS)
                    {
                        // 以前没有登录过
                        if (m_iLogonID == -1)
                        {
                            m_iLogonID = Info.m_iLogonID;
                            m_ClientInfo.m_iServerID = Info.m_iLogonID;
                        }
                        else
                        {
                            BreakContinue(Info.m_iLogonID);
                        }
                        
                    }
                    
                    break;
                default:
                    {
                        break;
                    }
            }
            
        }
        private void BreakContinue(int iLogonID)
        {

            
            foreach (Download dlfile in m_lstDownloadFile)
            {
                if (dlfile.GetLogonID() == iLogonID )  
                    if(dlfile.GetBreakContinue() == 1)
                    {
                        DOWNLOAD_FILE tdf = new DOWNLOAD_FILE();// = {sizeof(DOWNLOAD_CONTROL)};
                        tdf.m_cRemoteFilename = new char[255];
                        tdf.m_cLocalFilename = new char[255];
            
                        CharsCopy(dlfile.GetFilename().ToCharArray(), tdf.m_cRemoteFilename);
                        CharsCopy(("E:\\netclientdemo_download\\" + dlfile.GetFilename()).ToCharArray(), tdf.m_cLocalFilename);
  
                        tdf.m_iSize = Marshal.SizeOf(tdf);
                        tdf.m_iPosition = -1;
		                tdf.m_iSpeed = 16;
                        tdf.m_iReqMode = cbQueryMode.SelectedIndex;
                        UInt32 iConnID = 0;
                        int iRet = NVSSDK.NetClient_NetFileDownload(ref iConnID, m_ClientInfo.m_iServerID, 3, StructToBytes(tdf), Marshal.SizeOf(tdf));
                        if (iRet >= 0)
                        {
                            dlfile.SetConnID(iConnID);
                            timerFile.Enabled = true;
                        }
                    }
            }
	        
        }
        private void QueryFile()
        {
            if (m_iLogonID < 0)
            {
                return;
            }
            if (dtStartTimeFile.Value > dtEndTimeFile.Value)
            {
                MessageBox.Show("Start time > End time");
            }
            //NVS_FILE_QUERY
            NVS_FILE_QUERY Query = new NVS_FILE_QUERY();

            int iType = cbVideoType.SelectedIndex; 
            if (iType == 0)
            {
                Query.m_iType = 0xFF;
            }
            else if (iType <= 3)
            {
                Query.m_iType = iType;
            }
            else
            {
                Query.m_iType = 1431;
            }


            Query.m_iChannel =cbPassNOFile.SelectedIndex - 1;
            //if (Query.m_iChannel == -1)
            //{
                Query.m_iChannel = 0xFF;
           // }

            Query.m_struStartTime.m_iYear = Convert.ToUInt16(dtStartTimeFile.Value.Year);
            Query.m_struStartTime.m_iMonth = Convert.ToUInt16(dtStartTimeFile.Value.Month);
            Query.m_struStartTime.m_iDay = Convert.ToUInt16(dtStartTimeFile.Value.Day);
            Query.m_struStartTime.m_iHour = Convert.ToUInt16(dtStartTimeFile.Value.Hour);
            Query.m_struStartTime.m_iMinute = Convert.ToUInt16(dtStartTimeFile.Value.Minute);
            Query.m_struStartTime.m_iSecond = Convert.ToUInt16(dtStartTimeFile.Value.Second);


            Query.m_struStoptime.m_iYear = Convert.ToUInt16(dtEndTimeFile.Value.Year);
            Query.m_struStoptime.m_iMonth = Convert.ToUInt16(dtEndTimeFile.Value.Month);
            Query.m_struStoptime.m_iDay = Convert.ToUInt16(dtEndTimeFile.Value.Day);
            Query.m_struStoptime.m_iHour = Convert.ToUInt16(dtEndTimeFile.Value.Hour);
            Query.m_struStoptime.m_iMinute = Convert.ToUInt16(dtEndTimeFile.Value.Minute);
            Query.m_struStoptime.m_iSecond = Convert.ToUInt16(dtEndTimeFile.Value.Second);


            Query.m_iPageSize = NVSSDK.MAX_PAGESIZE;
            Query.m_iPageNo = m_iCurrentPage;
            Query.m_iFiletype = cbFileType.SelectedIndex;


            int ret;
            //Int32 i = 0;
            //Int32 j = 0;

            try
            {
                ret = NVSSDK.NetClient_NetFileQuery(m_iLogonID, ref Query);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("NetClient_NetFileQuery failed" + ex.Message);
                return;
            }

            if (ret < 0)
            {
                MessageBox.Show("NetClient_NetFileQuery failed");
                return;
            }
        }
        /// <summary>
        /// Get File Count and init some value
        /// </summary>
        private void GetFileCountInfo()
        {


            int iret = -1;
            iret = NVSSDK.NetClient_NetFileGetFileCount(m_iLogonID, ref m_iTotalCount, ref m_iCurrentCount);
            if (iret < 0)
            {
                MessageBox.Show("NetClient_NetFileGetFileCount failed");
               
            }
            if (m_iTotalCount > 0)
            {
                m_iTotalPage = m_iTotalCount / (NVSSDK.MAX_PAGESIZE) + 1;
              
            }


        }
        /// <summary>
        /// Get fileinfo and update ui
        /// </summary>
        private void Update_UI_Query_Result()
        {
            
            
            lbFileNumber.Text = "File Number:" + m_iTotalCount.ToString();

	        cbTotolPage.Items.Clear();
	        for (int i = 0; i < m_iTotalPage; i++)
	        {
		        cbTotolPage.Items.Add(i+1);
	        }
	        //cbTotolPage.SelectedIndex = m_iCurrentPage;
           
            lvFileData.Items.Clear();

            for (int i = 0; i < m_iCurrentCount; i++)
            {
                NVS_FILE_DATA filedata = new NVS_FILE_DATA();
                int iret = NVSSDK.NetClient_NetFileGetQueryfile(m_iLogonID, i, ref filedata);
                if (iret < 0)
                {
                    MessageBox.Show("NetClient_NetFileGetQueryfile failed");
                    return;
                    //GetLastError()
                }
                else
                {
                    ListViewItem one = new ListViewItem();
                    one.Text = (m_iCurrentPage * NVSSDK.MAX_PAGESIZE + i).ToString();
                    string str = new string(filedata.m_cFileName, 0, filedata.m_cFileName.Length);
                    str = str.Trim("\0".ToCharArray());
                    one.SubItems.Add(str);
                    one.SubItems.Add(filedata.m_iType.ToString());
                    one.SubItems.Add(filedata.m_iFileSize.ToString());
                    string stime = filedata.m_struStartTime.m_iYear.ToString()+ "-" +
                                   filedata.m_struStartTime.m_iMonth.ToString("D2")+ "-" +
                                   filedata.m_struStartTime.m_iDay.ToString("D2") + " " +
                                   filedata.m_struStartTime.m_iHour.ToString("D2") + ":" +
                                   filedata.m_struStartTime.m_iSecond.ToString("D2") + ":" +
                                   filedata.m_struStartTime.m_iMinute.ToString("D2");
                    one.SubItems.Add(stime);
                    stime = filedata.m_struStoptime.m_iYear.ToString() + "-" +
                                   filedata.m_struStoptime.m_iMonth.ToString("D2") + "-" +
                                   filedata.m_struStoptime.m_iDay.ToString("D2") + " " +
                                   filedata.m_struStoptime.m_iHour.ToString("D2") + ":" +
                                   filedata.m_struStoptime.m_iSecond.ToString("D2") + ":" +
                                   filedata.m_struStoptime.m_iMinute.ToString("D2");
                    one.SubItems.Add(stime);
                    one.SubItems.Add("");  // 预留增加下载进度
                    lvFileData.Items.Add(one);
                }
            }
        }

        private void btnUpPage_Click(object sender, EventArgs e)
        {
            if (m_iCurrentPage > 0)
            {
                m_iCurrentPage--;
                QueryFile();
            }
        }

        private void butNextPage_Click(object sender, EventArgs e)
        {
            if (m_iCurrentPage < m_iTotalPage - 1)
            {
                m_iCurrentPage++;
                QueryFile();
            }
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            m_iCurrentPage = 0;
            QueryFile();
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            if (m_iTotalPage > 0)
            {
                m_iCurrentPage = m_iTotalPage - 1;
                QueryFile();
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (m_iLogonID < 0)
            {
                return;
            }
            string sFileName = "";
            for (int i = 0; i < lvFileData.Items.Count; i++)
            {
                if (lvFileData.Items[i].Checked)
                {
                    sFileName = lvFileData.Items[i].SubItems[1].Text.ToString();
                    break;
                }
            }
            sFileName = sFileName.Trim();
            int irt = -1;
            irt = PLAYSDK.TC_CreateSystem(IntPtr.Zero);
            if (irt < 0)
            {
                MessageBox.Show("TC_CreateSystem failed!");
            }
            PlayForm frmPlay = new PlayForm(m_ClientInfo, sFileName,this);

            frmPlay.ShowDialog();
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            string sFileName = "";
            for (int i = 0; i < lvFileData.Items.Count; i++)
            {
                if (lvFileData.Items[i].Checked)
                {
                    sFileName = lvFileData.Items[i].SubItems[1].Text.ToString();
                    break;
                }
            }
            //sFileName = sFileName.Trim();
            if (m_iLogonID < 0 || sFileName == "")
	        {
		        return;
	        }

	        UInt32 iConnID = 0;
	        if (IsFileInList(m_iLogonID, sFileName,ref iConnID))
	        {
		        return;
	        }
            if (!System.IO.Directory.Exists("E:\\netclientdemo_download"))
            {
                System.IO.Directory.CreateDirectory("E:\\netclientdemo_download");
            }
	        //int iRet = NetClient_NetFileDownloadFile(&iConnID, m_iLogonID, (char *)(LPCSTR)m_szFileName, (char *)(LPCSTR)(g_szDownloadPath + m_szFileName), 0, -1, -1);
	        DOWNLOAD_FILE tdf = new DOWNLOAD_FILE();//= {sizeof(DOWNLOAD_FILE)};
	        tdf.m_cRemoteFilename = new char[255];
            tdf.m_cLocalFilename = new char[255];
            //strcpy_s(tdf.m_cRemoteFilename,m_szFileName.GetLength()+1,(char *)(LPCSTR)m_szFileName);
	        //strcpy_s(tdf.m_cLocalFilename,(g_szDownloadPath + m_szFileName).GetLength()+1,(char*)(LPCSTR)(g_szDownloadPath + m_szFileName));

            CharsCopy(sFileName.ToCharArray(), tdf.m_cRemoteFilename);
            //tdf.m_cRemoteFilename = sFileName.ToCharArray();
            CharsCopy(("E:\\netclientdemo_download\\" + sFileName).ToCharArray(), tdf.m_cLocalFilename);
            //tdf.m_cLocalFilename = "c:\\netclientdemo_download\\".ToCharArray();
            tdf.m_iPosition = -1;
	        tdf.m_iSpeed = 32;
            tdf.m_iReqMode = cbQueryMode.SelectedIndex;

            int iRet = NVSSDK.NetClient_NetFileDownload(ref iConnID, m_ClientInfo.m_iServerID, 0, StructToBytes(tdf), Marshal.SizeOf(tdf));
	        if (iRet >= 0)
	        {
		        //int iRet = NetClient_NetFileDownloadFile(&iConnID, m_iLogonID, (char *)(LPCSTR)m_szFileName, (char *)(LPCSTR)(g_szDownloadPath + m_szFileName), 1, -1, 16);//设置下载速度为32倍速
		        DOWNLOAD_CONTROL tdc = new DOWNLOAD_CONTROL();// = {sizeof(DOWNLOAD_CONTROL)};
                tdc.m_iSize = Marshal.SizeOf(tdc);
                tdc.m_iPosition = -1;
		        tdc.m_iSpeed = 16;
                tdc.m_iReqMode = cbQueryMode.SelectedIndex;
                iRet = NVSSDK.NetClient_NetFileDownload(ref iConnID, m_ClientInfo.m_iServerID, 2, StructToBytes(tdc), Marshal.SizeOf(tdc));
                Download downloadfile = new Download(m_iLogonID, iConnID, sFileName,"","");
                if (cbBreakContinue.Checked)
                {
                    downloadfile.SetBreakContinue(1);
                }
                else
                {
                    downloadfile.SetBreakContinue(0);
                }
                
		        downloadfile.SetReqMode(tdc.m_iReqMode);
		        m_lstDownloadFile.Add(downloadfile);
                timerFile.Enabled = true;
	        }
        }
        private void CharsCopy(char[] source, char[] des)
        {
            for (int i = 0; i < source.Length & i < des.Length;i++ )
            {
                des[i] = source[i];
            }
        }

        private byte[] StructToBytes(object structObj)
        {
            int iSize = Marshal.SizeOf(structObj);
            byte[] bytes = new byte[iSize];
            IntPtr SturctPtr = Marshal.AllocHGlobal(iSize);
            Marshal.StructureToPtr(structObj, SturctPtr, false);
            Marshal.Copy(SturctPtr, bytes, 0, iSize);
            Marshal.FreeHGlobal(SturctPtr);
            return bytes;
        
        }
        public bool IsFileInList( int _iLogonID, string _szFileName , ref UInt32 _iConnID)
        {
            bool bIn = false;
        	 
            //for (List <Download>::iterator it = m_lstDownloadFile.begin(); it != m_lstDownloadFile.end(); it++)
            for (int i = 0; i < m_lstDownloadFile.Count; i++)
            {

                if (m_lstDownloadFile[i].GetFilename() == _szFileName && m_lstDownloadFile[i].GetLogonID() == _iLogonID)
	            {
		            bIn = true;
		            _iConnID = m_lstDownloadFile[i].GetConnID();

                    //_pFile = m_lstDownloadFile[i];

		            break;
	            }
            }
            return bIn;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            string sFileName = "";
            for (int i = 0; i < lvFileData.Items.Count; i++)
            {
                if (lvFileData.Items[i].Checked)
                {
                    sFileName = lvFileData.Items[i].SubItems[1].Text.ToString();
                    break;
                }
            }
            if (m_iLogonID < 0 || sFileName == "")
            {
                return;
            }
            sFileName = sFileName.Trim();
            UInt32 iConnID = 0;
            if (IsFileInList(m_iLogonID, sFileName, ref iConnID))
	        {
		        //for (list <CLS_DownloadFile *>::iterator it = m_lstDownloadFile.begin(); it != m_lstDownloadFile.end(); it++)
                for (int i = 0; i < m_lstDownloadFile.Count;  i++)
                {
                    if (m_lstDownloadFile[i].GetConnID() == iConnID)
                        
                    {
                        m_lstDownloadFile[i].StopDownload();
                        m_lstDownloadFile.RemoveAt(i);
                    }
                }
	        }
            // 如果没有下载的文件了，停止Timer更新进度
            if (m_lstDownloadFile.Count == 0)
            {
                timerFile.Enabled = false;
            }
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            string sFileName = "";
            for (int i = 0; i < lvFileData.Items.Count; i++)
            {
                if (lvFileData.Items[i].Checked)
                {
                    sFileName = lvFileData.Items[i].SubItems[1].Text.ToString();
                    break;
                }
            }
            sFileName = sFileName.Trim();
            if (m_iLogonID < 0 || sFileName == "")
            {
                return;
            }

            UInt32 iConnID = 0;
            if (IsFileInList(m_iLogonID, sFileName, ref iConnID))
            {
                DOWNLOAD_CONTROL tdc = new DOWNLOAD_CONTROL();// = {sizeof(DOWNLOAD_CONTROL)};
                tdc.m_iSize = Marshal.SizeOf(tdc);
                tdc.m_iPosition = -1;
                tdc.m_iSpeed = 0;
                tdc.m_iReqMode = cbQueryMode.SelectedIndex;
                int iRet = NVSSDK.NetClient_NetFileDownload(ref iConnID, m_ClientInfo.m_iServerID, 2, StructToBytes(tdc), Marshal.SizeOf(tdc));
                //if (iRet < 0)
                //{
                //    MessageBox.Show("NetClient_NetFileDownload faild!");
                //}
            }
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            string sFileName = "";
            for (int i = 0; i < lvFileData.Items.Count; i++)
            {
                if (lvFileData.Items[i].Checked)
                {
                    sFileName = lvFileData.Items[i].SubItems[1].Text.ToString();
                    break;
                }
            }
            sFileName = sFileName.Trim();
            if (m_iLogonID < 0 || sFileName == "")
            {
                return;
            }

            UInt32 iConnID = 0;
            if (IsFileInList(m_iLogonID, sFileName, ref iConnID))
            {
                DOWNLOAD_CONTROL tdc = new DOWNLOAD_CONTROL();// = {sizeof(DOWNLOAD_CONTROL)};
                tdc.m_iSize = Marshal.SizeOf(tdc);
                tdc.m_iPosition = -1;
                tdc.m_iSpeed = 16;
                tdc.m_iReqMode = cbQueryMode.SelectedIndex;
                int iRet = NVSSDK.NetClient_NetFileDownload(ref iConnID, m_ClientInfo.m_iServerID, 2, StructToBytes(tdc), Marshal.SizeOf(tdc));
                //if (iRet < 0)
                //{
                //    MessageBox.Show("NetClient_NetFileDownload faild!");
                //}
            }
        }

        private void btnDownloadTime_Click(object sender, EventArgs e)
        {

            if (dtStartTimeTime.Value >= dtEndTimeTime.Value)
            {
                MessageBox.Show("Start Time > End Time!");
                return;
            }

            m_StartTime.m_iYear = Convert.ToUInt16(dtStartTimeTime.Value.Year);
            m_StartTime.m_iMonth = Convert.ToUInt16(dtStartTimeTime.Value.Month);
            m_StartTime.m_iDay = Convert.ToUInt16(dtStartTimeTime.Value.Day);
            m_StartTime.m_iHour = Convert.ToUInt16(dtStartTimeTime.Value.Hour);
            m_StartTime.m_iMinute = Convert.ToUInt16(dtStartTimeTime.Value.Minute);
            m_StartTime.m_iSecond = Convert.ToUInt16(dtStartTimeTime.Value.Second);


            m_EndTime.m_iYear = Convert.ToUInt16(dtEndTimeTime.Value.Year);
            m_EndTime.m_iMonth = Convert.ToUInt16(dtEndTimeTime.Value.Month);
            m_EndTime.m_iDay = Convert.ToUInt16(dtEndTimeTime.Value.Day);
            m_EndTime.m_iHour = Convert.ToUInt16(dtEndTimeTime.Value.Hour);
            m_EndTime.m_iMinute = Convert.ToUInt16(dtEndTimeTime.Value.Minute);
            m_EndTime.m_iSecond = Convert.ToUInt16(dtEndTimeTime.Value.Second);



            string localfile = "Demo_Download" + m_StartTime.m_iYear.ToString()
                                               + m_StartTime.m_iMonth.ToString()
                                               + m_StartTime.m_iDay.ToString()
                                               + m_StartTime.m_iHour.ToString()
                                               + m_StartTime.m_iMinute.ToString()
                                               + m_StartTime.m_iSecond.ToString()
                                               + "-"
                                               + m_EndTime.m_iYear.ToString()
                                               + m_EndTime.m_iMonth.ToString()
                                               + m_EndTime.m_iDay.ToString()
                                               + m_EndTime.m_iHour.ToString()
                                               + m_EndTime.m_iMinute.ToString()
                                               + m_EndTime.m_iSecond.ToString()
                                               + ".sdv";

            if (!System.IO.Directory.Exists("E:\\netclientdemo_download"))
            {
                System.IO.Directory.CreateDirectory("E:\\netclientdemo_download");
            }
            string path = "E:\\netclientdemo_download\\"  + localfile;
            int iChannelNo = cbPassNOTime.SelectedIndex;
            int iStreamNo = cbBitStreamType.SelectedIndex;
            int iChannelNum = 0;
            NVSSDK.NetClient_GetChannelNum(m_ClientInfo.m_iServerID, ref iChannelNum);
            int iRealChannel = iStreamNo * iChannelNum + iChannelNo;
            int iRet = NVSSDK.NetClient_NetFileDownloadByTimeSpanEx(ref m_ulConnID,
                              m_ClientInfo.m_iServerID,
                              path,iRealChannel,ref m_StartTime, ref m_EndTime,
                              0, -1, 16);
            if (iRet < 0)
            {
                if (m_ulConnID != -1)
                {
                    NVSSDK.NetClient_NetFileStopDownloadFile(m_ulConnID);
                    //m_ulConnID = 0;
                }
                return;
            }
            else
            {
                iRet = NVSSDK.NetClient_NetFileDownloadByTimeSpanEx(ref m_ulConnID,
                              m_ClientInfo.m_iServerID,
                              path, iRealChannel, ref m_StartTime, ref m_EndTime,
                              1, -1, 16);
            }

            pbDownloadTime.Value = 0;
            timerTime.Enabled = true;
        
        }


        private void btnStopDownloadTime_Click(object sender, EventArgs e)
        {
            if (m_ClientInfo.m_iServerID != -1 && m_ulConnID != -1)
            {
                NVSSDK.NetClient_NetFileStopDownloadFile(m_ulConnID);
                pbDownloadTime.Value = 0;
                m_ulConnID = 0;
                lbDownloadStatusTime.Text = "Stop Download";
                timerTime.Enabled = false;
            }
        }
        private void DownloadtFinished(int iID)
        {
            if (m_ulConnID == iID)
            {
                NVSSDK.NetClient_NetFileStopDownloadFile(m_ulConnID);
                pbDownloadTime.Value = 100;
                m_ulConnID = 0;
                lbDownloadStatusTime.Text = "Download Finished";
                timerTime.Enabled = false;
            }
            foreach (Download file in m_lstDownloadFile)
            {
                if (file.GetConnID() == iID)
                {
                    file.StopDownload();
                    m_lstDownloadFile.Remove(file);
                    for (int i = 0; i < lvFileData.Items.Count; i++)
                    {
                        string sFileName = lvFileData.Items[i].SubItems[1].Text.ToString();

                        if (sFileName == file.GetFilename())
                        {
                            lvFileData.Items[i].SubItems[6].Text = "Finished";
                        }
                    }

                    break;
                }
            }
            if (m_lstDownloadFile.Count == 0)
            {
                timerFile.Enabled = false;
            }
        }

        private void btnPlayTime_Click(object sender, EventArgs e)
        {
            if (dtStartTimeTime.Value >= dtEndTimeTime.Value)
            {
                MessageBox.Show("Start Time > End Time!");
                return;
            }
            if (m_iLogonID < 0)
            {
                return;
            }
            m_StartTime.m_iYear = Convert.ToUInt16(dtStartTimeTime.Value.Year);
            m_StartTime.m_iMonth = Convert.ToUInt16(dtStartTimeTime.Value.Month);
            m_StartTime.m_iDay = Convert.ToUInt16(dtStartTimeTime.Value.Day);
            m_StartTime.m_iHour = Convert.ToUInt16(dtStartTimeTime.Value.Hour);
            m_StartTime.m_iMinute = Convert.ToUInt16(dtStartTimeTime.Value.Minute);
            m_StartTime.m_iSecond = Convert.ToUInt16(dtStartTimeTime.Value.Second);


            m_EndTime.m_iYear = Convert.ToUInt16(dtEndTimeTime.Value.Year);
            m_EndTime.m_iMonth = Convert.ToUInt16(dtEndTimeTime.Value.Month);
            m_EndTime.m_iDay = Convert.ToUInt16(dtEndTimeTime.Value.Day);
            m_EndTime.m_iHour = Convert.ToUInt16(dtEndTimeTime.Value.Hour);
            m_EndTime.m_iMinute = Convert.ToUInt16(dtEndTimeTime.Value.Minute);
            m_EndTime.m_iSecond = Convert.ToUInt16(dtEndTimeTime.Value.Second);

            int iChannelNo = cbPassNOTime.SelectedIndex;
            int iStreamNo = cbBitStreamType.SelectedIndex;
            int iChannelNum = 0;
            NVSSDK.NetClient_GetChannelNum(m_ClientInfo.m_iServerID, ref iChannelNum);
            int iRealChannel = iStreamNo * iChannelNum + iChannelNo;
            //m_PlayPage.SetDownloadParam(m_iLogonID, iRealChannel/*iChannelNo*/, &begintime, &endtime);
            //m_PlayPage.DoModal();

            int irt = -1;
            irt = PLAYSDK.TC_CreateSystem(IntPtr.Zero);
            if (irt < 0)
            {
                MessageBox.Show("TC_CreateSystem failed!");
            }
            PlayForm frmPlay = new PlayForm(m_ClientInfo, iRealChannel, m_StartTime, m_EndTime,this);
        
            frmPlay.ShowDialog();
        }

        private void timerTime_Tick(object sender, EventArgs e)
        {
            ProgressDownlondByTime();
        }
        private void ProgressDownlondByTime()
        {
            if (m_ulConnID != -1)
            {
                int uiCurrentPos = 0;
                int iSize = 0;
                if (NVSSDK.NetClient_NetFileGetDownloadPos(m_ulConnID, ref uiCurrentPos, ref iSize) >= 0)
                {
                    long uiBeginTime = CommonFunction.NvsFileTimeToAbsSeconds(m_StartTime);
                    long uiEndTime = CommonFunction.NvsFileTimeToAbsSeconds(m_EndTime);
                    long iTimeInterval = uiEndTime - uiBeginTime;
                    if (iTimeInterval > 0)
                    {
                        long iCurrentInterval = uiCurrentPos - uiBeginTime;
                        if (iCurrentInterval > 0 && iCurrentInterval < iTimeInterval)
                        {

                            long iProcess = iCurrentInterval * 100 / iTimeInterval;
                            if (iProcess > 100)
                            {
                                return;
                            }
                            pbDownloadTime.Value = Convert.ToInt32(iProcess);
                        }

                    }
                    DateTime dtTime = CommonFunction.ConvertIntToDateTime(uiCurrentPos);

                    lbDownloadStatusTime.Text = dtTime.Year.ToString() + "-" +
                                                dtTime.Month.ToString("D2") + "-" +
                                                dtTime.Day.ToString("D2") + " " +
                                                dtTime.Hour.ToString("D2") + ":" +
                                                dtTime.Minute.ToString("D2") + ":" +
                                                dtTime.Second.ToString("D2");
                }
            }
        }

        private void timerFile_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < m_lstDownloadFile.Count; i++)
            {
                UInt32 iConnID = m_lstDownloadFile[i].GetConnID();
                if (iConnID != -1)
                {
                    string sName = m_lstDownloadFile[i].GetFilename();
                    int uiCurrentPos = 0;
                    int iSize = 0;
                    if (NVSSDK.NetClient_NetFileGetDownloadPos(iConnID, ref uiCurrentPos, ref iSize) >= 0)
                    {
                        string sPos = "0%";
                        if (uiCurrentPos > 0 & uiCurrentPos < 100)
                        {

                            sPos = uiCurrentPos.ToString() + "%/";

                        }

                        sPos += iSize.ToString() + "dB";
                        m_lstDownloadFile[i].SetPosition(iSize);
                        for (int j = 0; j < lvFileData.Items.Count; j++)
                        {
                            string sFileName = lvFileData.Items[j].SubItems[1].Text.ToString();

                            if (sName == sFileName)
                            {
                                lvFileData.Items[j].SubItems[6].Text = sPos;
                                break;
                            }
                        }
                    }
                }  // enf of if (m_ulConnID != -1)
            } // end of for (int i = 0; i < m_lstDownloadFile.Count, i++)
        }

        private void lvFileData_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (e.Item.Checked)
            {
                foreach (ListViewItem item in lvFileData.CheckedItems)
                {
                    if (e.Item != item)
                    {
                        item.Checked = false;
                    }
                }
            }

        } 

    }
}
