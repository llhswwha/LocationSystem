using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TModel.FuncArgs
{
    [DataContract]
    [Serializable]
    public class SearchArg
    {
        [DataMember]
        public string StartTime { get; set; }

        [DataMember]
        public string EndTime { get; set; }

        [DataMember]
        public string Key { get; set; }
    }
}
