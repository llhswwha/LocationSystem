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
using System.Diagnostics;
using System.Threading;

namespace NetClient
{
    public partial class FormDSM : Form
    {
        private const int REG_PAGE_SIZE = 20;
        private int m_iServerID;
        private DNSList_NOTIFY DnsNotify = null;
        private NVSList_NOTIFY NvsNotify = null;

        //连接到注册中心服务器的ID号
        public int ServerID
        {
            get { return m_iServerID; }
            set { m_iServerID = value; }
        }
        private string m_strServerIP;

        //注册中心服务器IP
        public string ServerIP
        {
            get { return m_strServerIP; }
            set { m_strServerIP = value; }
        }       
        private int m_iServerPort;

        //注册中心服务器的端口号
        public int ServerPort
        {
            get { return m_iServerPort; }
            set { m_iServerPort = value; }
        }        
        private string m_strUserName;

        //连接注册中心服务器的用户名
        public string UserName
        {
            get { return m_strUserName; }
            set { m_strUserName = value; }
        }        
        private string m_strPassword;

        //连接注册中心服务器的密码
        public string Password
        {
            get { return m_strPassword; }
            set { m_strPassword = value; }
        }       
        public FormDSM()
        {
            InitializeComponent();
            //Control.CheckForIllegalCrossThreadCalls = false;
        }

        //将字节数组转化为字符串
        private string Bytes2Str(byte[] _btData)
        {
            //获得字节数组中字节0的位址
            int ilen = Array.IndexOf<byte>(_btData,0);
            if (ilen < 0)
            {
                ilen = _btData.Length;
            }

            //从字节数组获得字符串
            return Encoding.Default.GetString(_btData,0, ilen);
        }

        //进行NVS查询
        private void btnNVSQuery_Click(object sender, EventArgs e)
        {
            //NVS ID 为空时,不能查询
            if (textNVSID.Text.Trim() == "")
            {
                MessageBox.Show("NVS ID 不能为空 ! ");
                return;
            }

            //创建结构体
            REG_NVS stRegNVS = new REG_NVS();

            //进行Byte数组赋值
            Array.Copy(Encoding.ASCII.GetBytes(textNVSID.Text), stRegNVS.m_stNvs.m_btFactoryID, textNVSID.Text.Length);

            //清空DataGridView内容
            dgvNVS.Rows.Clear();

            int iRet = -1;
            IntPtr pNvs = IntPtr.Zero;
            try
            {
                //申请结构体REG_NVS所需的内存
                pNvs = Marshal.AllocHGlobal(Marshal.SizeOf(stRegNVS));

                //将stRegNVS存入刚申请的内存
                Marshal.StructureToPtr(stRegNVS, pNvs, true);

                //查询NVS设备信息
                iRet = NVSSDK.NSLook_Query(ServerID, IntPtr.Zero, pNvs, NVSSDK.TYPE_NVS);

                //从内存中读出stRegNVS的值
                stRegNVS = (REG_NVS)Marshal.PtrToStructure(pNvs, typeof(REG_NVS));
            }
            catch (Exception ex)
            {
                //将异常输出到Output窗口
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                //释放内存
                Marshal.FreeHGlobal(pNvs);
            }

            //查询失败，退出
            if (iRet != 0)
            {
                MessageBox.Show("NSLook_Query NVS Error ! " + iRet);
                return;
            }
           
            //为DatagGridView添加新行
            dgvNVS.Rows.Add
            (
                new object[] 
                { 
                    Bytes2Str(stRegNVS.m_stNvs.m_btNvsIP),
                    Bytes2Str(stRegNVS.m_stNvs.m_btNvsName), 
                    stRegNVS.m_stNvs.m_iNvsType, 
                    Bytes2Str(stRegNVS.m_stNvs.m_btFactoryID)
                }
            );
        }

        //获取NVS设备列表 
        private void btnNVSRefresh_Click(object sender, EventArgs e)
        {
            int iCount = 0;

            //获取NVS设备数量
            int iRet = NVSSDK.NSLook_GetCount
                       (
                            ServerID,
                            Encoding.ASCII.GetBytes(UserName), 
                            Encoding.ASCII.GetBytes(Password), 
                            ref iCount, NVSSDK.TYPE_NVS
                       );
            if ( iRet != 0)
            {
                MessageBox.Show("NSLook_GetCount NVS Error ! " + iRet);
                textNVSCount.Text = "0";
                return;
            }
            textNVSCount.Text = iCount.ToString();

            //将回调保存，防止垃圾回收，出现异常
            NvsNotify = NVSListNotify; 
            if (iCount > 0)
            {
                //清空DataGridView的内容
                dgvNVS.Rows.Clear();

                //获取NVS列表
                iRet = NVSSDK.NSLook_GetList
                       (
                           ServerID,
                           Encoding.ASCII.GetBytes(UserName),
                           Encoding.ASCII.GetBytes(Password),
                           0,
                           null,
                           NvsNotify,
                           NVSSDK.TYPE_NVS
                       );   

                //操作失败       
                if (iRet != 0)
                {
                    MessageBox.Show("NSLook_GetList NVS Error ! " + iRet);
                }
            }
        }

        //获取NVS列表的回调函数,必须使用IntPtr，否则传值失败
        private void NVSListNotify(Int32 _iCount, IntPtr _pNvs)
        {
            
            for (int i = 0; i < _iCount; i++)
            {   
                NvsSingle stNvs = new NvsSingle();

                //指针后移，读取下一个NvsSingle结构体
                IntPtr pNvs = (IntPtr)((UInt32)_pNvs + i * Marshal.SizeOf(stNvs));

                //读取NvsSingle结构体数据
                stNvs = (NvsSingle)Marshal.PtrToStructure(pNvs, typeof(NvsSingle));
                string strNvsIP = Bytes2Str(stNvs.m_btNvsIP);
                string strNvsName = Bytes2Str(stNvs.m_btNvsName);
                int iNvsType = stNvs.m_iNvsType;
                string strFactoryID = Bytes2Str(stNvs.m_btFactoryID);

                //创建匿名委托，以处理跨线程修改DataGridView控件属性
                MethodInvoker notify = delegate()
                {
                    //为DataGridView添加新行
                    dgvNVS.Rows.Add
                    (
                         new object[]
                         { 
                            strNvsIP,
                            strNvsName,
                            iNvsType,
                            strFactoryID
                         }
                    );

                    //刷新DataGridView
                    dgvNVS.Invalidate();
                };

                //将匿名委托交给DataGridView控件处理
                dgvNVS.Invoke(notify);              
            }            
        }

        private void rdoDNSID_CheckedChanged(object sender, EventArgs e)
        {
            textDNSDomainName.Text = "";
            textDNSID.ReadOnly = false;
            textDNSDomainName.ReadOnly = true; 
        }

        private void rdoDNSDomainName_CheckedChanged(object sender, EventArgs e)
        {
            textDNSID.Text = "";
            textDNSID.ReadOnly = true;
            textDNSDomainName.ReadOnly = false;           
        }

        //进行DNS查询操作
        private void btnDNSQuery_Click(object sender, EventArgs e)
        {
            //创建REG_DNS结构体
            REG_DNS stRegDNS = new REG_DNS();

            //以ID方式进行查询
            if (rdoDNSID.Checked)
            {
                if (textDNSID.Text.Trim() == "")
                {
                    MessageBox.Show("DNS ID 不能为空 ! ");
                    return;
                }

                //Byte数组进行赋值
                Array.Copy(Encoding.ASCII.GetBytes(textDNSID.Text), stRegDNS.m_stDNSInfo.m_stNvs.m_btFactoryID, textDNSID.Text.Length);
            }
            else
            {
                //以域名方式进行查询
                if (textDNSDomainName.Text.Trim() == "")
                {
                    MessageBox.Show("DNS DomainName 不能为空 ! ");
                    return;
                }

                //Byte数组进行赋值
                Array.Copy(Encoding.ASCII.GetBytes(textDNSDomainName.Text), stRegDNS.m_stDNSInfo.m_stNvs.m_btNvsName, textDNSDomainName.Text.Length);
            }
            Array.Copy(Encoding.ASCII.GetBytes(UserName), stRegDNS.m_stDNSInfo.m_btUserName, UserName.Length);
            Array.Copy(Encoding.ASCII.GetBytes(Password), stRegDNS.m_stDNSInfo.m_btPwd, Password.Length); 
           
            //清空DataGridView
            dgvDNS.Rows.Clear();

            int iRet = -1;
            IntPtr pDns = IntPtr.Zero;
            try
            {
                //申请结构体REG_DNS所需的内存
                pDns = Marshal.AllocHGlobal(Marshal.SizeOf(stRegDNS));

                //将stRegDNS存入刚申请的内存
                Marshal.StructureToPtr(stRegDNS, pDns, true);

                //查询DNS设备信息
                iRet = NVSSDK.NSLook_Query(ServerID, pDns, IntPtr.Zero, NVSSDK.TYPE_DNS);

                //从内存中读出stRegDNS的值
                stRegDNS = (REG_DNS)Marshal.PtrToStructure(pDns, typeof(REG_DNS));                
            }
            catch(Exception ex)
            {
                //将异常输出到Output窗口
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                //释放内存
                Marshal.FreeHGlobal(pDns);
            }

            //操作失败
            if (iRet != 0)
            {
                MessageBox.Show("NSLook_Query DNS Error ! " + iRet);
                return;
            }
            
            //为DataGridView添加新行
            dgvDNS.Rows.Add
            (
                new object[] 
                { 
                    Bytes2Str(stRegDNS.m_stDNSInfo.m_stNvs.m_btNvsName),
                    Bytes2Str(stRegDNS.m_stDNSInfo.m_stNvs.m_btFactoryID),
                    Bytes2Str(stRegDNS.m_stDNSInfo.m_stNvs.m_btNvsIP),
                    Bytes2Str(stRegDNS.m_stDNSInfo.m_stReserve.m_btReserved1),
                    stRegDNS.m_stDNSInfo.m_iPort,
                    stRegDNS.m_stDNSInfo.m_stNvs.m_iNvsType,
                    stRegDNS.m_stDNSInfo.m_iChannel,
                    "",
                    "",
                    DateTime.Now
                }
            );
        }

        //更新页码
        private void btnDNSRefresh_Click(object sender, EventArgs e)
        {
            cboDNSPage.SelectedIndex = -1;
            int iCount = 0;

            //获取DNS设备数量
            int iRet = NVSSDK.NSLook_GetCount
                       (
                            ServerID,
                            Encoding.ASCII.GetBytes(UserName),
                            Encoding.ASCII.GetBytes(Password),
                            ref iCount, NVSSDK.TYPE_DNS
                       );

            //获取失败退出
            if (iRet != 0)
            {
                MessageBox.Show("NSLook_GetCount DNS Error ! " + iRet);
                textDNSCount.Text = "0";
                return;
            }
            cboDNSPage.Items.Clear();
            if (iCount <= 0)
            {
                textDNSCount.Text = "0";
                return;
            }
            textDNSCount.Text = iCount.ToString();
            int iPage = 0;

            //计算页数
            iPage = iCount / REG_PAGE_SIZE + iCount % REG_PAGE_SIZE == 0 ? 0 : 1;

            //为页码下拉菜单添加子项
            for (int i = 0; i < iPage; i++)
            {
                cboDNSPage.Items.Add(i + 1);
            }

            //默认选择第一页
            cboDNSPage.SelectedIndex = 0;            
        }

        //选择页码，更新DataGridView列表
        private void cboDNSPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iPage = cboDNSPage.SelectedIndex;
            if (iPage < 0)
            {
                return;
            }
            dgvDNS.Rows.Clear();

            //将回调保存，防止垃圾回收，出现异常
            DnsNotify = DNSListNotify;

            //获取DNS设备列表
            int iRet = NVSSDK.NSLook_GetList
            (
                ServerID,
                Encoding.ASCII.GetBytes(UserName),
                Encoding.ASCII.GetBytes(Password),
                iPage,
                DnsNotify,
                null,
                NVSSDK.TYPE_DNS
            );

            //刷新DataGridView
            dgvDNS.Invalidate();
            
            if (iRet != 0)
            {
                MessageBox.Show("NSLook_GetList DNS Error ! " + iRet);
            }
        }

        //获取DNS列表的回调函数
        private void DNSListNotify(Int32 _iCount,IntPtr _pDns)
        {
            for (int i = 0; i < _iCount; i++)
            {
                //创建结构体REG_DNS
                REG_DNS stDns = new REG_DNS();

                //指针下移，读取下一数据
                IntPtr pDns = (IntPtr)((UInt32)_pDns + i * Marshal.SizeOf(stDns));
                stDns = (REG_DNS)Marshal.PtrToStructure(pDns, typeof(REG_DNS));
                string strNvsName = Bytes2Str(stDns.m_stDNSInfo.m_stNvs.m_btNvsName);
                string strFactoryID = Bytes2Str(stDns.m_stDNSInfo.m_stNvs.m_btFactoryID);
                string strLANIP = Bytes2Str(stDns.m_stDNSInfo.m_stNvs.m_btNvsIP);
                string strWANIP = Bytes2Str(stDns.m_stDNSInfo.m_stReserve.m_btReserved1);
                int iPort = stDns.m_stDNSInfo.m_iPort;
                int iNvsType = stDns.m_stDNSInfo.m_stNvs.m_iNvsType;
                int iChannel = stDns.m_stDNSInfo.m_iChannel;
                string strAccount = "";
                string strPwd = "";

                //创建匿名委托，以处理跨线程修改DataGridView控件属性
                MethodInvoker notify = delegate()
                {
                    //为DataGridView创建新行
                    dgvDNS.Rows.Add
                    (
                         new object[]
                         { 
                            strNvsName,
                            strFactoryID,
                            strLANIP,
                            strWANIP,
                            iPort,
                            iNvsType,
                            iChannel,
                            strAccount,
                            strPwd,
                            DateTime.Now
                         }
                    );  
                };

                //将匿名委托交给DataGridView控件处理
                dgvDNS.Invoke(notify);                
            }            
        }

        //单击DataGridView控件dgvNVS的单元事件处理函数
        private void dgvNVS_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //修改textNVSID的ID值
                textNVSID.Text = dgvNVS.Rows[e.RowIndex].Cells["ID"].Value.ToString();
            }
            catch
            {
                textNVSID.Text = "";
            }
        }

        //双击DataGridView控件dgvNVS的单元事件处理函数
        private void dgvNVS_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //获得被双击的网络视频服务器的IP地址
            string strIP = dgvNVS.Rows[e.RowIndex].Cells["NVSIP"].Value.ToString(); 
            if (strIP.Trim() == "")
            {
                return;
            }

            //进行主窗体中网络视频服务器的登陆操作
            ((ClientForm)this.Owner).Logon(strIP);
        }

        //单击DataGridView控件dgvDNS的单元事件处理函数
        private void dgvDNS_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (rdoDNSID.Checked)
                {
                    //修改textDNSID的ID值
                    textDNSID.Text = dgvDNS.Rows[e.RowIndex].Cells["DNSID"].Value.ToString();
                }
                else
                {
                    //修改textDNSDomainName的域名
                    textDNSDomainName.Text = dgvDNS.Rows[e.RowIndex].Cells["DNSName"].Value.ToString();
                }
            }
            catch
            {
                textDNSID.Text = "";
                textDNSDomainName.Text = "";
            }
        }

        //双击DataGridView控件dgvDNS的单元事件处理函数
        private void dgvDNS_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //获得被双击的网络视频服务器的IP地址
            string strIP = dgvDNS.Rows[e.RowIndex].Cells["LANIP"].Value.ToString();
            if (strIP.Trim() == "")
            {
                return;
            }

            //进行主窗体中网络视频服务器的登陆操作
            ((ClientForm)this.Owner).Logon(strIP);            
        }

        //关闭窗体
        private void FormDSM_FormClosed(object sender, FormClosedEventArgs e)
        {
            //修改主窗体的formDSM域为空
            ((ClientForm)this.Owner).LogofServer();
        }
    }
}
