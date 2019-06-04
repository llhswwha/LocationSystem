using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TModel.BaseData
{
    [DataContract]
    public class Page<T>
    {
        [DataMember]
        public int PageIndex { get; set; }

        [DataMember]
        public int PageSize { get; set; }

        [DataMember]
        public int PageCount { get; set; }

        [DataMember]
        public int TotalCount { get; set; }

        [DataMember]
        public List<T> Content { get; set; }
    }
}
