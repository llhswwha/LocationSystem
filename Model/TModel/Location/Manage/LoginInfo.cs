using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TModel.Location.Manage
{
    public class LoginInfo
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public bool IsEncrypted { get; set; }

        public string Session { get; set; }

        public string Authority { get; set; }

        public bool Result { get; set; }

        public string ClientIp { get; set; }

        public int ClientPort { get; set; }

        public DateTime LoginTime { get; set; }

        public DateTime LiveTime { get; set; }

        public override string ToString()
        {
            return string.Format("{0},{1}", UserName, Password);
        }

        public void SetSuccess(string authority)
        {
            Authority = authority;
            Session = Guid.NewGuid().ToString();
            Result = true;
            LoginTime = DateTime.Now;
            LiveTime= DateTime.Now;
        }
    }
}
