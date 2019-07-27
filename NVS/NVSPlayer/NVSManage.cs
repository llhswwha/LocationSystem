using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using PlaybackDemo;

namespace NVSPlayer
{
    public static class NVSManage
    {
        public static string RTMP_Host = "192.168.108.109";
        public static string NVRIP = "192.168.108.107";
        public static string NVRPort = "3000";
        public static string NVRUser = "Admin";
        public static string NVRPass = "1111add";
        public static Dictionary<string,PlayBackForm> FormDict=new Dictionary<string, PlayBackForm>();

        public static PlayBackForm Form;

        public static void Init()
        {
            Form = Init(NVRIP);
        }
        private static PlayBackForm Init(string ip)
        {
            PlayBackForm form = new PlayBackForm();
            form.SetLoginInfo(ip,NVRPort,NVRUser,NVRPass);
            //form.Login();
            form.AutoLogin = true;
            form.IsShowMessageBox = false;
            form.Show();

            if (FormDict.ContainsKey(ip))
            {
                FormDict[ip] = form;
            }
            else
            {
                FormDict.Add(ip, form);
            }
            return form;
        }

        ///// <summary>
        ///// 从缓存中取
        ///// </summary>
        ///// <param name="ip"></param>
        ///// <returns></returns>
        //public static PlayBackForm GetPlayBackForm(string ip)
        //{
        //    if (FormDict.ContainsKey(ip))
        //    {
        //        return FormDict[ip];
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        public static void GetPlayBackFormEx(string ip,Action<PlayBackForm,bool> callback)
        {
            var ip2 = Form.GetIp();
            if (ip2 != ip)//不同Ip,切换登录
            {
                Form.SetIp(ip);
                Form.AfterLogin = callback;
                Form.Logout();
                //Thread.Sleep(50);
                Form.Login();
                //Thread.Sleep(50);
            }
            else
            {
                if (callback != null)
                {
                    callback(Form,true);
                }
            }
        }

        public static void Stop()
        {
            foreach (KeyValuePair<string, PlayBackForm> pair in FormDict)
            {
                pair.Value.Close();
            }
        }
    }
}
