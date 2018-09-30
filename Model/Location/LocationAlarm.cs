using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Location.Model
{
    /// <summary>
    /// 定位告警
    /// </summary>
    [DataContract]
    public class LocationAlarm
    {
        [DataMember]
        public int Id { get; set; }


        private int _type = 0;

        /// <summary>
        /// 告警类型：0:区域告警，1:消失告警，2:低电告警，3:传感器告警，4:重启告警，5:非法拆卸
        /// </summary>
        [DataMember]
        public int Type
        {
            get { return _type; }
            set
            {
                _type = value;

                if (_type == 0)
                {
                    TypeName= "区域告警";
                }
                else if (_type == 1)
                {
                    TypeName = "消失告警";
                }
                else if (_type == 2)
                {
                    TypeName = "低电告警";
                }
                else if (_type == 3)
                {
                    TypeName = "传感器告警";
                }
                else if (_type == 4)
                {
                    TypeName = "重启告警";
                }
                else if (_type == 5)
                {
                    TypeName = "非法拆卸";
                }
                else
                {
                    TypeName = "其他告警";
                }
            }
        }

        [DataMember]
        [NotMapped]
        public string TypeName { get; set; }

        private int _level = 0;

        /// <summary>
        /// 告警等级：
        /// </summary>
        [DataMember]
        public int Level { get {return _level;}
            set
            {
                _level = value;
                if (_level == 1)
                {
                    LevelName= "一级告警";
                }
                else if (_level == 2)
                {
                    LevelName= "二级告警";
                }
                else if (_level == 3)
                {
                    LevelName= "三级告警";
                }
                else if (_level == 4)
                {
                    LevelName= "四级告警";
                }
                else
                {
                    LevelName= "其他告警";
                }
            } }

        [NotMapped]
        [DataMember]
        public string LevelName { get; set; }

        /// <summary>
        /// 告警终端
        /// </summary>
        [DataMember]
        public int? TagId { get; set; }

        /// <summary>
        /// 告警终端
        /// </summary>
        [DataMember]
        public virtual Tag Tag { get; set; }

        [DataMember]
        public int? TargetId { get; set; }

        /// <summary>
        /// 告警目标(?)
        /// </summary>
        [DataMember]
        public virtual Personnel Target { get; set; }

        ///// <summary>
        ///// 告警区域名称
        ///// </summary>
        //public string Area { get; set; }

        /// <summary>
        /// 告警内容
        /// </summary>
        [DataMember]
        public string Content { get; set; }

        /// <summary>
        /// 告警时间
        /// </summary>
        [DataMember]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 处理时间
        /// </summary>
        [DataMember]
        public DateTime HandleTime { get; set; }

        /// <summary>
        /// 处理人
        /// </summary>
        [DataMember]
        public string Handler { get; set; }

        /// <summary>
        /// 处理类型：误报，忽略，确认
        /// </summary>
        [DataMember]
        public int HandleType { get; set; }

        public LocationAlarm()
        {
            CreateTime = DateTime.Now;
        }

        public LocationAlarm SetPerson(Personnel p)
        {
            TargetId = p.Id; Target = p; Tag = p.Tag; TagId = p.TagId;
            return this;
        }
        
    }
}
