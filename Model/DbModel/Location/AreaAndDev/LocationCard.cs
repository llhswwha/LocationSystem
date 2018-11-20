using DbModel.LocationHistory.AreaAndDev;
using System;
using System.ComponentModel.DataAnnotations;
using Location.IModel.Locations;
using Location.TModel.Tools;

namespace DbModel.Location.AreaAndDev
{
    /// <summary>
    /// 定位卡
    /// </summary>
    public class LocationCard:ITag
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Display(Name = "主键Id")]
        public int Id { get; set; }

        /// <summary>
        /// 对接Id
        /// </summary>
        [Display(Name = "对接Id")]
        public int? Abutment_Id { get; set; }

        /// <summary>
        /// 终端编号
        /// </summary>
        [Display(Name = "终端编号")]
        [MaxLength(16)]
        [Required]
        public string Code { get; set; }

        /// <summary>
        /// 终端名称
        /// </summary>
        [Display(Name = "终端名称")]
        [MaxLength(128)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Display(Name = "描述")]
        [MaxLength(128)]
        public string Describe { get; set; }


        [Display(Name = "标签角色")]
        public int? CardRoleId { get; set; }

        /// <summary>
        /// 电量（伏*100)
        /// </summary>
        [Display(Name = "电量")]
        public int Power { get; set; }

        /// <summary>
        /// 电量状态,0表示正常，1表示弱电
        /// </summary>
        [Display(Name = "电量状态")]
        public int PowerState { get; set; }

        /// <summary>
        /// 不知道什么信息 格式是 0:0:0:0:0 或者 0:0:0:0:1。
        /// 感觉是卡不动时会发1，动时发0。可能用:分开，不同位有不同作用
        /// 补充：卡大约20秒中不动后，会发0:0:0:0:1，然后再不动大约10秒后，不发位置信息
        /// </summary>
        [Display(Name = "信息")]
        [MaxLength(16)]
        public string Flag { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public LocationCard Clone()
        {
            return this.CloneObjectByBinary();
        }

        public LocationCardHistory RemoveToHistory()
        {
            LocationCardHistory history = new LocationCardHistory();
            history.Id = this.Id;
            history.Abutment_Id = this.Abutment_Id;
            history.Code = this.Code;
            history.Name = this.Name;
            history.Describe = this.Describe;
            history.HistoryTime = DateTime.Now;
            history.HistoryTimeStamp = TimeConvert.DateTimeToTimeStamp(history.HistoryTime);

            return history;
        }
    }
}
