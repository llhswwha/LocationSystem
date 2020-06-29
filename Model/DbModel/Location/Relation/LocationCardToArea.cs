using DbModel.Location.AreaAndDev;
using Location.IModel;
using Location.TModel.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.Location.Relation
{
    public class LocationCardToArea:IId
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
        [Display(Name = "区域")]
        public int AreaId { get; set; }
        public virtual Area Area { get; set; }

        public LocationCardToArea Clone()
        {
            LocationCardToArea copy = new LocationCardToArea();
            copy = this.CloneObjectByBinary();
            copy.LocationCard = null;
            copy.Area = null;

            if (this.LocationCard != null)
            {
                copy.LocationCard = this.LocationCard.Clone();
            }

            if (this.Area != null)
            {
                copy.Area = this.Area.Clone();
            }
            return copy;
        }

    }
}
