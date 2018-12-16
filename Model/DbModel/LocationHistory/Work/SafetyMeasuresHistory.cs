using DbModel.Tools;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Location.TModel.Tools;
using Location.IModel;

namespace DbModel.LocationHistory.Work
{
    public class SafetyMeasuresHistory:IId
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember]
        [Display(Name = "Id")]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [DataMember]
        [Display(Name = "序号")]
        public int No { get; set; }

        /// <summary>
        /// 签发人填写的安全措施
        /// </summary>
        [DataMember]
        [Display(Name = "签发人填写的安全措施")]
        [MaxLength(256)]
        public string LssuerContent { get; set; }

        /// <summary>
        /// 许可人填写的安全措施
        /// </summary>
        [DataMember]
        [Display(Name = "许可人填写的安全措施")]
        [MaxLength(256)]
        public string LicensorContent { get; set; }

        /// <summary>
        /// 工作票id
        /// </summary>
        [DataMember]
        [Display(Name = "工作票id")]
        public int? WorkTicketId { get; set; }

        public SafetyMeasuresHistory Clone()
        {
            return this.CloneObjectByBinary();
        }
    }
}
