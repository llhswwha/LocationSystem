using BLL;
using TEntity= DbModel.Location.Authorizations.CardRole;

namespace LocationServices.Locations.Services
{
    public interface ITagRoleService:IEntityService<TEntity>
    {

    }
    public class TagRoleService 
        : EntityService<TEntity>
        , ITagRoleService
    {
        public TagRoleService():base()
        {
        }

        public TagRoleService(Bll bll) : base(bll)
        {
        }

        protected override void SetDbSet()
        {
            dbSet = db.CardRoles;
        }
    }
}
