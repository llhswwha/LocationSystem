using DbModel.Location.AreaAndDev;
using DbModel.Location.Person;
using DbModel.LocationHistory.Relation;
using DbModel.Tools;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Location.TModel.Tools;

namespace DbModel.Location.Relation
{
    /// <summary>
    /// 门禁卡与人员之间的关系
    /// </summary>
    public class EntranceGuardCardToPersonnel
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [DataMember]
        [Display(Name = "主键Id")]
        public int Id { get; set; }

        /// <summary>
        /// 门禁卡
        /// </summary>
        [DataMember]
        [Display(Name = "门禁卡")]
        public int EntranceGuardCardId { get; set; }
        public virtual EntranceGuardCard EntranceGuardCard { get; set; }

        /// <summary>
        /// 人员
        /// </summary>
        [DataMember]
        [Display(Name = "人员")]
        public int PersonnelId { get; set; }
        public virtual Personnel Personnel { get; set; }

        public EntranceGuardCardToPersonnel Clone()
        {
            EntranceGuardCardToPersonnel copy = new EntranceGuardCardToPersonnel();
            copy = this.CloneObjectByBinary();
            copy.EntranceGuardCard = null;
            copy.Personnel = null;

            if (this.EntranceGuardCard != null)
            {
                copy.EntranceGuardCard = this.EntranceGuardCard.Clone();
            }

            if (this.Personnel != null)
            {
                copy.Personnel = this.Personnel.Clone();
            }

            return copy;
        }

        public EntranceGuardCardToPersonnelHistory RemoveToHistory()
        {
            EntranceGuardCardToPersonnelHistory history = new EntranceGuardCardToPersonnelHistory();
            history.Id = this.Id;
            history.EntranceGuardCardId = this.EntranceGuardCardId;
            history.PersonnelId = this.PersonnelId;
            history.HistoryTime = DateTime.Now;
            history.HistoryTimeStamp = TimeConvert.DateTimeToTimeStamp(history.HistoryTime);

            return history;
        }
    }
}
