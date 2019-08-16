using System.Data;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System;
using System.IO;

namespace LocationDaemon
{
    public class RegeditRW
    {
        public static bool ReadIsAutoRun()
        {
            //HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run
            RegistryKey user = Registry.CurrentUser;
            RegistryKey run=user.OpenSubKey("Software").OpenSubKey("Microsoft").OpenSubKey("Windows").OpenSubKey("CurrentVersion")
                .OpenSubKey("Run");
            object value=run.GetValue("LocationDaemon");
            if (value == null)
            {
                return false;
            }
            else
            {
                string path = value.ToString();
                string path2 = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                if (File.Exists(path) && path == path2)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool SetIsAutoRun(bool isAuto)
        {
            try
            {
                string path2 = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;

                //HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run
                RegistryKey user = Registry.CurrentUser;
                RegistryKey run = user.OpenSubKey("Software").OpenSubKey("Microsoft").OpenSubKey("Windows").OpenSubKey("CurrentVersion")
                    .OpenSubKey("Run",true);
                if (isAuto)
                {
                    object value = run.GetValue("LocationDaemon");
                    if (value == null)
                    {
                        run.SetValue("LocationDaemon", path2);
                    }
                    else
                    {
                        run.SetValue("LocationDaemon", path2);
                    }
                }
                else
                {
                    run.DeleteValue("LocationDaemon");
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        /// <summary>
            /// 注册列表读取数据
            /// </summary>
            /// <param name="strPath">路径</param>
            /// <param name="strName">注册列表项名称</param>
            /// <returns></returns>
            public static string ReadRegedit(string strPath, string strName)
        {
            try
            {
                string registData;
                RegistryKey Local = Registry.LocalMachine;
                RegistryKey software = Local.OpenSubKey("SOFTWARE", true);
                RegistryKey 机房监控 = software.OpenSubKey("机房监控", true);
                RegistryKey Bucket = 机房监控.OpenSubKey(strPath, true);
                registData = Bucket.GetValue(strName).ToString();
                if (registData == "")
                    return null;
                else
                    return Dec(registData);

            }
            catch
            {
                //MessageBox.Show(ex.ToString(), SampleColor.ClientUI.GlobalUserClass.strCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }

        
        /// <summary>
        /// 读取登陆用户表格
        /// </summary>
        /// <param name="strPath">路径</param>
        /// <returns></returns>
        public static DataTable ReadRegeditUser(string strPath)
        {
            DataTable dt =new DataTable();
            dt.Columns.Add("id", typeof(string));
            dt.Columns.Add("user", typeof(string));
            DataRow dr;
            try
            {
                RegistryKey Local = Registry.LocalMachine;
                RegistryKey software = Local.OpenSubKey("SOFTWARE", true);
                RegistryKey 机房监控 = software.OpenSubKey("机房监控", true);
                RegistryKey Bucket = 机房监控.OpenSubKey(strPath, true);
                for (int i = 0; i < Bucket.GetValueNames().Count(); i++)
                {
                    dr = dt.NewRow();
                    dr[0] = Bucket.GetValueNames()[i].ToString();
                    dr[1] =Dec(Bucket.GetValue(Bucket.GetValueNames()[i]).ToString());
                    dt.Rows.Add(dr);
                }

            }
            catch
            {
                //MessageBox.Show(ex.ToString(), DyColor.ClientUI.GlobalUserClass.strCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return dt;
        }

       
        /// <summary>
        /// 读取登陆服务器表格
        /// </summary>
        /// <param name="strPath">路径</param>
        /// <returns></returns>
        public static DataTable ReadRegeditServerIP(string strPath)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(string));
            dt.Columns.Add("serverIP", typeof(string));
            DataRow dr;
            try
            {
                RegistryKey Local = Registry.LocalMachine;
                RegistryKey software = Local.OpenSubKey("SOFTWARE", true);
                RegistryKey 机房监控 = software.OpenSubKey("机房监控", true);
                RegistryKey Bucket = 机房监控.OpenSubKey(strPath, true);
                for (int i = 0; i < Bucket.GetValueNames().Count(); i++)
                {
                    dr = dt.NewRow();
                    dr[0] = Bucket.GetValueNames()[i].ToString();
                    dr[1] = Dec(Bucket.GetValue(Bucket.GetValueNames()[i]).ToString());
                    dt.Rows.Add(dr);
                }

            }
            catch
            {
                //MessageBox.Show(ex.ToString(), DyColor.ClientUI.GlobalUserClass.strCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return dt;
        }

        
        /// <summary>
        /// 注册列表删除数据
        /// </summary>
        /// <param name="strPath">路径</param>
        /// <param name="strName">注册列表项名称</param>
        /// <returns></returns>
        public static bool DeleteRegedit(string strPath, string strName)
        {
            try
            {
                RegistryKey Local = Registry.LocalMachine;
                RegistryKey software = Local.OpenSubKey("SOFTWARE", true);
                RegistryKey 机房监控 = software.OpenSubKey("机房监控", true);
                RegistryKey key = 机房监控.OpenSubKey(strPath, true);
                key.DeleteValue(strName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 注册列表删除数据
        /// </summary>
        /// <param name="strPath">路径</param>
        /// <returns></returns>
        public static bool DeleteRegedit(string strPath)
        {
            try
            {
                RegistryKey Local = Registry.LocalMachine;
                RegistryKey software = Local.OpenSubKey("SOFTWARE", true);
                RegistryKey 机房监控 = software.OpenSubKey("机房监控", true);
                机房监控.DeleteSubKey(strPath);
                return true;
            }
            catch
            {
                return false;
            }
        }

       /// <summary>
       /// 注册列表写入数据
       /// </summary>
       /// <param name="strPath">路径</param>
        /// <param name="strName">注册列表项名称</param>
        /// <param name="strNum">注册列表项值</param>
       /// <returns></returns>
        public static bool WriteRegedit(string strPath, string strName, string strNum)
        {
            try
            {
                RegistryKey key = IsRegeditExit("机房监控", strPath);
                if (key == null)
                {
                    RegistryKey Local = Registry.LocalMachine.OpenSubKey("Software", true);
                    RegistryKey newkey = Local.CreateSubKey("机房监控");
                    key = newkey.CreateSubKey(strPath);//BucketSet
                }
                key.SetValue(strName, Enc(strNum));
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

       
       /// <summary>
        /// 判断注册列表项是否存在
       /// </summary>
       /// <param name="strPath">路径</param>
        /// <param name="name">注册列表项名称</param>
       /// <returns></returns>
        private static RegistryKey IsRegeditExit(string strPath, string name)
        {
            try
            {
                string[] subkeyNames;
                RegistryKey hkml = Registry.LocalMachine;
                RegistryKey software = hkml.OpenSubKey("SOFTWARE", true);
                RegistryKey 机房监控 = software.OpenSubKey("机房监控", true);
                RegistryKey key = 机房监控.OpenSubKey(strPath, true);
                if (key == null) return null;
                subkeyNames = key.GetValueNames();
                foreach (string keyName in subkeyNames)
                {
                    if (keyName == name)
                    {
                        return key;
                    }
                }
            }
            catch
            {
            }
            return null;
        }

        /// <summary>
        /// 密匙
        /// </summary>
        private static int[] XorKey = new int[] { 178, 9, 170, 85, 147, 109, 132, 71 };

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="strMsg">加密字符串</param>
        /// <returns></returns>
        private static string Enc(string strMsg)
        {
            string strRet = "";
            byte[] bytMsg = Encoding.Default.GetBytes(strMsg);

            for (int i = 0, j = 0; i < Encoding.Default.GetBytes(strMsg).Length; i++)
            {
                strRet += string.Format("{0:x2}", bytMsg[i] ^ XorKey[j]);
                j = (j + 1) % 8;
            }

            return strRet;
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="strMsg">解密字符串</param>
        /// <returns></returns>
        private static string Dec(string strMsg)
        {
            byte[] bytMsg = new byte[strMsg.Length / 2];

            for (int i = 0, j = 0; i < (strMsg.Length / 2); i++)
            {

                char[] str = new char[2];
                strMsg.CopyTo(i * 2, str, 0, 2);

                int n1 = 0, n2 = 0;

                if ((str[0] == '0') || (str[0] == '1') || (str[0] == '2') || (str[0] == '3')
                    || (str[0] == '4') || (str[0] == '5') || (str[0] == '6') || (str[0] == '7')
                    || (str[0] == '8') || (str[0] == '9'))
                {
                    n1 = str[0] - 48;
                }

                if ((str[0] == 'a') || (str[0] == 'b') || (str[0] == 'c') || (str[0] == 'd')
                    || (str[0] == 'e') || (str[0] == 'f'))
                {
                    n1 = str[0] - 87;
                }

                if ((str[0] == 'A') || (str[0] == 'B') || (str[0] == 'C') || (str[0] == 'D')
                    || (str[0] == 'E') || (str[0] == 'F'))
                {
                    n1 = str[0] - 55;
                }

                if ((str[1] == '0') || (str[1] == '1') || (str[1] == '2') || (str[1] == '3')
                    || (str[1] == '4') || (str[1] == '5') || (str[1] == '6') || (str[1] == '7')
                    || (str[1] == '8') || (str[1] == '9'))
                {
                    n2 = str[1] - 48;
                }

                if ((str[1] == 'a') || (str[1] == 'b') || (str[1] == 'c') || (str[1] == 'd')
                    || (str[1] == 'e') || (str[1] == 'f'))
                {
                    n2 = str[1] - 87;
                }

                if ((str[1] == 'A') || (str[1] == 'B') || (str[1] == 'C') || (str[1] == 'D')
                    || (str[1] == 'E') || (str[1] == 'F'))
                {
                    n2 = str[1] - 55;
                }

                int nValue = n1 * 16 + n2;

                int tmp = (nValue ^ XorKey[j]);

                int a10 = tmp / 16;
                string str10 = "";
                switch (a10)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9: str10 = a10.ToString(); break;
                    case 10: str10 = "a"; break;
                    case 11: str10 = "b"; break;
                    case 12: str10 = "c"; break;
                    case 13: str10 = "d"; break;
                    case 14: str10 = "e"; break;
                    case 15: str10 = "f"; break;
                    default: break;
                }

                int a1 = tmp % 16;
                string str1 = "";
                switch (a1)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9: str1 = a1.ToString(); break;
                    case 10: str1 = "a"; break;
                    case 11: str1 = "b"; break;
                    case 12: str1 = "c"; break;
                    case 13: str1 = "d"; break;
                    case 14: str1 = "e"; break;
                    case 15: str1 = "f"; break;
                    default: break;
                }

                bytMsg[i] = byte.Parse(str10 + str1, System.Globalization.NumberStyles.AllowHexSpecifier);

                j = (j + 1) % 8;
            }

            return Encoding.Default.GetString(bytMsg);
        }

    }
}
