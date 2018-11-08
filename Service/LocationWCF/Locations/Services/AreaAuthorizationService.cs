using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using BLL.Blls.Location;
using DbModel.Location.Work;
using Location.IModel;
using TModel.Tools;
using TEntity= DbModel.Location.Work.AreaAuthorization;

namespace LocationServices.Locations.Services
{
    public interface IAreaAuthorizationService:IEntityService<TEntity>
    {
        IList<TEntity> GetListByArea(string area);
    }
    public class AreaAuthorizationService
        : EntityService<TEntity>
        ,IAreaAuthorizationService
    {
        public AreaAuthorizationService():base()
        {
        }

        public AreaAuthorizationService(Bll bll) : base(bll)
        {
        }

        protected override void SetDbSet()
        {
            dbSet = db.AreaAuthorizations;
        }

        public IList<TEntity> GetListByArea(string area)
        {
            int areaId = area.ToInt();
            return dbSet.Where(i => i.AreaId== areaId);
        }
    }
}
