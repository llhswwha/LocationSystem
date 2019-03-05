using DbModel.LocationHistory.Data;
using DbModel.Tools;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Location.TModel.Tools;
using Location.IModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbModel.Location.Data
{
    /// <summary>
    /// 设备实时数据
    /// </summary>
    public class DevInstantData:IEntity<string>
    {
        /// <summary>
        /// KKS码
        /// </summary>
        [DataMember]
        [Display(Name = "KKS码")]
        [MaxLength(32)]
        [Column("KKS")]
        [Key]
        public string Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        [Display(Name = "名称")]
        [MaxLength(128)]
        public string Name { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        [DataMember]
        [Display(Name = "值")]
        [MaxLength(32)]
        public string Value { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        [DataMember]
        [Display(Name = "单位")]
        [MaxLength(8)]
        public string Unit { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        [DataMember]
        [Display(Name = "时间")]
        public DateTime DateTime { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        [DataMember]
        [Display(Name = "时间戳")]
        public long DateTimeStamp { get; set; }

        public DevInstantData()
        {
            DateTime = DateTime.Now;
            DateTimeStamp = TimeConvert.DateTimeToTimeStamp(DateTime);
        }

        public DevInstantData Clone()
        {
            DevInstantData copy = new DevInstantData();
            copy = this.CloneObjectByBinary();

            return copy;
        }

        public DevInstantDataHistory RemoveToHistory()
        {
            DevInstantDataHistory history = new DevInstantDataHistory();
            history.KKS = this.Id;
            history.Name = this.Name;
            history.Value = this.Value;
            history.Unit = this.Unit;
            history.DateTime = this.DateTime;
            history.DateTimeStamp = this.DateTimeStamp;
            
            return history;
        }
    }
}
