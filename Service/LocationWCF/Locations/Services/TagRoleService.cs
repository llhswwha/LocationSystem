using BLL;
using TModel.Tools;
using TEntity= DbModel.Location.Authorizations.CardRole;

namespace LocationServices.Locations.Services
{
    public interface ITagRoleService:IEntityService<TEntity>
    {
        TEntity GetEntityByTag(string tag);
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

        public TEntity GetEntityByTag(string tagId)
        {
            var tag = db.LocationCards.Find(tagId.ToInt());
            return dbSet.Find(tag.CardRoleId);
        }
    }
}
