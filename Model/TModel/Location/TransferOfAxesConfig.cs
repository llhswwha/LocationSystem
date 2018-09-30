using System.Runtime.Serialization;
using Location.TModel.Location.AreaAndDev;
using System;

namespace Location.TModel.Location
{
    [DataContract] [Serializable]
    public class TransferOfAxesConfig
    {
        [DataMember]
        public ConfigArg Zero { get; set; }
        [DataMember]
        public ConfigArg Scale { get; set; }
        [DataMember]
        public ConfigArg Direction { get; set; }    
    }
}
