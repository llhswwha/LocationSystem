using OPCAutomation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebApiLib.Clients.OpcCliect
{
    public class OPCClient1
    {
        public static  OPCServer KepServer;
        public  static  OPCGroups KepGroups;
        public static  OPCGroup KepGroup;
        public static  OPCItems KepItems;
        public static  OPCItem KepItem;
        static int itmHandleClient = 0;
        static  int itmHandleServer = 0;
        public static  string strHostIP = "192.168.1.32";

        public static  object readValue;

        public static  List<string> serverNames = new List<string>();
        public static  List<string> Tags = new List<string>();

        public OPCClient1()
        {
            
        }

        /// <summary>
        /// 枚举本地OPC SERVER
        /// </summary>
        public static void GetOPCServers()
        {
            //IPHostEntry IPHost = Dns.GetHostEntry(Environment.MachineName);

            //Console.WriteLine("MAC Address:");
            //foreach (IPAddress ip in IPHost.AddressList)
            //{
            //    Console.WriteLine(ip.ToString());
            //}
            //Console.WriteLine("Please Enter IPHOST");

           //Console.ReadLine();

            IPHostEntry ipHostEntry = Dns.GetHostEntry(strHostIP);
            try
            {
                KepServer = new OPCServer();
                object serverList = KepServer.GetOPCServers(ipHostEntry.HostName.ToString());
                int i = 0;
                foreach (string serverName in (Array)serverList)
                {
                    Console.WriteLine(i.ToString() + "." + serverName);
                    serverNames.Add(serverName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connect Error:" + ex.Message);
            }
        }

        /// <summary>
        /// 连接OPC SERVER
        /// </summary>
        /// <param name="serverName">OPC SERVER名字</param>
        public static  void ConnectServer(string serverName)
        {
            try
            {
                if (serverName == null||serverName=="")
                {
                    serverName = serverNames[0].ToString();
                }
                KepServer.Connect(serverName, "");
                CreateGroup("");
                CreateItems();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connect Error:" + ex.Message);
            }
        }

        /// <summary>
        /// 创建组,组名无所谓
        /// </summary>
        private static  void CreateGroup(string groupName)
        {
            try
            {
                KepGroups = KepServer.OPCGroups;
                KepGroup = KepGroups.Add(groupName);
                KepServer.OPCGroups.DefaultGroupIsActive = true;
                KepServer.OPCGroups.DefaultGroupDeadband = 0;
                KepGroup.UpdateRate = 250;
                KepGroup.IsActive = true;
                KepGroup.IsSubscribed = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create group error:" + ex.Message);
            }
        }

        private static  void CreateItems()
        {
            KepItems = KepGroup.OPCItems;
            KepGroup.DataChange += new DIOPCGroupEvent_DataChangeEventHandler(KepGroup_DataChange);
        }

        private static  void KepGroup_DataChange(int TransactionID, int NumItems, ref Array ClientHandles, ref Array ItemValues, ref Array Qualities, ref Array TimeStamps)
        {
            for (int i = 1; i <= NumItems; i++)
            {
                readValue = ItemValues.GetValue(i).ToString();
            }
        }

        private static  void GetTagValue(string tagName)
        {
            try
            {
                readValue = "";
                if (itmHandleClient != 0)
                {
                    Array Errors;
                    OPCItem bItem = KepItems.GetOPCItem(itmHandleServer);
                    //注：OPC中以1为数组的基数
                    int[] temp = new int[2] { 0, bItem.ServerHandle };
                    Array serverHandle = (Array)temp;
                    //移除上一次选择的项
                    KepItems.Remove(KepItems.Count, ref serverHandle, out Errors);
                }
                itmHandleClient = 12345;
                KepItem = KepItems.AddItem(tagName, itmHandleClient);
                itmHandleServer = KepItem.ServerHandle;
            }
            catch (Exception err)
            {
                //没有任何权限的项，都是OPC服务器保留的系统项，此处可不做处理。
                itmHandleClient = 0;
                Console.WriteLine("Read value error:" + err.Message);
            }
        }
        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="tagName"></param>
        /// <param name="_value"></param>
        public static void WriteValue(string tagName, object _value)
        {
            GetTagValue(tagName);
            OPCItem bItem = KepItems.GetOPCItem(itmHandleServer);
            int[] temp = new int[2] { 0, bItem.ServerHandle };
            Array serverHandles = (Array)temp;
            object[] valueTemp = new object[2] { "", _value };
            Array values = (Array)valueTemp;
            Array Errors;
            int cancelID;
            KepGroup.AsyncWrite(1, ref serverHandles, ref values, out Errors, 2009, out cancelID);
            //KepItem.Write(txtWriteTagValue.Text);//这句也可以写入，但并不触发写入事件
            GC.Collect();
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public static  object ReadValue(string tagName)
        {
            //GetTagValue(tagName);
            //Thread.Sleep(500);
            try
            {
                object result= KepItem.Value;
                return result;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public static  void ReadValue(string tagName, bool wtf)
        {
            GetTagValue(tagName);
            int aa = itmHandleServer;
            OPCItem bItem = KepItems.GetOPCItem(itmHandleServer);
            int[] temp = new int[2] { 0, bItem.ServerHandle };
            Array serverHandles = (Array)temp;
            Array Errors;
            int cancel;
            KepGroup.AsyncRead(1, ref serverHandles, out Errors, 2009, out cancel);
            GC.Collect();
        }


    }

}
