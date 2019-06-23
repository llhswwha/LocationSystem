using DbModel.Location.AreaAndDev;
using DbModel.Location.Person;
using DbModel.LocationHistory.Relation;
using DbModel.Tools;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Location.TModel.Tools;
using Location.IModel;

namespace DbModel.Location.Relation
{
    /// <summary>
    /// 定位卡与人员之间的关系
    /// </summary>
    public class LocationCardToPersonnel:IId
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [DataMember]
        [Display(Name = "主键Id")]
        public int Id { get; set; }

        /// <summary>
        /// 定位卡
        /// </summary>
        [DataMember]
        [Display(Name = "定位卡")]
        public int LocationCardId { get; set; }
        public virtual LocationCard LocationCard { get; set; }

        /// <summary>
        /// 人员
        /// </summary>
        [DataMember]
        [Display(Name = "人员")]
        public int PersonnelId { get; set; }
        public virtual Personnel Personnel { get; set; }

        public LocationCardToPersonnel Clone()
        {
            LocationCardToPersonnel copy = new LocationCardToPersonnel();
            copy = this.CloneObjectByBinary();
            copy.LocationCard = null;
            copy.Personnel = null;

            if (this.LocationCard != null)
            {
                copy.LocationCard = this.LocationCard.Clone();
            }

            if (this.Personnel != null)
            {
                copy.Personnel = this.Personnel.Clone();
            }

            return copy;
        }

        public LocationCardToPersonnelHistory RemoveToHistory()
        {
            LocationCardToPersonnelHistory history = new LocationCardToPersonnelHistory();
            history.Id = this.Id;
            history.LocationCardId = this.LocationCardId;
            history.PersonnelId = this.PersonnelId;

            history.HistoryTime = DateTime.Now;
            history.HistoryTimeStamp = TimeConvert.ToStamp(history.HistoryTime);

            return history;
        }
    }
}
