using BLL;
using TModel.Tools;
using TEntity= DbModel.Location.Authorizations.CardRole;

namespace LocationServices.Locations.Services
{
    public interface ITagRoleService:INameEntityService<TEntity>
    {
        TEntity GetEntityByTag(string tag);

        TEntity GetEntityByPerson(string person);
    }
    public class TagRoleService 
        : NameEntityService<TEntity>
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

        public TEntity GetEntityByPerson(string person)
        {
            int id = person.ToInt();
            var tp = db.LocationCardToPersonnels.Find(i => i.PersonnelId == id);
            var tag = db.LocationCards.Find(tp.LocationCardId);
            return dbSet.Find(tag.CardRoleId);
        }
    }
}
