using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Location.Model.LocationTables;

namespace Location.Model
{
    [DataContract]
    public class DeviceAlarm
    {
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [DataMember]
        public string Message { get; set; }


        private int _level = 0;

        /// <summary>
        /// 告警等级：
        /// </summary>
        [DataMember]
        public int Level
        {
            get { return _level; }
            set
            {
                _level = value;
                if (_level == 1)
                {
                    LevelName = "一级告警";
                }
                else if (_level == 2)
                {
                    LevelName = "二级告警";
                }
                else if (_level == 3)
                {
                    LevelName = "三级告警";
                }
                else if (_level == 4)
                {
                    LevelName = "四级告警";
                }
                else
                {
                    LevelName = "其他告警";
                }
            }
        }

        [NotMapped]
        [DataMember]
        public string LevelName { get; set; }

        /// <summary>
        /// 告警时间
        /// </summary>
        [DataMember]
        public DateTime CreateTime { get; set; }

        [DataMember]
        public int? DevId { get; set; }

        [DataMember]
        public DevInfo Dev { get; set; }

        public DeviceAlarm SetDev(DevInfo dev)
        {
            Dev = dev;
            DevId = dev.Id;
            return this;
        }
    }
}
