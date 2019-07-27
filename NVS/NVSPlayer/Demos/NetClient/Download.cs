using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetClient
{
    public class Download
    {
        private	string m_szFileName;
	    private int m_iLogonID;
	    private UInt32 m_ulConnID;
	    private string m_szOperationTime;
	    private string m_szOperate;
	    private int m_iBreakContine;
	    private int m_iReqMode;
	    private int m_iPosition;
        public Download(int _iLogonID, UInt32 _ulConnID,string _szFileName,string _szOperationTime,string _szOperate)
        {
            m_ulConnID = _ulConnID;
            m_iLogonID = _iLogonID;
            m_szFileName = _szFileName;
            m_szOperationTime = _szOperationTime;
            m_szOperate = _szOperate;
            m_iBreakContine = 0;
            m_iReqMode = 1;
            m_iPosition = 0;
        }


        public void StopDownload()
        {
            int iret = NVSSDK.NetClient_NetFileStopDownloadFile(m_ulConnID);
            if (iret < 0)
            {
                MessageBox.Show("NetClient_NetFileStopDownloadFile faild!");
            }
        }

        public int GetLogonID()
        {
            return m_iLogonID;
        }

        public UInt32 GetConnID()
        {
            return m_ulConnID;
        }

        public string GetFilename()
        {
            return m_szFileName;
        }

        public string GetOperationTime()
        {
            return m_szOperationTime;
        }

        public string GetOperate()
        {
            return m_szOperate;
        }

        public int GetBreakContinue()
        {
            return m_iBreakContine;
        }

        public int SetBreakContinue( int _iFlag )
        {
            m_iBreakContine = _iFlag;
            return 0;
        }

        public int GetReqMode()
        {
            return m_iReqMode;
        }

        public int SetReqMode(int _iMode)
        {
            m_iReqMode = _iMode;
            return 0;
        }

        public int SetConnID( UInt32 _ulConnID )
        {
            m_ulConnID = _ulConnID;
            return 0;
        }

        public int GetPosition()
        {
            return m_iPosition;
        }

        public int SetPosition( int _iPos )
        {
            m_iPosition = _iPos;
            return 0;
        }
    }
}
