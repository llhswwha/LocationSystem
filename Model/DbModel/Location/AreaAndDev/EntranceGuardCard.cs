using DbModel.LocationHistory.AreaAndDev;
using DbModel.Tools;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Location.TModel.Tools;

namespace DbModel.Location.AreaAndDev
{
    /// <summary>
    /// 门禁卡
    /// </summary>
    public class EntranceGuardCard
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [DataMember]
        [Display(Name = "主键Id")]
        public int Id { get; set; }

        /// <summary>
        /// 对接Id
        /// </summary>
        [DataMember]
        [Display(Name = "对接Id")]
        public int? Abutment_Id { get; set; }

        /// <summary>
        /// 门禁卡号
        /// </summary>
        [DataMember]
        [Display(Name = "门禁卡号")]
        [MaxLength(64)]
        public string Code { get; set; }

        /// <summary>
        /// 卡状态，0 未激活，1激活 使用中，2挂失 暂停
        /// </summary>
        [DataMember]
        [Display(Name = "卡状态")]
        public int State { get; set; }

        public EntranceGuardCard Clone()
        {
            EntranceGuardCard copy = new EntranceGuardCard();
            copy = this.CloneObjectByBinary();

            return copy;
        }

        public EntranceGuardCardHistory RemoveToHistory()
        {
            EntranceGuardCardHistory history = new EntranceGuardCardHistory();
            history.Id = this.Id;
            history.Abutment_Id = this.Abutment_Id;
            history.Code = this.Code;
            history.State = this.State;
           
            history.HistoryTime = DateTime.Now;
            history.HistoryTimeStamp = TimeConvert.DateTimeToTimeStamp(history.HistoryTime);

            return history;
        }
    }
}
