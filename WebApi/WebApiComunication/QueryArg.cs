using System.Collections.Generic;

namespace CommunicationClass
{
    public class QueryArg
    {
        public List<string> Args { get; set; }

        public QueryArg()
        {
            Args = new List<string>();
        }

        public void Add(string arg)
        {
            Args.Add(arg);
        }

        public void Add(string name,string value)
        {
            if (!string.IsNullOrEmpty(value))
                Args.Add(name + "=" + value);
        }

        public string GetQueryString()
        {
            string txt = "";
            if (Args.Count > 0)
            {
                for (int i = 0; i < Args.Count; i++)
                {
                    txt += Args[i];
                    if (i < Args.Count - 1)
                    {
                        txt += "&";
                    }
                }
                txt = "?" + txt;
            }
            return txt;
        }
    }
}
