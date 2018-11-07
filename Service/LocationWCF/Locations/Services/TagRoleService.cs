using BLL;
using BLL.Blls.Location;
using DbModel.Location.Authorizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TModel.Tools;

namespace LocationServices.Locations.Services
{
    public interface ITagRoleService:IEntityService<CardRole>
    {

    }
    public class TagRoleService : ITagRoleService
    {
        private Bll db;

        private CardRoleBll dbSet;

        public TagRoleService()
        {
            db = new Bll(false, false, false, false);
            dbSet = db.CardRoles;
        }

        public TagRoleService(Bll bll)
        {
            this.db = bll;
            dbSet = db.CardRoles;
        }

        public CardRole Delete(string id)
        {
            var item = dbSet.DeleteById(id.ToInt());
            return item;
        }

        public CardRole GetEntity(string id)
        {
            return dbSet.Find(id.ToInt());
        }

        public IList<CardRole> GetList()
        {
            return dbSet.ToList();
        }

        public IList<CardRole> GetListByName(string name)
        {
            return dbSet.Where(i => i.Name.Contains(name));
        }

        public CardRole Post(CardRole item)
        {
            bool result = dbSet.Add(item);
            return result ? item : null;
        }

        public CardRole Put(CardRole item)
        {
            bool result = dbSet.Edit(item);
            return result ? item : null;
        }
    }
}
