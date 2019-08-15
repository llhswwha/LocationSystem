using DAL;
using DbModel.Location.Relation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Blls.Location
{
    public class LocationCardToPersonnelBll : BaseBll<LocationCardToPersonnel, LocationDb>
    {
        public LocationCardToPersonnelBll():base()
        {

        }
        public LocationCardToPersonnelBll(LocationDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.LocationCardToPersonnels;
        }

        public Dictionary<int, int> GetTagToPerson()
        {
            Dictionary<int, int> dict = new Dictionary<int, int>();
            var list = this.ToList();
            foreach (var item in list)
            {
                if (dict.ContainsKey(item.LocationCardId))
                {
                    dict[item.LocationCardId] = item.PersonnelId;
                }
                else
                {
                    dict.Add(item.LocationCardId, item.PersonnelId);
                }
            }
            return dict;
        }

        public Dictionary<int, int> GetPersonToTag()
        {
            Dictionary<int, int> dict = new Dictionary<int, int>();
            var list = this.ToList();
            foreach (var item in list)
            {
                if (dict.ContainsKey(item.PersonnelId))
                {
                    dict[item.PersonnelId] = item.LocationCardId;
                }
                else
                {
                    dict.Add(item.PersonnelId, item.LocationCardId);
                }
            }
            return dict;
        }
    }
}
