using System;
using System.Collections.Generic;
using System.Net;

namespace DbModel.Tools
{
    public static class IpHelper
    {
        public static List<IPAddress> GetLocalList()
        {
            string name = Dns.GetHostName();
            IPAddress[] ipadrlist = Dns.GetHostAddresses(name);
            var list = new List<IPAddress>();
            list.Add(IPAddress.Parse("127.0.0.1"));
            list.AddRange(ipadrlist);
            return list;
        } 
    }
}
