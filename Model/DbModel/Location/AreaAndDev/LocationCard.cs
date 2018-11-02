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
        [Required]
        public string Code { get; set; }

        /// <summary>
        /// 终端名称
        /// </summary>
        [Display(Name = "终端名称")]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Display(Name = "描述")]
        public string Describe { get; set; }


        [Display(Name = "标签角色")]
        public int? CardRoleId { get; set; }

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
