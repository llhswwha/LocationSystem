using DAL;
using DbModel.LocationHistory.AreaAndDev;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TModel.LocationHistory.AreaAndDev;
using TModel.Tools;

namespace BLL.Blls.LocationHistory
{
    public class DevEntranceGuardCardActionBll : BaseBll<DevEntranceGuardCardAction, LocationHistoryDb>
    {
        public DevEntranceGuardCardActionBll() : base()
        {

        }
        public DevEntranceGuardCardActionBll(LocationHistoryDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.DevEntranceGuardCardActions;
        }

        public PageInfo<DevEntranceGuardCardsHistroy> getPageByCondition(string device_id, DateTime startTime, DateTime endTime, int pageIndex, int pageSize)
        {
            string sqlwhere = "";
            if (!string.IsNullOrEmpty(device_id))
            {
                sqlwhere += " and device_id='"+device_id+"'";
            }
            if (startTime != null && endTime != null)
            {
                sqlwhere += " and  OperateTime between  '"+startTime+"' and  '"+endTime+"'";
            }        
            string sqlCount = string.Format(@"select count(id) from deventranceguardcardactions");
            string strsql = string.Format(@"select * from( select a.id,DevInfoId,EntranceGuardCardId,OperateTime,OperateTimeStamp,`Code`,description,device_id,card_code,personnelAbutment_Id,b.id as PersonnelId, b.`Name`
                                            from deventranceguardcardactions  a  left join location.personnels  b on a.personnelAbutment_Id=b. Abutment_Id) t where 1=1 "+sqlwhere);
            PageInfo<DevEntranceGuardCardsHistroy> page = GetPageList<DevEntranceGuardCardsHistroy>(sqlCount, strsql, pageIndex, pageSize);
            return page;
        }

    }
}
