using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace DbModel.Tools
{
    public static class IpHelper
    {
        /// <summary>
        /// 获取当前电脑上的所有IP
        /// </summary>
        /// <returns></returns>
        public static List<IPAddress> GetLocalList()
        {
            string name = Dns.GetHostName();
            IPAddress[] ipadrlist = Dns.GetHostAddresses(name);
            var list = new List<IPAddress>();
            list.Add(IPAddress.Parse("127.0.0.1"));
            list.AddRange(ipadrlist);
            return list;
        } 
        
        /// <summary>
        /// 获取和ip同一网段的当前电脑上的Ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static IPAddress GetLocalIp(string ip)
        {
            var i = ip.IndexOf('.');
            var ipId = ip.Substring(0, i);
            var ips = GetLocalList();
            foreach (var item in ips)
            {
                if (item.ToString().StartsWith(ipId))
                {
                    return item;
                }
            }
            return null;
        }

        public static List<IPAddress> GetIpRange(string ipStart,string ipEnd)
        {
            var ipAddressList = new List<IPAddress>();
            var ips = GetIPS(ipStart, ipEnd);
            foreach (var item in ips)
            {
                var ip = IPAddress.Parse(item);
                ipAddressList.Add(ip);
            }
            return ipAddressList;
        }

        public static bool IsSameDomain(string ip1,string ip2)
        {
            string[] parts1 = ip1.Split('.');
            string[] parts2 = ip2.Split('.');
            if (parts1.Length == 4 && parts2.Length == 4)
            {
                return parts1[0] == parts2[0] && parts1[1] == parts2[1] && parts1[2] == parts2[2];
            }
            return true;
        }

        /// <summary>
        /// IP转化为数字（IPV4）
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static long IpToInt(this string ip)
        {
            try
            {
                char[] separator = new char[] { '.' };
                string[] items = ip.Split(separator);
                return long.Parse(items[0]) << 24
                       | long.Parse(items[1]) << 16
                       | long.Parse(items[2]) << 8
                       | long.Parse(items[3]);
            }
            catch (Exception ex)
            {
                //Debug.WriteLine(ip);
                //Debug.WriteLine(ex.ToString());
                return 0;
            }

        }


        /// <summary>
        /// 生成两个Ip之间的所有Ip（IPV4）
        /// </summary>
        /// <param name="ipStart"></param>
        /// <param name="ipEnd"></param>
        /// <returns></returns>
        public static List<string> GetIPS(string ipStart, string ipEnd)
        {
            List<string> result = new List<string>();
            long start = ipStart.IpToInt();
            long end = ipEnd.IpToInt();
            for (long i = start; i <= end; i++)
            {
                string ip = i.IntToIp();
                result.Add(ip);
            }
            return result;
        }
        ///// <summary>
        ///// 生成两个Ip之间的所有Ip（IPV6）
        ///// </summary>
        ///// <param name="ipStart"></param>
        ///// <param name="ipEnd"></param>
        ///// <returns></returns>
        //public static List<string> GetIPV6_IPS(string ipStart, string ipEnd)
        //{
        //    List<string> result = new List<string>();
        //    BigInteger start = ipv6ToInt(ipStart);
        //    BigInteger end = ipv6ToInt(ipEnd);
        //    for (BigInteger i = start; BigInteger.Subtract(end, i) >= 0; i += 1)
        //    {
        //        string ip = BigIntToString(i);
        //        result.Add(ip);
        //    }


        //    return result;
        //}
        /// <summary>
        /// 数字转化IP(IPV4)
        /// </summary>
        /// <param name="ipInt"></param>
        /// <returns></returns>
        public static string IntToIp(this long ipInt)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append((ipInt >> 24) & 0xFF).Append(".");
            sb.Append((ipInt >> 16) & 0xFF).Append(".");
            sb.Append((ipInt >> 8) & 0xFF).Append(".");
            sb.Append(ipInt & 0xFF);
            return sb.ToString();
        }
        ///// <summary>
        ///// IP转化数字（IPV6）
        ///// </summary>
        ///// <param name="ip"></param>
        ///// <returns></returns>
        //public static BigInteger ipv6ToInt(string ip)
        //{
        //    IPAddress IP = IPAddress.Parse(ip);
        //    List<byte> ipFormat = IP.GetAddressBytes().ToList();
        //    ipFormat.Reverse();
        //    ipFormat.Add(0);
        //    BigInteger ipAsInt = new BigInteger(ipFormat.ToArray());
        //    return ipAsInt;
        //}

        ///// <summary>
        ///// 数字转化为IP（IPV6）
        ///// </summary>
        ///// <param name="ipInBigInt"></param>
        ///// <returns></returns>
        //public static string BigIntToString(BigInteger ipInBigInt)
        //{
        //    try
        //    {
        //        List<byte> bytes = ipInBigInt.ToByteArray().ToList();
        //        bytes.RemoveAt(bytes.Count - 1);
        //        bytes.Reverse();

        //        IPAddress IP = new IPAddress(bytes.ToArray());
        //        return IP.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.ToString());
        //    }
        //}



        /// <summary>
        /// 获取IP段之间的数量（IPV4）
        /// </summary>
        /// <param name="ipStart"></param>
        /// <param name="ipEnd"></param>
        /// <returns></returns>
        public static long GetIPCount(string ipStart, string ipEnd)
        {
            //if (ValidateIPAddress(ipStart) == false) return 0;
            //if (ValidateIPAddress(ipEnd) == false) return 0;
            long start = IpToInt(ipStart);
            long end = IpToInt(ipEnd);
            if (start > end) return -1;
            return end - start + 1;
        }
        ///// <summary>
        /////  获取IP段之间的数量（IPV6）
        ///// </summary>
        ///// <param name="ipend"></param>
        ///// <param name="ipstart"></param>
        ///// <returns></returns>
        //public static long GetIPV6Count(BigInteger ipend, BigInteger ipstart)
        //{
        //    BigInteger number = BigInteger.Subtract(ipend, ipstart);
        //    return Convert.ToInt64(number.ToString()) + 1;
        //}

        public static bool IsSameIPRange(string ipStart, string ipEnd, int count)
        {
            string[] parts1 = ipStart.Split('.');
            string[] parts2 = ipEnd.Split('.');
            for (int i = 0; i < parts1.Length && i < count; i++)
            {
                if (parts1[i] != parts2[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
