using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace SimpleChat2
{
    public static class GravatarHelpers
    {
        public static string GetImage(string email)
        {
            const string urlBase = "http://www.gravatar.com/avatar.php?gravatar_id={0}&size=20";
            if(email==null||!email.Contains("@"))
            {
                return string.Format(urlBase, "0");
            }
            byte[] has = new MD5CryptoServiceProvider().ComputeHash(new UTF8Encoding().GetBytes(email.ToLower()));
            var str = new StringBuilder(has.Length * 2);
            foreach (var item in has)
            {
                str.Append(item.ToString("X2"));
            }
            return string.Format(urlBase, str.ToString().ToLower());
        }
    }
}