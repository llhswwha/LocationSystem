using DAL;
using DbModel.Location.Relation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Blls.Location
{
    public class LocationCardToAreaBll : BaseBll<LocationCardToArea, LocationDb>
    {
        public LocationCardToAreaBll():base()
        {

        }
        public LocationCardToAreaBll(LocationDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.LocationCardToAreas;
        }
        /// <summary>
        /// 定位卡Code对应AreaId
        /// </summary>
        /// <returns></returns>
        public Dictionary<string,int>ToDictionary()
        {
            List<LocationCardToArea>infoList = Db.LocationCardToAreas.ToList();
            Dictionary<int, DbModel.Location.AreaAndDev.LocationCard> cardDic = Db.LocationCards.ToDictionary(i=>i.Id);
            Dictionary<string, int> codeToAreaDic = new Dictionary<string, int>();
            foreach(var item in infoList)
            {
                if(cardDic.ContainsKey(item.LocationCardId))
                {
                    DbModel.Location.AreaAndDev.LocationCard card = cardDic[item.LocationCardId];
                    if (!codeToAreaDic.ContainsKey(card.Code)) codeToAreaDic.Add(card.Code,item.AreaId);
                }
            }
            return codeToAreaDic;
        }
        /// <summary>
        /// 获取定位卡号,关系顺序编号(作用：根据位置中的卡号，按顺序排序)
        /// </summary>
        /// <returns></returns>
        public Dictionary<string,int>GetCardToRelationIndex()
        {
            List<LocationCardToArea> infoList = Db.LocationCardToAreas.ToList();
            Dictionary<int, DbModel.Location.AreaAndDev.LocationCard> cardDic = Db.LocationCards.ToDictionary(i => i.Id);
            Dictionary<string, int> codeToAreaDic = new Dictionary<string, int>();
            foreach (var item in infoList)
            {
                if (cardDic.ContainsKey(item.LocationCardId))
                {
                    DbModel.Location.AreaAndDev.LocationCard card = cardDic[item.LocationCardId];
                    if (!codeToAreaDic.ContainsKey(card.Code)) codeToAreaDic.Add(card.Code, item.Id);
                }
            }
            return codeToAreaDic;
        }
    }
}
