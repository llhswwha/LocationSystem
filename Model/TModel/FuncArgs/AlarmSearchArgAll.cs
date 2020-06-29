using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TModel.FuncArgs
{
    [DataContract]
    [Serializable]
    public class AlarmSearchArgAll
    {
        [DataMember]
        public int[] personnels { get; set; }
        [DataMember]
        public DateTime startTime { get; set; }
        [DataMember]
        public DateTime endTime { get; set; }
    }
}
