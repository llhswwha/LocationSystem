using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using TModel.Tools;
using TEntity = DbModel.Location.Work.AreaAuthorizationRecord;

namespace LocationServices.Locations.Services
{
    public interface IAreaAuthorizationRecordService : IEntityService<TEntity>
    {
        IList<TEntity> GetListByArea(string area);

        IList<TEntity> GetListByRole(string role);

        IList<TEntity> GetListByTag(string role);

        IList<TEntity> GetListByPerson(string role);
    }
    public class AreaAuthorizationRecordService
        : EntityService<TEntity>
        , IAreaAuthorizationRecordService
    {
        public AreaAuthorizationRecordService():base()
        {
        }

        public AreaAuthorizationRecordService(Bll bll) : base(bll)
        {
        }
        protected override void SetDbSet()
        {
            dbSet = db.AreaAuthorizationRecords;
        }

        public IList<TEntity> GetListByArea(string area)
        {
            int areaId = area.ToInt();
            return dbSet.Where(i => i.AreaId == areaId);
        }

        public IList<TEntity> GetListByRole(string role)
        {
            int roleId = role.ToInt();
            return dbSet.Where(i => i.CardRoleId == roleId);
        }

        public IList<TEntity> GetListByTag(string tagId)
        {
            var tag = db.LocationCards.Find(tagId.ToInt());
            if (tag == null) return null;
            return dbSet.Where(i => i.CardRoleId == tag.CardRoleId);
        }

        public IList<TEntity> GetListByPerson(string personId)
        {
            var id = personId.ToInt();
            var tp=db.LocationCardToPersonnels.Find(i=>i.PersonnelId== id);
            var tag = db.LocationCards.Find(tp.LocationCardId);
            if (tag == null) return null;
            return dbSet.Where(i => i.CardRoleId == tag.CardRoleId);
        }
    }
}
