using DbModel.LocationHistory.AreaAndDev;
using IModel;
using Location.IModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.Location.AreaAndDev
{
    /// <summary>
    /// 设备监控点
    /// </summary>
    public class DevMonitorNode: IDevMonitorNode
    {
        [DataMember]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [DataMember]
        [Display(Name = "标签名")]
        [MaxLength(128)]
        public string TagName { get; set; }

        [DataMember]
        [Display(Name = "数据库标签名")]
        [MaxLength(128)]
        public string DbTagName { get; set; }

        [DataMember]
        [Display(Name = "描述")]
        [MaxLength(128)]
        public string Describe { get; set; }

        [DataMember]
        [Display(Name = "值")]
        [MaxLength(32)]
        public string Value { get; set; }

        [DataMember]
        [Display(Name = "单位")]
        [MaxLength(16)]
        public string Unit { get; set; }

        [DataMember]
        [Display(Name = "数据类型")]
        [MaxLength(16)]
        public string DataType { get; set; }

        [DataMember]
        [Display(Name = "KKS")]
        [MaxLength(128)]
        public string KKS { get; set; }

        [DataMember]
        [Display(Name = "KKS")]
        [MaxLength(128)]
        public string ParentKKS { get; set; }

        [DataMember]
        [Display(Name = "时间戳")]
        public int Time { get; set; }


        public DevMonitorNode()
        {
            TagName = "";
            DbTagName = "";
            Describe = "";
            Value = "";
            Unit = "";
            DataType = "";
            KKS = "";
            ParentKKS = "";
            Time = 0;
        }

        public DevMonitorNodeHistory ToHistory()
        {
            DevMonitorNodeHistory history = new DevMonitorNodeHistory();
            history.TagName = this.TagName;
            history.DbTagName = this.DbTagName;
            history.Describe = this.Describe;
            history.Value = this.Value;
            history.Unit = this.Unit;
            history.DataType = this.DataType;
            history.KKS = this.KKS;
            history.ParentKKS = this.ParentKKS;
            history.Time = this.Time;

            return history;
        }
    }
}
