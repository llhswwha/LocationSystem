using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TModel.BaseData
{
    [DataContract]
    public class ReturnInfo
    {
        [DataMember]
        public bool Result { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }

        [DataMember]
        public int IntData { get; set; }

        public ReturnInfo()
        {

        }

        public ReturnInfo(bool r,string e)
        {
            Result = r;
            ErrorMessage = e;
        }
    }
}
