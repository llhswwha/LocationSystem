using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Location.IModel.Locations;

namespace Location.TModel
{
    [DataContract]
    public class TPost:IPost
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        public override string ToString()
        {
            return string.Format("id:{0},name:{1}", Id, Name);
        }
    }
}
